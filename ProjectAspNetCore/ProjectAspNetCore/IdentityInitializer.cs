using Microsoft.AspNetCore.Identity;
using ProjectAspNetCore.Entities;
using ProjectAspNetCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectAspNetCore
{
    public class IdentityInitializer
    {
        public static void OlusturAdmin(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            AppUser appUser = new AppUser {
                Name = "Ramazan Alp",
                SurName = "Ersoy",
                UserName = "Alp",
                Type = (int)UserTypeEnum.Manager
            };
            if (userManager.FindByNameAsync("Alp").Result == null)
            {
                var identityResult= userManager.CreateAsync(appUser,"1").Result;
            }
            if (roleManager.FindByNameAsync("Admin").Result == null)
            {
                IdentityRole role = new IdentityRole 
                {
                    Name = "Admin"
                };
            var identityResult = roleManager.CreateAsync(role).Result;

            var result =  userManager.AddToRoleAsync(appUser, role.Name).Result;
            }
        }
    }
}
 