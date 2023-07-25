using Group7WebApp.Areas.Identity.Data;
using Group7WebApp.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Group7WebApp.Helpers
{
    public class MyIdentityDbInitializer
    {
        public static async Task SeedData(UserManager<WebAppUser> userManager, RoleManager<IdentityRole> roleManager, AuthDbContext context)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);


        }
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("User").Result)
            {


                IdentityRole role = new IdentityRole
                {
                    Name = "User",

                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                if (roleResult.Succeeded)
                {
                    var packedString = PreviledgePacker.PackPreviledgesIntoString(new Priviledges().UserPreviledges);
                    var result = roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, packedString)).Result;

                }
            }


            if (!roleManager.RoleExistsAsync("Manager").Result)
            {


                IdentityRole role = new IdentityRole
                {
                    Name = "Manager",

                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                if (roleResult.Succeeded)
                {
                    var packedString = PreviledgePacker.PackPreviledgesIntoString(new Priviledges().ManagerPreviledges);
                    var result = roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, packedString)).Result;

                }
            }

            if (!roleManager.RoleExistsAsync("Admin").Result)
            {


                IdentityRole role = new IdentityRole
                {
                    Name = "Admin",

                };
                IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                if (roleResult.Succeeded)
                {
                    var packedString = PreviledgePacker.PackPreviledgesIntoString(new Priviledges().AdminPreviledges);
                    var result = roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, packedString)).Result;

                }
            }
        }
        public static void SeedUsers(UserManager<WebAppUser> userManager)
        {
            if (userManager.FindByNameAsync("user@email.com").Result == null)
            {
                WebAppUser user = new WebAppUser
                {
                    UserName = "user@email.com",
                    Email = "user@email.com",
                    FirtName="User",
                    LastName="User",
                    Role="User",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    PhoneNumber = "08000000001",
                    PhoneNumberConfirmed = true,
                    Status = Status.Pending.GetDescription(),
                };

                IdentityResult result = userManager.CreateAsync(user, "User@123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "User").Wait();
                }

            }

            if (userManager.FindByNameAsync("admin").Result == null)
            {
                WebAppUser user = new WebAppUser
                {
                    UserName = "admin@email.com",
                    Email = "admin@email.com",
                    FirtName = "Admin",
                    LastName = "Admin",
                    Role = "Admin",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    PhoneNumber = "08000000001",
                    PhoneNumberConfirmed = true,
                    Status = Status.Pending.GetDescription(),
                };

                IdentityResult result = userManager.CreateAsync(user, "Admin@123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }

            if (userManager.FindByNameAsync("manager").Result == null)
            {
                WebAppUser user = new WebAppUser
                {
                    UserName = "manager@email.com",
                    Email = "manager@email.com",
                    FirtName ="Manager",
                    LastName = "Manager",
                    Role = "Manager",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    PhoneNumber = "08000000001",
                    PhoneNumberConfirmed = true,
                    Status = Status.Pending.GetDescription(),
                };

                IdentityResult result = userManager.CreateAsync(user, "Manager@123").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Manager").Wait();
                }
            }
        }


    }
}
