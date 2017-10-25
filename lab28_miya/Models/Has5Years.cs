using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace lab28_miya.Models
{
    public class Has5Years : AuthorizationHandler<MinimumYearsInService>
    {
        private const int minYears = 5;

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumYearsInService requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                return Task.CompletedTask;
            }
            var startDate = Convert.ToDateTime(context.User.FindFirst(c => c.Type == ClaimTypes.UserData).Value);

            int tenure = DateTime.Compare(DateTime.Now, startDate);

            if (tenure < minYears)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
