// Copyright 2022 Carnegie Mellon University. All Rights Reserved.
// Released under a MIT (SEI)-style license. See LICENSE.md in the project root for license information.

using Blueprint.Api.Data.Models;
using Blueprint.Api.ViewModels;

namespace Blueprint.Api.Infrastructure.Mappings
{
    public class CardTeamProfile : AutoMapper.Profile
    {
        public CardTeamProfile()
        {
            CreateMap<CardTeamEntity, CardTeam>();

            CreateMap<CardTeam, CardTeamEntity>();
        }
    }
}


