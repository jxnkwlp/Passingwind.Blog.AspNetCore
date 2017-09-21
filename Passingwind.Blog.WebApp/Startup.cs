using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Passingwind.Blog.WebApp.Data;
using Passingwind.Blog.WebApp.Models;
using Passingwind.Blog.WebApp.Services;
using Passingwind.Blog.Data;
using Microsoft.AspNetCore.Routing;
using Passingwind.Blog.BlogML;

namespace Passingwind.Blog.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<DbContext, BlogDbContext>();

            services.AddDbContext<BlogDbContext>(options =>
            {
                var dbType = Configuration.GetValue<string>("DbType");
                if (string.Equals(dbType, "sqlite", StringComparison.OrdinalIgnoreCase))
                {
                    options.UseSqlite(Configuration.GetConnectionString("Sqlite"));
                }
                else if (string.Equals(dbType, "sqlserver", StringComparison.OrdinalIgnoreCase))
                {
                    options.UseSqlServer(Configuration.GetConnectionString("SqlServer"));
                }
                else
                {
                    throw new NotSupportedException("Unknow the databse type !");
                }
            });  // default Scoped ServiceLifetime

            services.AddScoped<EntityStore>();

            services.AddIdentity<User, Role>(options =>
            {
                // password rule 
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Lockout settings
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                //options.Lockout.MaxFailedAccessAttempts = 10;

                options.User.RequireUniqueEmail = true;

                // options.Cookies.ApplicationCookie.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/admin/default/UnAuthorized/");

            })
            .AddUserManager<UserManager>()
            .AddRoleManager<RoleManager>()
            .AddSignInManager<SignInManager>()
            .AddEntityFrameworkStores<BlogDbContext>()
            .AddDefaultTokenProviders();

            services.AddMemoryCache();
            services.AddSession();

            services.AddMvc()
                .AddViewLocalization();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", p => p.AddRequirements(new AdminRequirement()));
            });

            // Add application services.
            services.AddTransient<PostManager>();
            services.AddTransient<PageManager>();
            services.AddTransient<CommentManager>();
            services.AddTransient<CategoryManager>();
            services.AddTransient<TagsManager>();
            services.AddTransient<SettingManager>();
            services.AddTransient<IEmailSender, EmailSender>();
            //services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddTransient<BlogMLImporter>();
            services.AddTransient<BlogMLExporter>();
            services.AddTransient<IFileService, LocalFileService>();

            services.AddScoped<DbInitializer>();

            services.AddScoped(s => s.GetService<SettingManager>().LoadSetting<BasicSettings>());
            services.AddScoped(s => s.GetService<SettingManager>().LoadSetting<AdvancedSettings>());
            services.AddScoped(s => s.GetService<SettingManager>().LoadSetting<EmailSettings>());
            services.AddScoped(s => s.GetService<SettingManager>().LoadSetting<CommentsSettings>());
            services.AddScoped(s => s.GetService<SettingManager>().LoadSetting<FeedSettings>());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DbInitializer dbInitializer)
        {
            dbInitializer.Initialize();

            app.UseStatusCodePages();

            app.UseDeveloperExceptionPage();

            if (env.IsDevelopment())
            { 
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseImageAxdMiddleware();

            app.UseStaticFiles();

            app.UseSession();

            app.UseAuthentication();

            app.UseMvc(RegisterRoutes);
        }

        private void RegisterRoutes(IRouteBuilder routes)
        {
            routes.MapRoute(
                name: "areas",
                template: "{area:exists}/{controller=Default}/{action=Index}/{id?}"
             );
            // routes.MapAreaRoute(name: "admin", areaName: "admin", template: "admin/{controller=Default}/{action=Index}/{id?}");

            routes.MapRoute(
                name: RouteNames.Home,
                template: "",
                defaults: new { controller = "Home", action = "Index" });


            routes.MapRoute(
               name: RouteNames.CommentForm,
               template: "comment/add/",
               defaults: new { controller = "home", action = "AddComment" });


            routes.MapRoute(
              name: RouteNames.Author,
              template: "author/{username}",
              defaults: new { controller = "home", action = "index" });

            routes.MapRoute(
               name: RouteNames.Tags,
               template: "tag/{name}",
               defaults: new { controller = "home", action = "index" });

            routes.MapRoute(
               name: RouteNames.Category,
               template: "category/{name}",
               defaults: new { controller = "home", action = "index" });

            routes.MapRoute(
               name: RouteNames.Monthlist,
               template: "{year:int}/{month:range(1,12)}",
               defaults: new { controller = "home", action = "index" });

            routes.MapRoute(
               name: RouteNames.Search,
               template: "search",
               defaults: new { controller = "home", action = "index" });


            routes.MapRoute(
                name: RouteNames.Post,
                template: "post/{slug}",
                defaults: new { controller = "home", action = "post" });

            routes.MapRoute(
                name: RouteNames.Page,
                template: "page/{slug}",
                defaults: new { controller = "home", action = "page" });

            routes.MapRoute(
              name: RouteNames.Archive,
              template: "archive",
              defaults: new { controller = "home", action = "archive" });

            routes.MapRoute(
                name: RouteNames.LogIn,
                template: "account/login",
                defaults: new { controller = "account", action = "Login" });
            routes.MapRoute(
                name: RouteNames.LogOff,
                template: "account/Logout",
                defaults: new { controller = "account", action = "Logout" });
            routes.MapRoute(
                name: RouteNames.Register,
                template: "account/register",
                defaults: new { controller = "account", action = "register" });
            routes.MapRoute(
                name: RouteNames.ChangePassword,
                template: "account/changePassword",
                defaults: new { controller = "account", action = "changePassword" });



            routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
