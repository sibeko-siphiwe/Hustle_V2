using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Hustle.Data
{
    public class SeedData
    {
        public static async Task EnsurePopulated(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                string[] roleNames = { "Worker", "Employer" };

                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }
            }
        }
    }
}