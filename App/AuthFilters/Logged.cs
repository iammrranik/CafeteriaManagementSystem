using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace App.AuthFilters
{
    // For both admin and customer access
    public class Logged : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var data = context.HttpContext.Session.GetString("Uname");
            if (data == null)
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
        }
    }
}
