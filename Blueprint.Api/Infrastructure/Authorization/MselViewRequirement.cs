// Copyright 2022 Carnegie Mellon University. All Rights Reserved.
// Released under a MIT (SEI)-style license, please see LICENSE.md in the project root for license information or contact permission@sei.cmu.edu for full terms.

using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Blueprint.Api.Data;

namespace Blueprint.Api.Infrastructure.Authorization
{
    public static class MselViewRequirement
    {
        public static async Task<Boolean> IsMet(Guid userId, Guid? mselId, BlueprintContext blueprintContext)
        {
            var mselTeamIdList = await blueprintContext.Teams
                .Where(t => t.MselId == mselId)
                .Select(t => t.Id)
                .ToListAsync();
            var isSuccess = await blueprintContext.TeamUsers
                .Where(tu => tu.UserId == userId && mselTeamIdList.Contains(tu.TeamId))
                .AnyAsync();
            if (isSuccess)
            {
                isSuccess = await blueprintContext.UserMselRoles
                    .Where(umr => umr.UserId == userId &&
                        umr.MselId == mselId &&
                        (
                            umr.Role == Data.Enumerations.MselRole.Viewer ||
                            umr.Role == Data.Enumerations.MselRole.Editor ||
                            umr.Role == Data.Enumerations.MselRole.Approver ||
                            umr.Role == Data.Enumerations.MselRole.MoveEditor ||
                            umr.Role == Data.Enumerations.MselRole.Owner
                        )
                    )
                    .AnyAsync();
            }
            return isSuccess;
        }
    }
}

