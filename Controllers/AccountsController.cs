using Advert.Web.Models.Accounts;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Advert.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _pool;

        public AccountsController(SignInManager<CognitoUser> signInManager,UserManager<CognitoUser> userManager,CognitoUserPool pool)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _pool = pool;
        }
        
       public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignUpModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _pool.GetUser(model.Email);
                    if (user.Status != null)
                    {
                        ModelState.AddModelError("Email", "User already Exists!");
                        return View(model);
                    }

                    user.Attributes.Add(CognitoAttribute.Name.AttributeName, model.Email);
                    var createdUser = await _userManager.CreateAsync(user, model.Password);
                    if (createdUser.Succeeded)
                        return RedirectToAction("Index","Home");

                    return View("Error");
                }
            }
            catch(Exception ex)
            {
                return View("Error");
            }
            return View();
        }

        public IActionResult Confirm()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(ConfirmModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user =await  _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "User Not found");
                return View(model);
            }

            var result = await _userManager.ConfirmEmailAsync(user, model.Code);
            if(result.Succeeded)
               return RedirectToAction("Index","Home");

            foreach (var err in result.Errors)
            {
                ModelState.AddModelError("", err.Description);
            }
            return View(model);
        }

        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SigninModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);
                    if (result.Succeeded)
                        return RedirectToAction("Index", "Home");

                    ModelState.AddModelError("Email", "User name or password is wrong!");

                }

                return View(user);
            }
            catch(Exception ex)
            {
                return View("error");
            }
        }


    }
}
