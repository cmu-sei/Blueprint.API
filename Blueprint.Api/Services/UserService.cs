// Copyright 2022 Carnegie Mellon University. All Rights Reserved.
// Released under a MIT (SEI)-style license, please see LICENSE.md in the project root for license information or contact permission@sei.cmu.edu for full terms.

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Blueprint.Api.Data;
using Blueprint.Api.Data.Models;
using Blueprint.Api.Infrastructure.Extensions;
using Blueprint.Api.Infrastructure.Authorization;
using Blueprint.Api.Infrastructure.Exceptions;
using Blueprint.Api.ViewModels;

namespace Blueprint.Api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<ViewModels.User>> GetAsync(CancellationToken ct);
        Task<ViewModels.User> GetAsync(Guid id, CancellationToken ct);
        Task<IEnumerable<ViewModels.User>> GetByTeamAsync(Guid TeamId, CancellationToken ct);
        Task<ViewModels.User> CreateAsync(ViewModels.User user, CancellationToken ct);
        Task<ViewModels.User> UpdateAsync(Guid id, ViewModels.User user, CancellationToken ct);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct);
        Task<bool> ImportUsersAsync(FileForm form, CancellationToken ct);
    }

    public class UserService : IUserService
    {
        private readonly BlueprintContext _context;
        private readonly ClaimsPrincipal _user;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserClaimsService _userClaimsService;
        private readonly IMapper _mapper;

        public UserService(BlueprintContext context, IPrincipal user, IAuthorizationService authorizationService, IUserClaimsService userClaimsService, IMapper mapper)
        {
            _context = context;
            _user = user as ClaimsPrincipal;
            _authorizationService = authorizationService;
            _userClaimsService = userClaimsService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ViewModels.User>> GetAsync(CancellationToken ct)
        {
            if(!(await _authorizationService.AuthorizeAsync(_user, null, new FullRightsRequirement())).Succeeded &&
                !(await _context.TeamUsers.AnyAsync(tu => tu.UserId == _user.GetId()))
            )
                throw new ForbiddenException();

            var items = await _context.Users
                .ProjectTo<ViewModels.User>(_mapper.ConfigurationProvider, dest => dest.Permissions)
                .ToArrayAsync(ct);
            return items;
        }

        public async Task<ViewModels.User> GetAsync(Guid id, CancellationToken ct)
        {
            if (!(await _authorizationService.AuthorizeAsync(_user, null, new FullRightsRequirement())).Succeeded &&
                !((await _authorizationService.AuthorizeAsync(_user, null, new BaseUserRequirement())).Succeeded && id == _user.GetId()))
                throw new ForbiddenException();

            var item = await _context.Users
                .ProjectTo<ViewModels.User>(_mapper.ConfigurationProvider, dest => dest.Permissions)
                .SingleOrDefaultAsync(o => o.Id == id, ct);
            return item;
        }

        public async Task<IEnumerable<ViewModels.User>> GetByTeamAsync(Guid teamId, CancellationToken ct)
        {
            if (!(await _authorizationService.AuthorizeAsync(_user, null, new FullRightsRequirement())).Succeeded)
                throw new ForbiddenException();

            var items = await _context.TeamUsers
                .Where(tu => tu.TeamId == teamId)
                .Select(tu => tu.User)
                .ToListAsync(ct);

            return _mapper.Map<IEnumerable<User>>(items);
        }

        public async Task<ViewModels.User> CreateAsync(ViewModels.User user, CancellationToken ct)
        {
            if (!(await _authorizationService.AuthorizeAsync(_user, null, new FullRightsRequirement())).Succeeded)
                throw new ForbiddenException();

            user.DateCreated = DateTime.UtcNow;
            user.CreatedBy = _user.GetId();
            user.DateModified = null;
            user.ModifiedBy = null;
            var userEntity = _mapper.Map<UserEntity>(user);

            _context.Users.Add(userEntity);
            await _context.SaveChangesAsync(ct);

            return await GetAsync(user.Id, ct);
        }

        public async Task<ViewModels.User> UpdateAsync(Guid id, ViewModels.User user, CancellationToken ct)
        {
            if (!(await _authorizationService.AuthorizeAsync(_user, null, new FullRightsRequirement())).Succeeded)
                throw new ForbiddenException();

            // Don't allow changing your own Id
            if (id == _user.GetId() && id != user.Id)
            {
                throw new ForbiddenException("You cannot change your own Id");
            }

            var userToUpdate = await _context.Users.SingleOrDefaultAsync(v => v.Id == id, ct);

            if (userToUpdate == null)
                throw new EntityNotFoundException<User>();

            user.CreatedBy = userToUpdate.CreatedBy;
            user.DateCreated = userToUpdate.DateCreated;
            user.ModifiedBy = _user.GetId();
            user.DateModified = DateTime.UtcNow;
            _mapper.Map(user, userToUpdate);

            _context.Users.Update(userToUpdate);
            await _context.SaveChangesAsync(ct);

            return await GetAsync(id, ct);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        {
            if (!(await _authorizationService.AuthorizeAsync(_user, null, new FullRightsRequirement())).Succeeded)
                throw new ForbiddenException();

            if (id == _user.GetId())
            {
                throw new ForbiddenException("You cannot delete your own account");
            }

            var userToDelete = await _context.Users.SingleOrDefaultAsync(v => v.Id == id, ct);

            if (userToDelete == null)
                throw new EntityNotFoundException<User>();

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync(ct);

            return true;
        }

        public async Task<bool> ImportUsersAsync(FileForm form, CancellationToken ct)
        {
            var noProblems = true;
            // user must be a Content Developer
            if (!(await _authorizationService.AuthorizeAsync(_user, null, new ContentDeveloperRequirement())).Succeeded)
                throw new ForbiddenException();

            var uploadItem = form.ToUpload;
            using (var reader = new StreamReader(uploadItem.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var newUsers = csv.GetRecords<UserInfo>();
                foreach (var userInfo in newUsers)
                {
                    
                }
            }
            await _context.SaveChangesAsync(ct);

            return noProblems;
        }

    }

    public class UserInfo
    {
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public string Email { get; set; }
        public string TeamName { get; set; }
    }
}

