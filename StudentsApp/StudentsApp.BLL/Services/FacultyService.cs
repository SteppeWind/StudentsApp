using StudentsApp.BLL.Contracts;
using StudentsApp.BLL.DTO;
using StudentsApp.BLL.Infrastructure;
using StudentsApp.DAL.Contracts;
using StudentsApp.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Services
{
    public class FacultyService : BaseService<FacultyDTO, Faculty>, IFacultyService
    {
        public FacultyService(IUnitOfWork uow) : base(uow) { }


        private bool IsFacultyExistByName(string facultyName)
        {
            return DataBase.FacultyRepository.Find(f => f.FacultyName.Equals(facultyName)).Any();
        }


        /// <summary>
        /// Return faculty from id
        /// </summary>
        /// <param name="id">Unique number of faculty</param>
        /// <returns>If faculty is null, than throw ValidationException, else return faculty</returns>
        private Faculty GetFacultyIfExist(string id)
        {
            var faculty = DataBase.FacultyRepository[id];

            if (faculty == null)
            {
                throw new ValidationException("Факультет не найден");
            }

            return faculty;
        }

        /// <summary>
        /// Convert faculty in DB to FacultyDTO
        /// </summary>
        /// <param name="entity">Converting entity</param>
        /// <returns></returns>
        protected override FacultyDTO Convert(Faculty entity)
        {
            var result = base.Convert(entity);

            //save id`s subjects in current faculty
            result.ListIdSubjects = entity.ListSubjects.Select(s => s.Id).ToList();
            //save id`s groups in current faculty
            result.ListIdGroups = entity.ListGroups.Select(g => g.Id).ToList();
            //save id`s teachers in current faculty
            result.ListIdTeachers = entity.ListTeacherFaculty.Select(tf => tf.TeacherId).ToList();

            //save id dean which current manage in this faculty
            result.DeanId = entity.ListDeanFaculties.FirstOrDefault(df => df.EndManage == null).DeanId;

            return result;
        }

        /// <summary>
        /// Get all convert faculties in DB
        /// </summary>
        public IEnumerable<FacultyDTO> GetAll
        {
            get
            {
                return Convert(DataBase.FacultyRepository.GetAll);
            }
        }

        public int Count => DataBase.FacultyRepository.Count;

        /// <summary>
        /// Add faculty to DB
        /// </summary>
        /// <param name="entity">added faculty DTO</param>
        /// <returns></returns>
        public async Task<OperationDetails> AddAsync(FacultyDTO entity)
        {
            OperationDetails answer = null;

            //find faculty with same name, if exist, then throw ValidationException
            if (IsFacultyExistByName(entity.FacultyName))
            {
                answer = new OperationDetails(false, $"Факультет с именем {entity.FacultyName} уже существует");
            }
            else
            {
                var faculty = ReverseConvert(entity);//convert faculty

                DataBase.FacultyRepository.Add(faculty);
                await DataBase.SaveAsync();
                answer = new OperationDetails(true, $"Факультет {entity.FacultyName} успешно добавлен в базу");
            }

            return answer;
        }

        /// <summary>
        /// Full delete faculty from DB
        /// </summary>
        /// <param name="id">Id faculty</param>
        /// <returns></returns>
        public OperationDetails FullRemove(string id)
        {
            OperationDetails answer = null;

            try
            {
                var faculty = GetFacultyIfExist(id);

                DataBase.DeanFacultyRepository.FullRemove(faculty.ListDeanFaculties);
                DataBase.TeacherFacultyRepository.FullRemove(faculty.ListTeacherFaculty);
                DataBase.GroupRepository.FullRemove(faculty.ListGroups);
                DataBase.SubjectRepository.FullRemove(faculty.ListSubjects);

                DataBase.FacultyRepository.FullRemove(faculty);
                DataBase.Save();

                answer = new OperationDetails(true, $"Факультет {faculty.FacultyName} полностью удален из базы");
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            
            return answer;
        }

        /// <summary>
        /// Get faculty by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FacultyDTO Get(string id)
        {
            var faculty = GetFacultyIfExist(id);

            return Convert(faculty);
        }

        /// <summary>
        /// Change value IsDelete to false for faculty
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OperationDetails Remove(string id)
        {
            OperationDetails answer = null;

            try
            {
                var faculty = GetFacultyIfExist(id);

                DataBase.FacultyRepository.Remove(faculty);
                DataBase.Save();
                answer = new OperationDetails(true, $"Факультет {faculty.FacultyName} помечен как 'Удалён'");
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }


        public async Task<OperationDetails> UpdateAsync(FacultyDTO entity)
        {
            OperationDetails answer = null;

            if (IsFacultyExistByName(entity.FacultyName))
            {
                answer = new OperationDetails(false, $"Факультет с именем {entity.FacultyName} уже существует");
            }
            else
            {
                var faculty = GetFacultyIfExist(entity.Id);

                faculty.FacultyName = entity.FacultyName;
                DataBase.FacultyRepository.Update(faculty);
                await DataBase.SaveAsync();

                answer = new OperationDetails(true, $"Информация о факультете успешно обновлена");
            }

            return answer;
        }

        public bool IsHaveSubjectFromFaculty(string subjectId, string facultyId)
        {
            return GetAll.FirstOrDefault(f => f.ListIdSubjects.Contains(subjectId) && facultyId == f.Id) != null;
        }
    }
}