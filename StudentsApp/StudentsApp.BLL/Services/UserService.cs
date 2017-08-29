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
using AutoMapper;

namespace StudentsApp.BLL.Services
{
    public class UserService : PersonService<PersonDTO, Person>, IUserService
    {
        public UserService(IUnitOfWork uow) : base(uow) { }

        public async Task<ClaimsIdentity> Authenticate(PersonDTO person)
        {
            ClaimsIdentity claim = null;

            DAL.Entities.Profile user = await DataBase.ProfileManager.FindAsync(person.Email, person.Password);

            if (user != null)
            {
                claim = await DataBase.ProfileManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            }

            return claim;
        }

        public async Task SetRoles(List<string> roles)
        {
            await SetRoles(roles);
        }

        public async Task SetRoles(params string[] roles)
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
        }

        public async Task<OperationDetails> UpdateProfile(PersonDTO person)
        {
            var oldProfile = await DataBase.ProfileManager.FindByEmailAsync(person.Email);
            var profile = await DataBase.ProfileManager.FindByIdAsync(person.Id);
            OperationDetails answer = null;

            if (oldProfile == null || profile.Email == person.Email)
            {
                UpdatePerson(person, profile);
                await DataBase.SaveAsync();
                answer = new OperationDetails(true, "Профайл успешно обновлен");
            }
            else
            {
                answer = new OperationDetails(false, $"Профайл с email '{oldProfile.Email}' уже существует");
            }

            return answer;
        }

        public async Task<PersonDTO> Get(string id)
        {            
            Mapper.Initialize(cfg => cfg.CreateMap<DAL.Entities.Profile, PersonDTO>());
            var profile = Mapper.Map<DAL.Entities.Profile, PersonDTO>(await DataBase.ProfileManager.FindByIdAsync(id));
            return profile;
        }
    }
}