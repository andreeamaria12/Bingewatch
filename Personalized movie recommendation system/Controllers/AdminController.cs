using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Personalized_movie_recommendation_system.Models;
using Microsoft.AspNetCore.Authorization;

namespace Personalized_movie_recommendation_system.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private IUserValidator<User> userValidator;
        private IPasswordValidator<User> passwordValidator;
        private IPasswordHasher<User> passwordHasher;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<User> usrMgr, IUserValidator<User> userValid, IPasswordValidator<User> passValid, IPasswordHasher<User> passwordHash, RoleManager<IdentityRole> roleMgr) 
        {
            _userManager = usrMgr;
            userValidator = userValid;
            passwordValidator = passValid;
            passwordHasher = passwordHash;
            _roleManager = roleMgr;
        }

        public async Task<IActionResult> Index()
        {
            UserViewModel model = new UserViewModel();
            string userRole = "User";
            string adminRole = "Admin";
            model.Admins = (List<User>)await _userManager.GetUsersInRoleAsync(adminRole);
            model.Users = (List<User>)await _userManager.GetUsersInRoleAsync(userRole);
            model.Roles = _roleManager.Roles.ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if(user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View("Index", _userManager.Users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if(user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string email, string password)
        {
            User user = await _userManager.FindByIdAsync(id);
            if(user !=  null)
            {
                user.Email = email;
                IdentityResult validEmail = await userValidator.ValidateAsync(_userManager, user);
                if(!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }
                IdentityResult validPass = null;
                if(!string.IsNullOrEmpty(password))
                {
                    validPass = await passwordValidator.ValidateAsync(_userManager, user, password);
                    if(validPass.Succeeded)
                    {
                        user.PasswordHash = passwordHasher.HashPassword(user, password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }

                if((validEmail.Succeeded && validPass == null) || (validEmail.Succeeded && password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(user);

        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}