using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DnDManager.Models
{
    public class IsDm : AuthorizationHandler<UserTypeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserTypeRequirement requirement)
        {
            if (!context.User.HasClaim(u => u.Type == ClaimTypes.Role))
            {
                return Task.CompletedTask;
            }

            if (context.User.FindFirst(u => u.Type == ClaimTypes.Role).Value == "Dungeon Master")
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
