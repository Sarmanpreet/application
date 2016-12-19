using InsightWebApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace InsightWebApi.DAL
{
    public class DbInitializer: CreateDatabaseIfNotExists<InsightWebApi.Models.ApplicationDbContext>
    {
        protected override void Seed(InsightWebApi.Models.ApplicationDbContext context)
        {
            
            var store = new RoleStore<IdentityRole>(context);
            var manager1 = new RoleManager<IdentityRole>(store);
            List<IdentityRole> IdentityRoles = new List<IdentityRole>
                {
                           new IdentityRole {Id=Guid.NewGuid().ToString() ,Name="Admin"},
                           new IdentityRole {Id=Guid.NewGuid().ToString() , Name="Edittor"},
                           new IdentityRole {Id=Guid.NewGuid().ToString() , Name="User"},
               };
            foreach (var role in IdentityRoles)
            {
                manager1.Create(role);
                context.SaveChanges();
            }


            Guid _userId = Guid.NewGuid();
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var user = new ApplicationUser { Id = _userId.ToString(), FullName="Test",Email = "admin@qservices.com", PasswordHash = "AOvIIvezZB4/NEj25k62JlMAGesYdRRL6aAOu/FhqpxrA13qtD+sqS8DMxFBgdk+Aw==", UserName = "admin@qservices.com", EmailConfirmed = true};
            manager.Create(user);
            manager.AddToRole(user.Id, "Admin");
        }
    }
}