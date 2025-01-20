using Microsoft.AspNetCore.Identity;

namespace Fitally.Models
{
    public class SeedUsers
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            UserManager<IdentityUser> _userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            var userlist = new List<SeedUserModel>()
                {
                    new SeedUserModel(){ UserName="admin@gmail.com", Password= "Admin1." },
                };


            foreach (var user in userlist)
            {
                if (!_userManager.Users.Any(r => r.UserName == user.UserName))
                {
                    var newuser = new IdentityUser { UserName = user.UserName, Email = user.UserName };

                    var result = await _userManager.CreateAsync(newuser, user.Password);
                    var roleResult = await _userManager.AddToRoleAsync(newuser, "Admin");
                }
            }

            var usAdm = _userManager.Users.FirstOrDefault(f => f.UserName == "admin@gmail.com");

            var isAdm = await _userManager.IsInRoleAsync(usAdm, "Admin");

            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            bool b = await roleManager.RoleExistsAsync("Admin");

            if (!isAdm)
            {

                var addAdm = await _userManager.AddToRoleAsync(usAdm, "Admin");
            }

        }
    }
}
