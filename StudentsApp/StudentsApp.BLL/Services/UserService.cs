using StudentsApp.BLL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsApp.BLL.DTO;
using StudentsApp.BLL.Infrastructure;
using System.Security.Claims;
using StudentsApp.DAL.Contracts;
using StudentsApp.DAL.Entities;
using Microsoft.AspNet.Identity;

namespace StudentsApp.BLL.Services
{
    public class UserService : IUserService
    {
        private IIndentityUnitOfWork DataBase { get; set; }

        public UserService(IIndentityUnitOfWork iuow)
        {
            DataBase = iuow;
        }

        public async Task<ClaimsIdentity> Authenticate(PersonDTO person)
        {
            ClaimsIdentity claim = null;

            ApplicationUser user = await DataBase.UserManager.FindAsync(person.Email, person.Password);

            if (user != null)
            {
                claim = await DataBase.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            }

            return claim;
        }

        public async Task<OperationDetails> Create(PersonDTO person)
        {
            ApplicationUser user = await DataBase.UserManager.FindByEmailAsync(person.Email);

            if (user == null)
            {
                user = new ApplicationUser() { UserName = person.Email, Email = person.Email };

                var result = await DataBase.UserManager.CreateAsync(user, person.Password);
                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");

                await DataBase.UserManager.AddToRolesAsync(user.Id, person.Role);
                await DataBase.SaveAsync();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
        }

        public async Task SetInitialData(PersonDTO adminDto, List<string> roles)
        {
            foreach (var roleName in roles)
            {
                var role = await DataBase.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole() { Name = roleName };
                    await DataBase.RoleManager.CreateAsync(role);
                }
            }

            await Create(adminDto);
        }

        public void Dispose()
        {

        }
    }
}