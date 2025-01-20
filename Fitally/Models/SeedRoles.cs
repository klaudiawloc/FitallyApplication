using Microsoft.AspNetCore.Identity;

namespace Fitally.Models
{
    public class SeedRoles
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            string[] roles = new string[] { "Admin" };


            var newrolelist = new List<IdentityRole>();

            foreach (string role in roles)
            {

                bool exists = await roleManager.RoleExistsAsync(role);

                if (!exists)
                {
                    newrolelist.Add(new IdentityRole(role));
                }
            }

            foreach (var r in newrolelist)
            {
                await roleManager.CreateAsync(r);
            }
        }
    }
}
