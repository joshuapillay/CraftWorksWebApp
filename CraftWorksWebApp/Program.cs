using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CraftWorksWebApp.Models;
using CraftWorksWebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CraftWorksWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureServices((context, services) =>
                    {
                        services.AddDbContext<CraftWorksWebAppContext>(options =>
                            options.UseSqlServer(
                                context.Configuration.GetConnectionString("DefaultConnection")));

                        services.AddDefaultIdentity<IdentityUser>().AddDefaultTokenProviders()
                            .AddRoles<IdentityRole>()
                            .AddEntityFrameworkStores<CraftWorksWebAppContext>();

                        services.AddControllersWithViews();

                        // Register DataAccess with the appropriate configuration
                        services.AddTransient<DataAccess>(provider =>
                        {
                            var configuration = provider.GetRequiredService<IConfiguration>();
                            return new DataAccess(configuration);
                        });
                    });
                    webBuilder.Configure((context, app) =>
                    {
                        if (context.HostingEnvironment.IsDevelopment())
                        {
                            app.UseDeveloperExceptionPage();
                            app.UseMigrationsEndPoint();
                        }
                        else
                        {
                            app.UseExceptionHandler("/Home/Error");
                            app.UseHsts();
                        }
                        app.UseHttpsRedirection();
                        app.UseStaticFiles();

                        app.UseRouting();

                        app.UseAuthentication();
                        app.UseAuthorization();

                        app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllerRoute(
                                name: "default",
                                pattern: "{controller=CraftWorks}/{action=Index}/{id?}");
                            endpoints.MapRazorPages();
                        });
                    });
                });
    }
}







