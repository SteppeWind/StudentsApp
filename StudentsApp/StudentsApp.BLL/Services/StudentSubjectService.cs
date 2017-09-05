using StudentsApp.BLL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsApp.BLL.DTO;
using StudentsApp.BLL.Infrastructure;
using StudentsApp.DAL.Entities;
using StudentsApp.DAL.Contracts;
using StudentsApp.BLL.Message;

namespace StudentsApp.BLL.Services
{
    public class StudentSubjectService : BaseService<StudentSubjectDTO, StudentSubject>, IStudentSubjectService
    {
        public StudentSubjectService(IUnitOfWork uow) : base(uow) { }

        protected override StudentSubjectDTO Convert(StudentSubject entity)
        {
            var result = base.Convert(entity);

            result.FullStudentName = entity.Student.Profile.ToString();
            result.FullTeacherName = entity.Teacher.Profile.ToString();
            result.SubjectName = entity.Subject.SubjectName;
            result.FacultyId = entity.Subject.FacultyId;
            result.FacultyName = entity.Subject.Faculty.FacultyName;

            return result;
        }

        private StudentSubject GetStudentSubjectIfExistById(string id)
        {
            var model = DataBase.StudentSubjectRepository[id];

            if (model == null)
            {
                throw new ValidationException("Изучаемый предмет не найден");
            }

            return model;
        }


        private Student GetStudentIfExistById(string id)
        {
            var model = DataBase.StudentRepository[id];

            if (model == null)
            {
                throw new ValidationException(new StudentMessage().NotFound());
            }

            return model;
        }

        private Subject GetSubjectIfExistById(string id)
        {
            var result = DataBase.SubjectRepository[id];

            if (result == null)
            {
                throw new ValidationException(new SubjectMessage().NotFound());
            }

            return result;
        }

        private bool IsStudentSubjectExist(string studentId, string subjectId)
        {
            return DataBase.StudentSubjectRepository
                .FindFirst(ss => ss.StudentId == studentId && ss.SubjectId == subjectId) != null;
        }

        public IEnumerable<StudentSubjectDTO> GetAll => Convert(DataBase.StudentSubjectRepository.GetAll);

        public int Count => DataBase.StudentSubjectRepository.Count;

        public async Task<OperationDetails> AddAsync(StudentSubjectDTO entity)
        {
            OperationDetails answer = null;

            try
            {
                var subject = GetSubjectIfExistById(entity.SubjectId);
                var student = GetStudentIfExistById(entity.StudentId);

                if (IsStudentSubjectExist(entity.StudentId, entity.SubjectId))
                {
                    answer = new OperationDetails(false, $"Предмет \"{subject.SubjectName}\" уже изучается студентом \"{student.Profile}\"");
                }
                else
                {
                    var result = ReverseConvert(entity);
                    result.Id = BaseEntity.GenerateId;
                    DataBase.StudentSubjectRepository.Add(result);
                    await DataBase.SaveAsync();
                    answer = new OperationDetails(true, $"Предмет \"{subject.SubjectName}\" успешно добавлен студенту \"{student.Profile}\"");
                }
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        public async Task<OperationDetails> AddByEmail(string studentEmail, StudentSubjectDTO model)
        {
            OperationDetails answer = null;

            try
            {
                var profile = DataBase.ProfileManager.FindByEmailAsync(studentEmail).Result;

                model.StudentId = profile.Id;
                model.FullStudentName = profile.ToString();
                answer = await AddAsync(model);
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        public OperationDetails FullRemove(string id)
        {
            OperationDetails answer = null;

            try
            {
                var model = GetStudentSubjectIfExistById(id);

                answer = new OperationDetails(true, $"Предмет \"{model.Subject.SubjectName}\" у студента \"{model.Student.Profile}\" полностью удалён из базы");
                DataBase.StudentSubjectRepository.FullRemove(model);
                DataBase.Save();
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        public StudentSubjectDTO Get(string id)
        {
            return Convert(DataBase.StudentSubjectRepository[id]);
        }

        public OperationDetails Remove(string id)
        {
            OperationDetails answer = null;

            try
            {
                var model = GetStudentSubjectIfExistById(id);

                DataBase.StudentSubjectRepository.Remove(model);
                DataBase.Save();
                answer = new OperationDetails(true, $"Предмет \"{model.Subject.SubjectName}\" у студента \"{model.Student.Profile}\" помечен в базе как 'Удалён'");
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        public async Task<OperationDetails> UpdateAsync(StudentSubjectDTO entity)
        {
            OperationDetails answer = null;

            try
            {
                var model = GetStudentSubjectIfExistById(entity.Id);

                model.StudentId = entity.StudentId;
                model.SubjectId = entity.SubjectId;
                model.TeacherId = entity.TeacherId;

                DataBase.StudentSubjectRepository.Update(model);
                await DataBase.SaveAsync();
                answer = new OperationDetails(true, $"Предмет \"{model.Subject.SubjectName}\" успешно обновлен");
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        public async Task<IEnumerable<OperationDetails>> Add(IEnumerable<StudentSubjectDTO> models)
        {
            List<OperationDetails> answers = new List<OperationDetails>();

            if (models == null || !models.Any())
            {
                answers.Add(new OperationDetails(false, "Вы не выбрали ни одного предмета"));
            }
            else
            {
                foreach (var item in models)
                {
                    answers.Add(await AddAsync(item));
                }
            }

            return answers;
        }
        
        public async Task<IEnumerable<OperationDetails>> AddByEmail(string studentEmail, IEnumerable<StudentSubjectDTO> models)
        {
            List<OperationDetails> answers = new List<OperationDetails>();

            if (models == null || !models.Any())
            {
                answers.Add(new OperationDetails(false, "Вы не выбрали ни одного предмета"));
            }
            else
            {
                foreach (var item in models)
                {
                    answers.Add(await AddByEmail(studentEmail, item));
                }
            }

            return answers;
        }

        public IEnumerable<StudentSubjectDTO> GetStudentSubjects(string subjectId, string teacherId)
        {
            return Convert(DataBase.StudentSubjectRepository.Find(ss => ss.SubjectId == subjectId && ss.TeacherId == teacherId));
        }

        public IEnumerable<StudentSubjectDTO> GetStudentSubjectsByStudentId(string studentId)
        {
            return Convert(DataBase.StudentSubjectRepository.Find(ss => ss.StudentId == studentId));
        }

        public IEnumerable<StudentSubjectDTO> GetStudentSubjectsByTeacherId(string teacherId)
        {
            return Convert(DataBase.StudentSubjectRepository.Find(ss => ss.TeacherId == teacherId));
        }
    }
}