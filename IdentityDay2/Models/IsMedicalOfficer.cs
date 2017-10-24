using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityDay2.Models
{
    public class IsMedicalOfficer : AuthorizationHandler<MedicalOfficerRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MedicalOfficerRequirement requirement)
        {
            if (context.User.FindFirst(c => c.Type == ClaimTypes.Role).Value == "Medical")
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
