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

        private Subject GetSubjectIfExist(string id)
        {
            var subject = DataBase.SubjectRepository[id];

            if (subject == null)
            {
                throw new ValidationException("Предмет не найден");
            }

            return subject;
        }

        private bool IsSubjectExist(string subjectName)
        {
            //find subjects with same name
            return DataBase.SubjectRepository
                    .FindFirst(s => s.SubjectName.ToUpper().Equals(subjectName.ToUpper())) != null;
        }


        protected override SubjectDTO Convert(Subject entity)
        {
            var result = base.Convert(entity);

            //save faculty name
            result.FacultyName = entity.Faculty.FacultyName;
            //save id`s students
            result.ListIdStudents = entity.ListVisitSubjects.Select(s => s.StudentId);
            //save id`s teachers
            result.ListIdTeachers = entity.ListTeachers.Select(t => t.TeacherId);
            
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

        public int Count => DataBase.SubjectRepository.Count;

        /// <summary>
        /// Add subject to DB
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        public async Task<OperationDetails> AddAsync(SubjectDTO subject)
        {
            OperationDetails answer = null;

            try
            {
                Subject newSubject = ReverseConvert(subject);

                if (IsSubjectExist(subject.SubjectName))
                {
                    answer = new OperationDetails(false, $"Предмет с именем {subject.SubjectName} уже существует");
                }
                else
                {
                    DataBase.SubjectRepository.Add(newSubject);
                    await DataBase.SaveAsync();
                    answer = new OperationDetails(true, $"Предмет {subject.SubjectName} успешно добавлен");
                }
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        /// <summary>
        /// Get subject by id
        /// </summary>
        /// <param name="id">Id subject</param>
        /// <returns></returns>
        public SubjectDTO Get(string id)
        {
            return Convert(GetSubjectIfExist(id));
        }

        /// <summary>
        /// Change value IsDelete to false for finded subject
        /// </summary>
        /// <param name="id">Id subject</param>
        /// <returns></returns>
        public OperationDetails Remove(string id)
        {
            OperationDetails answer = null;

            try
            {
                var subject = GetSubjectIfExist(id);

                DataBase.SubjectRepository.Remove(subject);
                DataBase.Save();
                answer = new OperationDetails(true, $"Предмет {subject.SubjectName} помечен как 'Удалён'");
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }


        public async Task<OperationDetails> UpdateAsync(SubjectDTO subject)
        {
            OperationDetails answer = null;

            try
            {
                if (IsSubjectExist(subject.SubjectName))
                {
                    answer = new OperationDetails(false, $"Предмет с именем {subject.SubjectName} уже существует");
                }
                else
                {
                    var newSubject = DataBase.SubjectRepository[subject.Id];

                    newSubject.SubjectName = subject.SubjectName;
                    newSubject.FacultyId = subject.FacultyId;

                    DataBase.SubjectRepository.Update(newSubject);
                    await DataBase.SaveAsync();
                }
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        /// <summary>
        /// Full delete from DB
        /// </summary>
        /// <param name="id">Id subject</param></param>
        /// <returns></returns>
        public OperationDetails FullRemove(string id)
        {
            OperationDetails answer = null;

            try
            {
                var subject = GetSubjectIfExist(id);

                DataBase.MarkRepository.FullRemove(subject.ListMarks);
                DataBase.StudentSubjectRepository.FullRemove(subject.ListVisitSubjects);
                DataBase.TeacherSubjectRepository.FullRemove(subject.ListTeachers);
                DataBase.SubjectRepository.FullRemove(subject);
                DataBase.Save();
                answer = new OperationDetails(true, $"Предмет {subject.SubjectName} полностью удалён из базы");
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        /// <summary>
        /// Get subject in faculty
        /// </summary>
        /// <param name="idFaculty">Id faculty</param>
        /// <returns></returns>
        public IEnumerable<SubjectDTO> GetSubjectsInFaculty(string idFaculty)
        {
            var result =  Convert(DataBase.SubjectRepository.Find(s => s.FacultyId == idFaculty)).ToList();
            return result;
        }

        /// <summary>
        /// Get subject in faculty from individual teacher
        /// </summary>
        /// <param name="idFaculty">Id faculty</param>
        /// <param name="idTeacher">Id teacher</param>
        /// <returns></returns>
        public IEnumerable<SubjectDTO> GetSubjectsInFacultyFromTeacher(string idFaculty, string idTeacher)
        {
            var result = GetSubjectsInFaculty(idFaculty).Where(s => s.ListIdTeachers.Contains(idTeacher)).ToList();
            return result;
        }

        public IEnumerable<SubjectDTO> GetStudentSubjects(string studentId)
        {            
            var result = Convert(DataBase.SubjectRepository.Find(s => s.ListVisitSubjects.Select(ss => ss.StudentId).Contains(studentId)));
            return result;
        }
    }
}