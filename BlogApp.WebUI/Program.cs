using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Data.Concrete.EfCore;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlogApp.WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            try
            {
                var scope = host.Services.CreateScope();

                var context = scope.ServiceProvider.GetRequiredService<BlogContext>();
                var userMngr = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleMngr = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                context.Database.EnsureCreated();

                var adminRole = new IdentityRole("Admin");
                if (!context.Roles.Any())
                {
                    //islem asenkron ancak main method asenkron olmadigi icin GetAwaiter, GetResult kullandık
                    roleMngr.CreateAsync(adminRole).GetAwaiter().GetResult();
                    //creating seed data for roles
                }

                if (!context.Users.Any(p => p.UserName == "admin"))
                {
                    //creating seed data for admin
                    var adminUser = new IdentityUser
                    {
                        UserName = "admin",
                        Email = "admin@gmail.com"
                    };

                    var result = userMngr.CreateAsync(adminUser, "password").GetAwaiter().GetResult();

                    //Admin'e rol tanımlama
                    userMngr.AddToRoleAsync(adminUser, adminRole.Name).GetAwaiter().GetResult();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseDefaultServiceProvider(options =>
                options.ValidateScopes = false);
    }
}
