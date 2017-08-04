using StudentsApp.BLL.DTO;
using StudentsApp.BLL.Services;
using StudentsApp.DAL.Contracts;
using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsApp.BLL.Infrastructure;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using AutoMapper;

namespace StudentsApp.BLL.Contracts
{
    public class PersonService<TPersonDTO, TPerson> : BaseService<TPersonDTO, TPerson>
        where TPersonDTO : PersonDTO, new()
        where TPerson : Person
    {
        protected IIndentityUnitOfWork IdentityDataBase { get; set; }

        public PersonService(IUnitOfWork uow, IIndentityUnitOfWork iouw) : base(uow)
        {
            IdentityDataBase = iouw;
        }

        protected Task<IEnumerable<ApplicationUser>> GetAppUsersByPassword(string password)
        {
            return Task.Run(() => IdentityDataBase.UserManager.Users
            .Where(appUser => appUser.PasswordHash.Equals(password))
            .AsEnumerable());
        }

        protected Task<ApplicationUser> GetAppUserByEmail(string email)
        {
            return Task.Run(() => IdentityDataBase.UserManager.FindByEmailAsync(email));
        }

        protected Task<IEnumerable<TPersonDTO>> ConvertAsync(IEnumerable<TPerson> entyties) 
        {
            return Task.Run(() => Convert(entyties));
        }

        protected Task<TPersonDTO> ConvertAsync(TPerson entity)
        {
            return Task.Run(() => Convert(entity));
        }
        
        protected TProfile UpdatePerson<TProfile>(TPersonDTO person, TProfile profile) where TProfile : DAL.Entities.Profile
        {
            profile.Name = person.Name;
            profile.Surname = person.Surname;
            profile.MiddleName = person.MiddleName;
            //profile.Email = person.Email;
           
            return profile;
        }

        protected override TPersonDTO Convert(TPerson entity)
        {
            TPersonDTO personDTO  = base.Convert(entity);
            var appUsers = IdentityDataBase.UserManager.Users.Select(au => new { au.Email, au.PasswordHash, au.Id });

            personDTO.Id = entity.Id;
            personDTO.IsDelete = entity.IsDelete;
            personDTO.Name = entity.Profile.Name;
            personDTO.Surname = entity.Profile.Surname;
            personDTO.Email = entity.Profile.Email;
            personDTO.MiddleName = entity.Profile.MiddleName;

            foreach (var au in appUsers)
            {
                if (personDTO.Email.Equals(au.Email))
                {
                    personDTO.Password = au.PasswordHash;
                    //personDTO.Role = IdentityDataBase.RoleManager.Roles.FirstOrDefault(r => r.Id == au.Id).Name;
                    break;
                }
            }

            return personDTO;
        }
        
        protected bool PersonIsExist(TPersonDTO person)
        {
            var defUser = DataBase.ProfileRepository.GetByEmail(person.Email);//user from first DB
            var appUser = IdentityDataBase.UserManager.FindByEmail(person.Email);//user from second DB

            return defUser != null || appUser != null ? true : false;
        }

        public void Delete(TPerson person)
        {
            var appTeacher = IdentityDataBase.UserManager.FindByEmailAsync(person.Profile.Email);
            IdentityDataBase.UserManager.Delete(appTeacher.Result);
            IdentityDataBase.Save();
        }

        public void Create(TPersonDTO person)
        {
            ApplicationUser user = new ApplicationUser() { Email = person.Email, UserName = person.Email };
            var result = IdentityDataBase.UserManager.Create(user, person.Password);
            IdentityDataBase.Save();
            IdentityDataBase.UserManager.AddToRoles(user.Id, person.Role);
            IdentityDataBase.Save();
        }

        protected void Initial()
        {
            foreach (var item in DataBase.ProfileRepository.GetAll)
            {
                var res = IdentityDataBase.UserManager.Create(new ApplicationUser() {UserName = item.Name,  Email = item.Email }, "qwe123");
            }
            IdentityDataBase.Save();
        }

        protected async Task InitialAsync()
        {
            foreach (var item in DataBase.ProfileRepository.GetAll)
            {
                var res = await IdentityDataBase.UserManager.CreateAsync(new ApplicationUser() { Email = item.Email }, "qwe123");
            }
            await IdentityDataBase.SaveAsync();
        }
    }
}