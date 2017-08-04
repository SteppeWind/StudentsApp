using StudentsApp.BLL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsApp.BLL.DTO;
using StudentsApp.DAL.Entities;
using AutoMapper;
using StudentsApp.DAL.Contracts;
using StudentsApp.BLL.Infrastructure;

namespace StudentsApp.BLL.Services
{
    /// <summary>
    /// Service for work with subjects
    /// </summary>
    public class SubjectService : BaseService<SubjectDTO, Subject>, ISubjectService
    {
        public SubjectService(IUnitOfWork uow) : base(uow) { }

        private Subject GetSubjectIfExist(int id)
        {
            var subject = DataBase.SubjectRepository[id];

            if (subject == null)
            {
                throw new ValidationException("Предмет не найден");
            }

            return subject;
        }


        protected override SubjectDTO Convert(Subject entity)
        {
            var result = base.Convert(entity);

            //save faculty name
            result.FacultyName = entity.Faculty.FacultyName;
            //save id`s students
            result.ListIdStudents = entity.ListVisitSubjects.Select(s => s.StudentId);
            //save id`s teachers
            result.ListIdTeachers = entity.ListTeachers.Select(t => t.Id);

            return result;
        }

        /// <summary>
        /// Get all subjects in DB
        /// </summary>
        public IEnumerable<SubjectDTO> GetAll
        {
            get
            {
                return Convert(DataBase.SubjectRepository.GetAll);
            }
        }

        /// <summary>
        /// Add subject to DB
        /// </summary>
        /// <param name="subject"></param>
        public void Add(SubjectDTO subject)
        {
            Subject newSubject = ReverseConvert(subject);

            var sameSubject = DataBase.SubjectRepository
                .Find(s => s.SubjectName.ToUpper().Equals(subject.SubjectName.ToUpper())).FirstOrDefault();//find subjects with same name

            if (sameSubject != null)
            {
                throw new ValidationException($"Предмет с именем {subject.SubjectName} уже существует");
            }

            DataBase.SubjectRepository.Add(newSubject);
            DataBase.Save();
        }

        /// <summary>
        /// Get subject by id
        /// </summary>
        /// <param name="id">Id subject</param>
        /// <returns></returns>
        public SubjectDTO Get(int id)
        {
            return Convert(GetSubjectIfExist(id));
        }

        /// <summary>
        /// Change value IsDelete to false for finded subject
        /// </summary>
        /// <param name="id">Id subject</param>
        public void Remove(int id)
        {
            var subject = GetSubjectIfExist(id);

            DataBase.SubjectRepository.Remove(subject);
            DataBase.Save();
        }

        public void Update(SubjectDTO subject)
        {
            var newSubject = ReverseConvert(subject);

            DataBase.SubjectRepository.Update(newSubject);
            DataBase.Save();
        }

        /// <summary>
        /// Full delete from DB
        /// </summary>
        /// <param name="id">Id subject</param>
        public void FullRemove(int id)
        {
            var subject = GetSubjectIfExist(id);

            DataBase.MarkRepository.FullRemove(subject.ListMarks);
            DataBase.SubjectRepository.FullRemove(subject);
            DataBase.Save();
        }

        /// <summary>
        /// Get subject in faculty
        /// </summary>
        /// <param name="idFaculty">Id faculty</param>
        /// <returns></returns>
        public IEnumerable<SubjectDTO> GetSubjectsInFaculty(int idFaculty)
        {
            return GetAll.Where(s => s.FacultyId == idFaculty);
        }

        /// <summary>
        /// Get subject in faculty from individual teacher
        /// </summary>
        /// <param name="idFaculty">Id faculty</param>
        /// <param name="idTeacher">Id teacher</param>
        /// <returns></returns>
        public IEnumerable<SubjectDTO> GetSubjectsInFacultyFromTeacher(int idFaculty, int idTeacher)
        {
            return (GetSubjectsInFaculty(idFaculty).Where(s => s.ListIdTeachers.Contains(idTeacher)));
        }

        public IEnumerable<SubjectDTO> GetStudentSubjects(int studentId)
        {
            return GetAll.Where(s => s.ListIdStudents.Contains(studentId));
        }
    }
}