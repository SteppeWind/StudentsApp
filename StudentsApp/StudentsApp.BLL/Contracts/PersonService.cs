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
        where TPerson : Person, new()
    {
        public PersonService(IUnitOfWork uow) : base(uow) { }
       
        protected Task<DAL.Entities.Profile> GetAppUserByEmail(string email)
        {
            return Task.Run(() => DataBase.ProfileManager.FindByEmailAsync(email));
        }

        protected Task<IEnumerable<TPersonDTO>> ConvertAsync(IEnumerable<TPerson> entyties) 
        {
            return Task.Run(() => Convert(entyties));
        }

        protected Task<TPersonDTO> ConvertAsync(TPerson entity)
        {
            return Task.Run(() => Convert(entity));
        }

        protected async Task<TPerson> Create<TProfile>(TPersonDTO person) where TProfile : DAL.Entities.Profile
        {
            TProfile profile = Map<TProfile, TPersonDTO>(person);
            profile.UserName = profile.Email;
            profile.Id = BaseEntity.GenerateId;

            await DataBase.ProfileManager.CreateAsync(profile, person.Password);

            TPerson result = new TPerson();
            result.Profile = profile;

            return result;
        }
        
        protected async Task<bool> DeleteProfile<TProfile>(TProfile profile) where TProfile : DAL.Entities.Profile
        {
            await DataBase.ProfileManager.RemoveFromRoleAsync(profile.Id, profile.Roles.First().RoleId);
            await DataBase.ProfileManager.DeleteAsync(profile);
            return true;
        }

        protected TProfile UpdatePerson<TProfile>(TPersonDTO person, TProfile profile) where TProfile : DAL.Entities.Profile
        {
            profile.Name = person.Name;
            profile.Surname = person.Surname;
            profile.MiddleName = person.MiddleName;
            profile.Email = person.Email;
           
            return profile;
        }

        protected override TPersonDTO Convert(TPerson entity)
        {
            TPersonDTO personDTO  = base.Convert(entity);
            var appUsers = DataBase.ProfileManager.Users.Select(au => new { au.Email, au.PasswordHash, au.Id });
            
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
            var appUser = DataBase.ProfileManager.FindByEmail(person.Email);//user from DB
            return appUser != null ? true : false;
        }
    }
}