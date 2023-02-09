// Copyright 2022 Carnegie Mellon University. All Rights Reserved.
// Released under a MIT (SEI)-style license, please see LICENSE.md in the project root for license information or contact permission@sei.cmu.edu for full terms.

using AutoMapper;
using Blueprint.Api.Infrastructure.Authorization;
using Blueprint.Api.Infrastructure.Exceptions;
using Blueprint.Api.Infrastructure.Extensions;
using Blueprint.Api.Infrastructure.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Blueprint.Api.Data;
using Blueprint.Api.Data.Enumerations;
using Blueprint.Api.Data.Models;
using Gallery.Api.Client;

namespace Blueprint.Api.Services
{
    public interface IGalleryService
    {
        Task<ViewModels.Msel> PushToGalleryAsync(Guid mselId, CancellationToken ct);
        Task<ViewModels.Msel> PullFromGalleryAsync(Guid mselId, CancellationToken ct);
    }

    public class GalleryService : IGalleryService
    {
        private readonly IGalleryApiClient _galleryApiClient;
        private readonly ResourceOwnerAuthorizationOptions _resourceOwnerAuthorizationOptions;
        private readonly ClaimsPrincipal _user;
        private readonly IAuthorizationService _authorizationService;
        private readonly BlueprintContext _context;
        protected readonly IMapper _mapper;
        private readonly ILogger<GalleryService> _logger;
        private readonly string _galleryDelivery = "Gallery";

        public GalleryService(
            IGalleryApiClient galleryApiClient,
            IPrincipal user,
            BlueprintContext mselContext,
            IMapper mapper,
            IAuthorizationService authorizationService,
            ILogger<GalleryService> logger,
            ResourceOwnerAuthorizationOptions resourceOwnerAuthorizationOptions)
        {
            _galleryApiClient = galleryApiClient;
            _resourceOwnerAuthorizationOptions = resourceOwnerAuthorizationOptions;
            _user = user as ClaimsPrincipal;
            _authorizationService = authorizationService;
            _context = mselContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ViewModels.Msel> PushToGalleryAsync(Guid mselId, CancellationToken ct)
        {
            // user must be a Content Developer or a MSEL owner
            if (!(await _authorizationService.AuthorizeAsync(_user, null, new ContentDeveloperRequirement())).Succeeded &&
                !(await MselOwnerRequirement.IsMet(_user.GetId(), mselId, _context)))
                throw new ForbiddenException();
            // get the MSEL and verify data state
            var msel = await _context.Msels
                .Include(m => m.Cards)
                .Include(m => m.DataFields)
                .Include(m => m.ScenarioEvents)
                .ThenInclude(se => se.DataValues)
                .SingleOrDefaultAsync(m => m.Id == mselId);
            if (msel == null)
                throw new EntityNotFoundException<MselEntity>($"MSEL {mselId} was not found when attempting to create a collection.");
            if (msel.GalleryCollectionId != null)
                throw new InvalidOperationException($"MSEL {mselId} is already associated to a Gallery Collection.");
            // start a transaction, because we will modify many database items
            await _context.Database.BeginTransactionAsync();
            // create the Gallery Collection
            await CreateCollectionAsync(msel, ct);
            // create the Gallery Exhibit
            await CreateExhibitAsync(msel, ct);
            // create the Gallery Teams
            await CreateTeamsAsync(msel, ct);
            // create the Gallery Cards
            await CreateCardsAsync(msel, ct);
            // create the Gallery Articles
            await CreateArticlesAsync(msel, ct);
            // commit the transaction
            await _context.Database.CommitTransactionAsync(ct);

            return _mapper.Map<ViewModels.Msel>(msel); 
        }

        public async Task<ViewModels.Msel> PullFromGalleryAsync(Guid mselId, CancellationToken ct)
        {
            // user must be a Content Developer or a MSEL owner
            if (!(await _authorizationService.AuthorizeAsync(_user, null, new ContentDeveloperRequirement())).Succeeded &&
                !(await MselOwnerRequirement.IsMet(_user.GetId(), mselId, _context)))
                throw new ForbiddenException();
            // get the MSEL and verify data state
            var msel = await _context.Msels.FindAsync(mselId);
            if (msel == null)
                throw new EntityNotFoundException<MselEntity>($"MSEL {mselId} was not found when attempting to remove from Gallery.");
            if (msel.GalleryCollectionId == null)
                throw new InvalidOperationException($"MSEL {mselId} is not associated to a Gallery Collection.");
            // delete
            try
            {
                await _galleryApiClient.DeleteCollectionAsync((Guid)msel.GalleryCollectionId, ct);
            }
            catch (System.Exception)
            {
            }
            // update the MSEL
            msel.GalleryCollectionId = null;
            msel.GalleryExhibitId = null;
            // update the MSEL Cards
            var cards = await _context.Cards
                .Where(c => c.MselId == msel.Id)
                .ToListAsync(ct);
            foreach (var card in cards)
            {
                card.GalleryId = null;
            }
            // save the changes
            await _context.SaveChangesAsync(ct);

            return _mapper.Map<ViewModels.Msel>(msel); 
        }

        //
        // Helper methods
        //

        // Create a Gallery Collection for this MSEL
        private async Task CreateCollectionAsync(MselEntity msel, CancellationToken ct)
        {
            Collection newCollection = new Collection() {
                Name = msel.Name,
                Description = msel.Description
            };
            newCollection = await _galleryApiClient.CreateCollectionAsync(newCollection, ct);
            // update the MSEL
            msel.GalleryCollectionId = newCollection.Id;
            await _context.SaveChangesAsync(ct);
        }

        // Create a Gallery Exhibit for this MSEL
        private async Task CreateExhibitAsync(MselEntity msel, CancellationToken ct)
        {
            Exhibit newExhibit = new Exhibit() {
                CollectionId = (Guid)msel.GalleryCollectionId,
                ScenarioId = null,
                CurrentMove = 0,
                CurrentInject = 0,

            };
            newExhibit = await _galleryApiClient.CreateExhibitAsync(newExhibit, ct);
            // update the MSEL
            msel.GalleryExhibitId = newExhibit.Id;
            await _context.SaveChangesAsync(ct);
        }

        // Create Gallery Teams for this MSEL
        private async Task CreateTeamsAsync(MselEntity msel, CancellationToken ct)
        {
            // get the Gallery teams, Gallery Users, and the Gallery TeamUsers
            var galleryTeams = await _galleryApiClient.GetTeamsAsync(ct);
            var exhibitTeams = await _galleryApiClient.GetTeamsByExhibitAsync((Guid)msel.GalleryExhibitId, ct);
            var galleryUserIds = (await _galleryApiClient.GetUsersAsync(ct)).Select(u => u.Id);
            var galleryTeamUsers = await _galleryApiClient.GetTeamUsersAsync(ct);
            // get the teams for this MSEL and loop through them
            var teams = await _context.MselTeams
                .Where(mt => mt.MselId == msel.Id)
                .Select(mt => mt.Team)
                .ToListAsync();
            foreach (var team in teams)
            {
                // if this team doesn't exist in Gallery, create it
                var galleryTeam = galleryTeams.FirstOrDefault(t => t.Id == team.Id);
                if (galleryTeam == null)
                {
                    galleryTeam = new Team() {
                        Id = team.Id,
                        Name = team.Name,
                        ShortName = team.ShortName
                    };
                    galleryTeam = await _galleryApiClient.CreateTeamAsync(galleryTeam, ct);
                }
                // add the gallery team to the gallery exhibit, if necessary
                var exhibitTeam = exhibitTeams.FirstOrDefault(et => et.Id == team.Id);
                if (exhibitTeam == null)
                {
                    var galleryExhibitTeam = new ExhibitTeam() {
                        TeamId = team.Id,
                        ExhibitId = (Guid)msel.GalleryExhibitId
                    };
                    await _galleryApiClient.CreateExhibitTeamAsync(galleryExhibitTeam, ct);
                }
                // get all of the users for this team and loop through them
                var users = await _context.TeamUsers
                    .Where(tu => tu.TeamId == team.Id)
                    .Select(tu => tu.User)
                    .ToListAsync(ct);
                foreach (var user in users)
                {
                    // if this user is not in Gallery, add it
                    if (!galleryUserIds.Contains(user.Id))
                    {
                        var newUser = new User() {
                            Id = user.Id,
                            Name = user.Name
                        };
                        await _galleryApiClient.CreateUserAsync(newUser, ct);
                    }
                    // if there is no Gallery TeamUser, create it
                    if (!galleryTeamUsers.Any(tu => tu.TeamId == galleryTeam.Id && tu.UserId == user.Id))
                    {
                        var teamUser = new TeamUser() {
                            TeamId = galleryTeam.Id,
                            UserId = user.Id
                        };
                        await _galleryApiClient.CreateTeamUserAsync(teamUser, ct);
                    }
                }
            }
        }

        // Create Gallery Cards for this MSEL
        private async Task CreateCardsAsync(MselEntity msel, CancellationToken ct)
        {
            foreach (var card in msel.Cards)
            {
                Card galleryCard = new Card() {
                    CollectionId = (Guid)msel.GalleryCollectionId,
                    Name = card.Name,
                    Description = card.Description,
                    Move = card.Move,
                    Inject = card.Inject
                };
                galleryCard = await _galleryApiClient.CreateCardAsync(galleryCard, ct);
                card.GalleryId = galleryCard.Id;
                await _context.SaveChangesAsync(ct);
                // create the Gallery Team Cards
                var teamIds = await _context.CardTeams
                    .Where(ct => ct.CardId == card.Id)
                    .Select(ct => ct.TeamId)
                    .ToListAsync(ct);
                foreach (var teamId in teamIds)
                {
                    var newTeamCard = new TeamCard() {
                        TeamId = teamId,
                        CardId = (Guid)card.GalleryId
                    };
                    await _galleryApiClient.CreateTeamCardAsync(newTeamCard, ct);
                }
            }
        }

        // Create Gallery Articles for this MSEL
        private async Task CreateArticlesAsync(MselEntity msel, CancellationToken ct)
        {
            var mselTeams = await _context.MselTeams
                .Where(mt => mt.MselId == msel.Id)
                .Select(mt => mt.Team)
                .ToListAsync(ct);
            foreach (var scenarioEvent in msel.ScenarioEvents)
            {
                var deliveryMethod = GetArticleValue(GalleryArticleParameter.DeliveryMethod.ToString(), scenarioEvent.DataValues, msel.DataFields);
                if (deliveryMethod.Contains(_galleryDelivery))
                {
                    Int32 move = 0;
                    Int32 inject = 0;
                    object status = Gallery.Api.Client.ItemStatus.Unused;
                    object sourceType = SourceType.News;
                    DateTime datePosted;
                    bool openInNewTab = false;
                    // get the Gallery Article values from the scenario event data values
                    var cardIdString = GetArticleValue(GalleryArticleParameter.CardId.ToString(), scenarioEvent.DataValues, msel.DataFields);
                    Guid? galleryCardId = null;
                    if (!String.IsNullOrWhiteSpace(cardIdString))
                    {
                        var card = msel.Cards.FirstOrDefault(c => c.Id == Guid.Parse(cardIdString));
                        galleryCardId = card.GalleryId;
                    }
                    var name = GetArticleValue(GalleryArticleParameter.Name.ToString(), scenarioEvent.DataValues, msel.DataFields);
                    var description = GetArticleValue(GalleryArticleParameter.Description.ToString(), scenarioEvent.DataValues, msel.DataFields);
                    Int32.TryParse(GetArticleValue(GalleryArticleParameter.Move.ToString(), scenarioEvent.DataValues, msel.DataFields), out move);
                    Int32.TryParse(GetArticleValue(GalleryArticleParameter.Inject.ToString(), scenarioEvent.DataValues, msel.DataFields), out inject);
                    Enum.TryParse(typeof(Gallery.Api.Client.ItemStatus), GetArticleValue(GalleryArticleParameter.Status.ToString(), scenarioEvent.DataValues, msel.DataFields), true, out status);
                    Enum.TryParse(typeof(SourceType), GetArticleValue(GalleryArticleParameter.SourceType.ToString(), scenarioEvent.DataValues, msel.DataFields), true, out sourceType);
                    var sourceName = GetArticleValue(GalleryArticleParameter.SourceName.ToString(), scenarioEvent.DataValues, msel.DataFields);
                    var url = GetArticleValue(GalleryArticleParameter.Url.ToString(), scenarioEvent.DataValues, msel.DataFields);
                    DateTime.TryParse(GetArticleValue(GalleryArticleParameter.DatePosted.ToString(), scenarioEvent.DataValues, msel.DataFields), out datePosted);
                    bool.TryParse(GetArticleValue(GalleryArticleParameter.OpenInNewTab.ToString(), scenarioEvent.DataValues, msel.DataFields), out openInNewTab);
                    // create the article
                    Article galleryArticle = new Article() {
                        CollectionId = (Guid)msel.GalleryCollectionId,
                        CardId = galleryCardId,
                        Name = name,
                        Description = description,
                        Move = move,
                        Inject = inject,
                        Status = status == null ? Gallery.Api.Client.ItemStatus.Unused : (Gallery.Api.Client.ItemStatus)status,
                        SourceType = sourceType == null ? SourceType.News : (SourceType)sourceType,
                        SourceName = sourceName,
                        Url = url,
                        DatePosted = datePosted,
                        OpenInNewTab = openInNewTab
                    };
                    galleryArticle = await _galleryApiClient.CreateArticleAsync(galleryArticle, ct);
                    // create the Gallery Team Articles
                    var toOrgs = GetArticleValue(GalleryArticleParameter.ToOrg.ToString(), scenarioEvent.DataValues, msel.DataFields).Split(",", StringSplitOptions.TrimEntries);
                    var teamIds = mselTeams
                        .Where(t => toOrgs.Contains("ALL") || toOrgs.Contains(t.ShortName))
                        .Select(t => t.Id);
                    foreach (var teamId in teamIds)
                    {
                        var newArticleTeam = new TeamArticle() {
                            ExhibitId = (Guid)msel.GalleryExhibitId,
                            TeamId = teamId,
                            ArticleId = galleryArticle.Id
                        };
                        await _galleryApiClient.CreateTeamArticleAsync(newArticleTeam, ct);
                    }
                }
            }
        }

        private string GetArticleValue(string key, ICollection<DataValueEntity> dataValues, ICollection<DataFieldEntity> dataFields)
        {
            var dataField = dataFields.SingleOrDefault(df => df.GalleryArticleParameter == key);
            var dataValue = dataField == null ? null : dataValues.SingleOrDefault(dv => dv.DataFieldId == dataField.Id);
            return dataValue == null ? "" : dataValue.Value;
        }

    }
}

