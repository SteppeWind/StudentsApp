using StudentsApp.BLL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsApp.BLL.DTO;
using StudentsApp.DAL.Contracts;
using StudentsApp.DAL.Entities;
using AutoMapper;
using StudentsApp.BLL.Infrastructure;

namespace StudentsApp.BLL.Services
{
    /// <summary>
    /// Service for work with student marks
    /// </summary>
    public class MarkService : BaseService<MarkDTO, Mark>, IMarkService
    {
        public MarkService(IUnitOfWork uow) : base(uow) { }


        private Mark GetMarkIfExist(int id)
        {
            var mark = DataBase.MarkRepository[id];

            if (mark == null)
            {
                throw new ValidationException("Запись не найдена");
            }

            return mark;
        }

        protected override MarkDTO Convert(Mark entity)
        {
            var result = new MarkDTO();


            if (entity.Type == SubjectType.Exam)//if type entity Exam, then convert to ExamMarkDTO
            {
                result = Map<ExamMarkDTO, ExamMark>(entity as ExamMark);
            }
            else//convert to TestMarkDTO
            {
                result = Map<TestMarkDTO, TestMark>(entity as TestMark);
            }

            result.SubjectName = entity.Subject.SubjectName;

            return result;
        }

        protected override Mark ReverseConvert(MarkDTO entity)
        {
            var result = new Mark();

            if (entity.Type == SubjectTypeDTO.Exam)//if type entity Exam, then convert to ExamMarkDTO
            {
                result = Map<ExamMark, ExamMarkDTO>(entity as ExamMarkDTO);
            }
            else//convert to TestMarkDTO
            {
                result = Map<TestMark, TestMarkDTO>(entity as TestMarkDTO);
            }

            return result;
        }

        /// <summary>
        /// Get all marks in DB
        /// </summary>
        public IEnumerable<MarkDTO> GetAll
        {
            get
            {
                return Convert(DataBase.MarkRepository.GetAll);
            }
        }

        /// <summary>
        /// Get all examms in BD
        /// </summary>
        public IEnumerable<ExamMarkDTO> GetExams
        {
            get
            {
                var examMarks = DataBase.MarkRepository
                    .Find(m => m.Type == SubjectType.Exam)
                    .Select(m => (ExamMarkDTO)Convert(m));

                return examMarks;
            }
        }

        /// <summary>
        /// Get all tests in DB
        /// </summary>
        public IEnumerable<TestMarkDTO> GetTests
        {
            get
            {
                var testMarks = DataBase.MarkRepository
                    .Find(m => m.Type == SubjectType.Test)
                    .Select(m => (TestMarkDTO)Convert(m));

                return testMarks;
            }
        }

        /// <summary>
        /// Add mark in DB
        /// </summary>
        /// <param name="entity">Mark</param>
        public void Add(MarkDTO entity)
        {
            Mark mark = ReverseConvert(entity);//convert to entity DB

            DataBase.MarkRepository.Add(mark);//add converted mark
            DataBase.Save();
        }

        /// <summary>
        /// Full remove from DB
        /// </summary>
        /// <param name="id">Id mark</param>
        public void FullRemove(int id)
        {
            var mark = GetMarkIfExist(id);//find mark

            DataBase.MarkRepository.FullRemove(mark);//delete from DB
            DataBase.Save();
        }

        /// <summary>
        /// Return converted mark from DB by id
        /// </summary>
        /// <param name="id">Id mark</param>
        /// <returns>Converted mark</returns>
        public MarkDTO Get(int id)
        {
            return Convert(GetMarkIfExist(id));
        }

        /// <summary>
        /// Change value IsDelete to false for finded mark
        /// </summary>
        /// <param name="id"></param>
        public void Remove(int id)
        {
            var mark = GetMarkIfExist(id);

            DataBase.MarkRepository.Remove(mark);
            DataBase.Save();
        }

        /// <summary>
        /// Update mark
        /// </summary>
        /// <param name="entity"></param>
        public void Update(MarkDTO entity)
        {
            var mark = GetMarkIfExist(entity.Id);//find mark

            mark.SemesterNumber = entity.SemesterNumber;//change semestr
            mark.DateSubjectPassing = entity.DateSubjectPassing;//change date

            if (mark is ExamMark examMark && entity is ExamMarkDTO examMarkDTO)//check if mark is exam in both cases
            {
                examMark.Mark = examMarkDTO.Mark;
                DataBase.MarkRepository.Update(examMark);
            }

            if (mark is TestMark testMark && entity is TestMarkDTO testMarkDTO)//check if mark is test in both cases
            {
                testMark.IsPassed = testMarkDTO.IsPassed;
                DataBase.MarkRepository.Update(testMark);
            }

            DataBase.Save();
        }

        public IEnumerable<MarkDTO> GetStudentMarks(int idStudent)
        {
            return GetAll.Where(m => m.StudentId == idStudent);
        }
    }
}