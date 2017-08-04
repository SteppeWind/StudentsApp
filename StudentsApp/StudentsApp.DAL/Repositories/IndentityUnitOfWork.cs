using StudentsApp.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsApp.DAL.Identity;
using StudentsApp.DAL.EF;
using Microsoft.AspNet.Identity.EntityFramework;
using StudentsApp.DAL.Entities;

namespace StudentsApp.DAL.Repositories
{
    public class IndentityUnitOfWork : IIndentityUnitOfWork
    {
        private IdentityContext identityContext;

        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;


        public IndentityUnitOfWork()
        {
            identityContext = IdentityContext.IdentityUserContext;
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(identityContext));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(identityContext));
        }

        public ApplicationUserManager UserManager => userManager;

        public ApplicationRoleManager RoleManager => roleManager;

        public async Task SaveAsync()
        {            
            await identityContext.SaveChangesAsync();
        }

        public void Save()
        {
            identityContext.SaveChanges();
        }        
    }
}