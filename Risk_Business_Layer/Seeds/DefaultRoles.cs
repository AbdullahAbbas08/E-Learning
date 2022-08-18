using Risk_Data_Access_Layer.Constants;

namespace Risk_Business_Layer.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedRole(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin));
                await roleManager.CreateAsync(new IdentityRole(Roles.Agent));
                await roleManager.CreateAsync(new IdentityRole(Roles.Client));
                await roleManager.CreateAsync(new IdentityRole(Roles.Customer));
            }
        }
    }
}
