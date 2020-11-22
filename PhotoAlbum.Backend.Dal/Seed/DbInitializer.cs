using PhotoAlbum.Backend.Common.Constants;
using PhotoAlbum.Backend.Common.Options;
using PhotoAlbum.Backend.Dal.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhotoAlbum.Backend.Dal.Seed
{
    public static class DbInitializer
    {
        public static async Task Seed(PhotoAlbumDbContext context, IServiceProvider serviceProvider)
        {
            var dbSeedOptions = serviceProvider.GetRequiredService<IOptions<DbSeedOptions>>().Value;
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

            await CreateRolesAsync(roleManager);

            if (dbSeedOptions.CreateDefaultAdmin)
            {
                var defaultAdmin = new User 
                { 
                    UserName = dbSeedOptions.DefaultAdminUserName,
                    Email = dbSeedOptions.DefaultAdminEmailAddress
                };
                await CreateDefaultAdminAsync(userManager, defaultAdmin, dbSeedOptions.DefaultAdminPassword);
            }
        }

        private static async Task CreateRolesAsync(RoleManager<IdentityRole<int>> roleManager)
        {
            var roles = new List<IdentityRole<int>> {
                new IdentityRole<int> { Name = Roles.Admin},
                new IdentityRole<int> { Name = Roles.User}
            };

            foreach (var role in roles)
                if (!await roleManager.RoleExistsAsync(role.Name))
                    await roleManager.CreateAsync(role);
        }

        private static async Task CreateDefaultAdminAsync(UserManager<User> userManager, User defaultAdmin, string defaultAdminPassword)
        {
            var user = await userManager.FindByNameAsync(defaultAdmin.UserName);

            if (user is null)
            {
                var createdUser = await userManager.CreateAsync(defaultAdmin, defaultAdminPassword);

                if (createdUser.Succeeded)
                    await userManager.AddToRoleAsync(defaultAdmin, Roles.Admin);
            }
        }
    }
}
