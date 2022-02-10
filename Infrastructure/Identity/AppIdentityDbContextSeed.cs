using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "fatouh",
                    Email = "fatouh_a@yahoo.com",
                    UserName = "fatouh_a@yahoo.com",
                    PhoneNumber = "01200226140",
                    Address = new Address
                    {
                        FirstName = "Ahmed",
                        LastName = "fatouh",
                        PhoneNumber = "01200226140",
                        Street = "1st",
                        City = "salam",
                        State = "cairo",
                        Country = "egypt",
                        Pincode = "17888"
                    },
                    Role = "Admin"
                };

                await userManager.CreateAsync(user, "Test@2021");
            }
            if (userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "admin",
                    Email = "admin@yahoo.com",
                    UserName = "admin@yahoo.com",
                    PhoneNumber = "01200226140",
                    Address = new Address
                    {
                        FirstName = "Ahmed",
                        LastName = "fatouh",
                        PhoneNumber = "01200226140",
                        Street = "1st",
                        City = "salam",
                        State = "cairo",
                        Country = "egypt",
                        Pincode = "17888"
                    },
                    Role = "Admin"
                };

                await userManager.CreateAsync(user, "Test@2021");
            }
            if (userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Finance",
                    Email = "Finance@El2.com",
                    UserName = "Finance@El2.com",
                    PhoneNumber = "01200226140",
                    Address = new Address
                    {
                        FirstName = "Finance",
                        LastName = "Finance",
                        PhoneNumber = "01200226140",
                        Street = "1st",
                        City = "salam",
                        State = "cairo",
                        Country = "egypt",
                        Pincode = "17888"
                    },
                    Role = "Finance"
                };

                await userManager.CreateAsync(user, "Test@2021");
            }
            if (userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Stock",
                    Email = "Stock@El2.com",
                    UserName = "Stock@El2.com",
                    PhoneNumber = "01200226140",
                    Address = new Address
                    {
                        FirstName = "Stock",
                        LastName = "Stock",
                        PhoneNumber = "01200226140",
                        Street = "1st",
                        City = "salam",
                        State = "cairo",
                        Country = "egypt",
                        Pincode = "17888"
                    },
                    Role = "Stock"
                };

                await userManager.CreateAsync(user, "Test@2021");
            }

        }
    }
}