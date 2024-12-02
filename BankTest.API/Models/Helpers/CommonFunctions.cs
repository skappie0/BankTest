using BankTest.API.Services;
using Microsoft.EntityFrameworkCore;

namespace BankTest.API.Models.Helpers
{
    public static class CommonFunctions
    {
        public static bool BankUserCheck(HttpContext httpContext, BankTestDBService context)
        {
            var pEmail = httpContext.User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value ?? "";

            // Check if the user exists
            var currentUser = context.Users.First(p => p.EMail == pEmail);
            if (currentUser == null)
                throw new Exception($"User with Email {pEmail} not found.");

            if(currentUser.Id != 1)
                return false;
            else
                return true;
        }
    }
}
