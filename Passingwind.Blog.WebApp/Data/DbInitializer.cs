using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Passingwind.Blog.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Passingwind.Blog.WebApp.Data
{
    public class DbInitializer
    {
        private DbContext _dbContext;
        private UserManager _userManager;
        private RoleManager _roleManager;

        public DbInitializer(DbContext dbcontext, UserManager userManager, RoleManager roleManager)
        {
            this._dbContext = dbcontext;
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        public void Initialize()
        {
            var result = _dbContext.Database.EnsureCreated();

            if (result)
            {
                var role = new Role()
                {
                    Name = "Administrator",
                };
                _roleManager.CreateAsync(role).Wait();

                _roleManager.CreateAsync(new Role() { Name = "Editor" }).Wait();
                _roleManager.CreateAsync(new Role() { Name = "Anonymous" }).Wait();

                var user = new User()
                {
                    UserName = "admin",
                    Email = "admin@wuliping.cn",
                    DisplayName = "admin",
                };

                _userManager.CreateAsync(user, "123456").Wait();

                _userManager.AddToRolesAsync(user, new string[] { role.Name }).Wait();

            }

        }
    }
}
