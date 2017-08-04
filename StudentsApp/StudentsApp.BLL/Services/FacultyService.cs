using StudentsApp.BLL.Contracts;
using StudentsApp.BLL.DTO;
using StudentsApp.BLL.Infrastructure;
using StudentsApp.DAL.Contracts;
using StudentsApp.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System;

namespace StudentsApp.BLL.Services
{
    public class FacultyService : BaseService<FacultyDTO, Faculty>, IFacultyService
    {
        public FacultyService(IUnitOfWork uow) : base(uow) { }

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

        /// <summary>
        /// Add faculty to DB
        /// </summary>
        /// <param name="entity">added faculty DTO</param>
        public void Add(FacultyDTO entity)
        {
            //find faculty with same name, if exist, then throw ValidationException
            if (DataBase.FacultyRepository.Find(f => f.FacultyName.Equals(entity.FacultyName)).Any())
            {
                throw new ValidationException($"Факультет с именем {entity.FacultyName} уже существует");
            }

            var faculty = ReverseConvert(entity);//convert faculty

            DataBase.FacultyRepository.Add(faculty);
            DataBase.Save();
        }

        /// <summary>
        /// Full delete faculty from DB
        /// </summary>
        /// <param name="id">Id faculty</param>
        public void FullRemove(int id)
        {
            var faculty = GetFacultyIfExist(id);

            DataBase.DeanFacultyRepository.FullRemove(faculty.ListDeanFaculties);
            DataBase.TeacherFacultyRepository.FullRemove(faculty.ListTeacherFaculty);
            DataBase.GroupRepository.FullRemove(faculty.ListGroups);
            DataBase.SubjectRepository.FullRemove(faculty.ListSubjects);

            DataBase.FacultyRepository.FullRemove(faculty);
            DataBase.Save();
        }

        /// <summary>
        /// Get faculty by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public FacultyDTO Get(int id)
        {
            var faculty = GetFacultyIfExist(id);

            return Convert(faculty);
        }

        /// <summary>
        /// Change value IsDelete to false for faculty
        /// </summary>
        /// <param name="id"></param>
        public void Remove(int id)
        {
            var faculty = GetFacultyIfExist(id);

            DataBase.FacultyRepository.Remove(faculty);
            DataBase.Save();
        }

        public void Update(FacultyDTO entity)
        {
            var faculty = GetFacultyIfExist(entity.Id);

            faculty.FacultyName = entity.FacultyName;
            DataBase.FacultyRepository.Update(faculty);
            DataBase.Save();
        }
    }
}