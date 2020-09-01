using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApplication.Areas.Admin.Pages
{
    [Authorize(Roles = "SuperAdmin")]
    public class UsersModel : PageModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task OnGet()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var allUsersExceptCurrentUser = await _userManager.Users.Where(a => a.Id != currentUser.Id).ToListAsync();
            Users = allUsersExceptCurrentUser;
        }
        public async Task<IActionResult> OnPostActivateUserAsync(string data)
        {
            var currentUser = await _userManager.FindByIdAsync(data);
            currentUser.Active = true;
            currentUser.ActivatedBy =  _userManager.GetUserAsync(HttpContext.User).Result.Id;
            await _userManager.UpdateAsync(currentUser);
            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDeActivateUserAsync(string data)
        {
            var currentUser = await _userManager.FindByIdAsync(data);
            currentUser.Active = false;
            await _userManager.UpdateAsync(currentUser);
            return RedirectToPage();
        }
    }
}
