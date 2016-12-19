 using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using InsightWebApi.Models;
using InsightWebApi.LogicLayer;
using InsightWebApi.Utilities;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using InsightWebApi.Utlities;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
 private ApplicationUserManager _userManager;
        [HttpPost]
        [AllowAnonymous]
        [ActionName("ExternalLogin")]
        [Route("api/CategoriesApi/ExternalLogin")]
        public async Task<IHttpActionResult> ExternalLogin(ExternalLoginModel formModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();
                var userAlreadyLogin = await UserManager.FindByEmailAsync(formModel.Email);
                if (userAlreadyLogin == null)
                {
                    if (formModel.FirstName == null)
                    {
                        user.UserName = formModel.Email;
                    }
                    else
                    {
                        user.UserName = formModel.FirstName;
                    }
                    user.Email = formModel.Email;
                    //     string password = GeneratePassword(6);
                    //user.PasswordHash = password;
                   
                    user.UserName = formModel.FirstName + " " + formModel.LastName;
                    // user.RegisterWith = formModel.RegisterWith;
                    var users = new ApplicationUser() { UserName = formModel.FirstName+" "+ formModel.LastName, Email = formModel.Email /*PasswordHash = password*/ };
                   
                    IdentityResult result = await UserManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        return Ok(result);
                    }
                    else if (result.Succeeded)
                    {
                        UserManager.AddToRole(user.Id, "User");
                    }
                   
                    RootObject rootObject = new RootObject();
                    rootObject.Email = user.Email;
                    rootObject.UserName = formModel.FirstName + " " + formModel.LastName;
                    
                   var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                    string role = roleManager.FindById(user.Roles.FirstOrDefault().RoleId).Name;
                    rootObject.RoleId = role;
                    rootObject.Id = user.Id;
                    //  rootObject.RegisterWith = user.RegisterWith;
                    string response = string.Empty;
                    response = Converter.TranslateObject(rootObject);
                    return Ok(response);
                }
                else
                {
                    RootObject rootObject = new RootObject();
                    rootObject.Email = userAlreadyLogin.Email;
                    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                    string role = roleManager.FindById(userAlreadyLogin.Roles.FirstOrDefault().RoleId).Name;
                    rootObject.RoleId = role;
                    rootObject.Id = userAlreadyLogin.Id;
                    //  rootObject.RegisterWith = userAlreadyLogin.RegisterWith;
                    string response = string.Empty;
                    response = Converter.TranslateObject(rootObject);
                    return Ok(response);
                }

            }
            return Ok("Register SucessFul");
        }
        public static string GeneratePassword(int length) //length of salt    
        {
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            var randNum = new Random();
            var chars = new char[length];
            var allowedCharCount = allowedChars.Length;
            for (var i = 0; i <= length - 1; i++)
            {
                chars[i] = allowedChars[Convert.ToInt32((allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
