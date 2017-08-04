using StudentsApp.BLL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsApp.BLL.DTO;
using AutoMapper;
using StudentsApp.DAL.Entities;
using StudentsApp.DAL.Contracts;
using StudentsApp.BLL.Infrastructure;

namespace StudentsApp.BLL.Services
{
    /// <summary>
    /// Provides for working with deans in DB
    /// </summary>
    public class DeanService : PersonService<DeanDTO, Dean>, IDeanService
    {
        public DeanService(IUnitOfWork uow, IIndentityUnitOfWork iouw) : base(uow, iouw) { }

        /// <summary>
        /// Return dean from id
        /// </summary>
        /// <param name="id">Unique number of dean</param>
        /// <returns>If dean is null, than throw ValidationException, else return dean</returns>
        private Dean GetDeanIfExist(int id)
        {
            var dean = DataBase.DeanRepository[id];

            if (dean == null)
            {
                throw new ValidationException("Декан не найден");
            }

            return dean;
        }

        /// <summary>
        /// Return faculty from id
        /// </summary>
        /// <param name="id">Unique number of faculty</param>
        /// <returns>If faculty is null, than throw ValidationException, else return faculty</returns>
        private Faculty GetFacultyIfExist(int id)
        {
            var faculty = DataBase.FacultyRepository[id];

            if (faculty == null)
            {
                throw new ValidationException("Факультет не найден");
            }

            return faculty;
        }

        /// <summary>
        /// Get convert Dean in DB to DeanDTO
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected override DeanDTO Convert(Dean entity)
        {
            var result = base.Convert(entity);

            //save history of this dean
            result.ListIdDeanFaculties = entity.ListDeanFaculties.Select(df => df.Id).ToList();
            //save id faculty
            var deanFaculty = entity.ListDeanFaculties.FirstOrDefault(d => d.EndManage == null);
            if (deanFaculty != null)
                result.FacultyId = deanFaculty.FacultyId;

            return result;
        }

        /// <summary>
        /// Get all deans in DB
        /// </summary>
        public IEnumerable<DeanDTO> GetAll
        {
            get
            {
                return Convert(DataBase.DeanRepository.GetAll);
            }
        }

        /// <summary>
        /// Add dean to DB
        /// </summary>
        /// <param name="entity">Added entity</param>
        public void Add(DeanDTO entity)
        {
            bool isExist = PersonIsExist(entity);//check if exist

            if (!isExist)//if not exist that add dean to DB
            {
                var dean = ReverseConvert(entity);//convert

                DataBase.DeanRepository.Add(dean);
                Create(entity);//add dean to IndentityDB
                DataBase.Save();//save result
            }
            else
            {
                throw new PersonIsExistException(entity);
            }
        }


        /// <summary>
        /// Add faculty to dean
        /// </summary>
        /// <param name="idDean">Id dean</param>
        /// <param name="idFaculty">Id faculty</param>
        public void AddFaculty(int idDean, int idFaculty)//*
        {
            var dean = GetDeanIfExist(idDean);//find dean
            var faculty = GetFacultyIfExist(idFaculty);//find faculty

            DataBase.DeanRepository.AddFaculty(dean, faculty.Id);
            DataBase.Save();
        }


        /// <summary>
        /// Remove faculty from dean
        /// </summary>
        /// <param name="idDean">Id dean</param>
        /// <param name="idFaculty">Id faculty</param>
        public void RemoveFaculty(int idDean, int idFaculty)
        {
            var dean = GetDeanIfExist(idDean);//find dean
            var faculty = GetFacultyIfExist(idFaculty);//find faculty

            DataBase.DeanRepository.RemoveFaculty(dean, faculty.Id);
            DataBase.Save();
        }

        /// <summary>
        /// Full delete from DB
        /// </summary>
        /// <param name="id">Id dean</param>
        public void FullRemove(int id)
        {
            var dean = GetDeanIfExist(id);//find dean

            DataBase.DeanRepository.FullRemove(dean);//call full remove dean
            Delete(dean);//delete entry from IdentityDB
            DataBase.Save();
        }

        /// <summary>
        /// Get dean from DB
        /// </summary>
        /// <param name="id">Id dean</param>
        /// <returns>Return dean, if he is exist in DB, else throw ValidationException</returns>
        public DeanDTO Get(int id)
        {
            var dean = GetDeanIfExist(id);

            return Convert(dean);
        }

        /// <summary>
        /// Change value IsDelete to false for dean
        /// </summary>
        /// <param name="id">Id dean</param>
        public void Remove(int id)
        {
            var dean = GetDeanIfExist(id);//find dean

            DataBase.DeanRepository.Remove(dean);//call remove dean
            DataBase.Save();
        }

        /// <summary>
        /// Update dean
        /// </summary>
        /// <param name="entity"></param>
        public void Update(DeanDTO entity)
        {
            var dean = GetDeanIfExist(entity.Id);
            var profile = dean.Profile;

            UpdatePerson(entity, profile);//update user profile
            DataBase.ProfileRepository.Update(profile);//call update in DB
            DataBase.Save();
        }

        public Task<DeanDTO> GetByEmailAsync(string email)
        {
            return ConvertAsync(DataBase.DeanRepository.GetByEmail(email));
        }

        public Task<IEnumerable<DeanDTO>> GetByMiddleNameAsync(string middleName)
        {
            return ConvertAsync(DataBase.DeanRepository.GetByMiddleName(middleName));
        }

        public Task<IEnumerable<DeanDTO>> GetByNameAsync(string name)
        {
            return ConvertAsync(DataBase.DeanRepository.GetByName(name));
        }

        public Task<IEnumerable<DeanDTO>> GetBySurnameAsync(string surname)
        {
            return ConvertAsync(DataBase.DeanRepository.GetBySurname(surname));
        }
    }
}