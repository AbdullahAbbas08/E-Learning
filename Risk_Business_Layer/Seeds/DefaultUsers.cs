using Risk_Business_Layer.Constants;
using Risk_Data_Access_Layer.Constants;


namespace Risk_Business_Layer.Seeds
{
    public static class DefaultUsers  
    {
        public static async Task SeedAdminUser(UserManager<ApplicationUser> UserManager, RoleManager<IdentityRole> roleManager)
        {
            var DefaultUser = new Employee
            {
                UserName = "admin250",
                Email = "admin250@gmail.com",
                EmailConfirmed = false,
                Name = "المدير",
                Address = "dummy address",
                Mobile = "01006559199",
                NationalId = "00000000000000"
            };

            var user = await UserManager.FindByEmailAsync(DefaultUser.Email);
            if (user == null)
            {
                await UserManager.CreateAsync(DefaultUser, "Consoleapp#123");
                //await UserManager.CreateAsync(DefaultUser);
                await UserManager.AddToRolesAsync(DefaultUser, new List<string>
                {
                    Roles.Admin
                    //Roles.Agent,
                    //Roles.Client,
                    //Roles.Customer
                });
            }
            await roleManager.SeedClaimsForAdmin();
        }

        public static async Task SeedAgentUser(UserManager<ApplicationUser> UserManager)
        {
            var DefaultUser = new Employee
            {
                UserName = "ali250123",
                Email="ali2158123@gmail.com",
                EmailConfirmed = false,
                Name="ali",
                Address="dummy address",
                Mobile="01280798145",
                NationalId = "00000000000000"
            };

            var user = await UserManager.FindByEmailAsync(DefaultUser.Email);
            if (user == null)
            {
                await UserManager.CreateAsync(DefaultUser, "Consoleapp#123");
                //await UserManager.CreateAsync(DefaultUser);
                await UserManager.AddToRoleAsync(DefaultUser, Roles.Agent);
            }
        }

        private static async Task SeedClaimsForAdmin(this RoleManager<IdentityRole> roleManager)
        {
            var AdminRole = await roleManager.FindByNameAsync(Roles.Admin);
            await roleManager.AddClaimPermissions(AdminRole, "call");
        }

        private static async Task AddClaimPermissions(this RoleManager<IdentityRole> roleManager,IdentityRole role , string Module)
        {
            var AllClaims = await roleManager.GetClaimsAsync(role);
            var AllPermissions = Permissions.GeneratePermissionsList(Module);
            foreach (var item in AllPermissions)
            {
                if(!AllClaims.Any(x=>x.Type =="Permission" && x.Value == item))
                {
                    await roleManager.AddClaimAsync(role, new Claim("Permission", item));
                    //List<string> x = Enum.GetValues(typeof(ClientPermissions)).Cast<ClientPermissions>().Cast<string>().ToList();
                }
            }
        }
    }
}
