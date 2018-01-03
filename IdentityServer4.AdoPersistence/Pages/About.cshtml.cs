using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace IdentityServer4.AdoPersistence.Pages
{
    [Authorize]
    public class AboutModel : PageModel
    {
 
       public string Message { get; set; }

        public void OnGet()
        {
            Message = "Your application description page.";
        }

        public async Task OnGetLogoff()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }

    }
}