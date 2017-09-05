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
using StudentsApp.BLL.Message;

namespace StudentsApp.BLL.Services
{
    /// <summary>
    /// Provides for working with deans in DB
    /// </summary>
    public class DeanService : PersonService<DeanDTO, Dean>, IDeanService
    {
        public DeanService(IUnitOfWork uow) : base(uow) { }

        /// <summary>
        /// Return dean from id
        /// </summary>
        /// <param name="id">Unique number of dean</param>
        /// <returns>If dean is null, than throw ValidationException, else return dean</returns>
        private Dean GetDeanIfExist(string id)
        {
            var dean = DataBase.DeanRepository[id];

            if (dean == null)
            {
                throw new ValidationException(new DeanMessage().NotFound());
            }

            return dean;
        }

        /// <summary>
        /// Return faculty from id
        /// </summary>
        /// <param name="id">Unique number of faculty</param>
        /// <returns>If faculty is null, than throw ValidationException, else - faculty</returns>
        private Faculty GetFacultyIfExist(string id)
        {
            var faculty = DataBase.FacultyRepository[id];

            if (faculty == null)
            {
                throw new ValidationException(new FacultyMessage().NotFound());
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

        public int Count => DataBase.DeanRepository.Count;

        /// <summary>
        /// Add dean to DB
        /// </summary>
        /// <param name="entity">Added entity</param>
        /// <returns></returns>
        public async Task<OperationDetails> AddAsync(DeanDTO entity)
        {
            OperationDetails answer = null;
            bool isExist = PersonIsExist(entity);//check if exist

            if (!isExist)//if not exist that add dean to DB
            {
                var dean = await Create<DAL.Entities.Profile>(entity);
                DataBase.DeanRepository.Add(dean);
                await DataBase.SaveAsync();//save result

                await DataBase.ProfileManager.AddToRoleAsync(dean.Id, "dean");
                await DataBase.SaveAsync();//save result

                answer = new OperationDetails(true, new DeanMessage().Create(dean.Profile.ToString()));
            }
            else
            {
                answer = new OperationDetails(false, new DeanMessage().IsExist(entity.Email));
            }

            return answer;
        }

        /// <summary>
        /// Full delete from DB
        /// </summary>
        /// <param name="id">Id dean</param>
        /// <returns></returns>
        public OperationDetails FullRemove(string id)
        {
            OperationDetails answer = null;
            try
            {
                var dean = GetDeanIfExist(id);//find dean
                DataBase.DeanRepository.FullRemove(dean);//call full remove dean
                DataBase.Save();
                answer = new OperationDetails(true, new DeanMessage().FullRemove(dean.Profile.ToString()));
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }
           
            return answer;
        }

        /// <summary>
        /// Get dean from DB
        /// </summary>
        /// <param name="id">Id dean</param>
        /// <returns>Return dean, if he is exist in DB, else throw ValidationException</returns>
        public DeanDTO Get(string id)
        {
            var dean = GetDeanIfExist(id);

            return Convert(dean);
        }

        /// <summary>
        /// Change value IsDelete to false for dean
        /// </summary>
        /// <param name="id">Id dean</param>
        /// <returns></returns>
        public OperationDetails Remove(string id)
        {
            OperationDetails answer = null;
            try
            {
                var dean = GetDeanIfExist(id);//find dean

                DataBase.DeanRepository.Remove(dean);//call remove dean
                DataBase.Save();

                answer = new OperationDetails(true, new DeanMessage().Remove(dean.Profile.ToString()));
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        /// <summary>
        /// Update dean
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<OperationDetails> UpdateAsync(DeanDTO entity)
        {
            OperationDetails answer = null;

            try
            {
                if (PersonIsExist(entity))
                {
                    answer = new OperationDetails(false, new DeanMessage().IsExist(entity.Email));
                }
                else
                {
                    var dean = GetDeanIfExist(entity.Id);
                    var profile = dean.Profile;

                    UpdatePerson(entity, profile);//update user profile
                    await DataBase.ProfileManager.UpdateAsync(profile);//call update in DB
                    await DataBase.SaveAsync();                   
                    answer = new OperationDetails(true, new DeanMessage().Update());
                }
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }
           
            return answer;
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

        public IEnumerable<DeanDTO> GetDeansInFaculty(string facultyId)
        {
            return Convert(DataBase.DeanRepository
                .Find(d => d.ListDeanFaculties
                                .Select(df => df.FacultyId)
                                .Contains(facultyId)));
        }
    }
}