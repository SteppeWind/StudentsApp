using StudentsApp.BLL.Contracts;
using StudentsApp.BLL.DTO;
using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsApp.BLL.Infrastructure;
using StudentsApp.DAL.Contracts;
using StudentsApp.BLL.Message;

namespace StudentsApp.BLL.Services
{
    public class TeacherSubjectService : BaseService<TeacherSubjectDTO, TeacherSubject>, ITeacherSubjectService
    {
        public TeacherSubjectService(IUnitOfWork uow) : base(uow) { }

        protected override TeacherSubjectDTO Convert(TeacherSubject entity)
        {
            var result = base.Convert(entity);

            result.SubjectName = entity.Subject.SubjectName;
            result.FullTeacherName = entity.Teacher.Profile.ToString();
            result.FacultyId = entity.Subject.FacultyId;
            result.FacultyName = entity.Subject.Faculty.FacultyName;

            return result;
        }

        private TeacherSubject GetTeacherSubjectIfExistById(string id)
        {
            var result = DataBase.TeacherSubjectRepository[id];

            if (result == null)
            {
                throw new ValidationException("Дисцплина преподавателя не найдена");
            }

            return result;
        }

        private TeacherSubject GetTeacherSubjectIfExistBySubjectAndTeacherId(string subjectId, string teacherId)
        {
            var result = DataBase.TeacherSubjectRepository.FindFirst(ts => ts.SubjectId == subjectId && ts.TeacherId == teacherId);

            if (result == null)
            {
                throw new ValidationException("Дисцплина преподавателя не найдена");
            }

            return result;
        }

        private Teacher GetTeacherIfExistById(string id)
        {
            var result = DataBase.TeacherRepository[id];

            if (result == null)
            {
                throw new ValidationException(new TeacherMessage().NotFound());
            }

            return result;
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

        private Subject GetSubjectIfExistByName(string subjectName)
        {
            var result = DataBase.SubjectRepository.FindFirst(s => s.SubjectName.Equals(subjectName));

            if (result == null)
            {
                throw new ValidationException(new SubjectMessage().NotFound(subjectName));
            }

            return result;
        }

        private bool IsTeacherHaveSubject(TeacherSubjectDTO entity)
        {

            if (DataBase.TeacherSubjectRepository.Count == 0)
            {
                return false;
            }
            else
            {
                var result = DataBase.TeacherSubjectRepository.FindFirst(ts => ts.TeacherId == entity.TeacherId && ts.SubjectId == entity.SubjectId);
                return result != null;
            }
        }

        public IEnumerable<TeacherSubjectDTO> GetAll => Convert(DataBase.TeacherSubjectRepository.GetAll);

        public int Count => DataBase.TeacherSubjectRepository.Count;

        public async Task<OperationDetails> AddAsync(TeacherSubjectDTO entity)
        {
            OperationDetails answer = null;

            try
            {
                var subject = GetSubjectIfExistById(entity.SubjectId);
                var teacher = GetTeacherIfExistById(entity.TeacherId);

                if (IsTeacherHaveSubject(entity))
                {
                    answer = new OperationDetails(false, $"Дисциплина \"{subject.SubjectName}\" уже преподаётся данным преподавателем \"{teacher.Profile}\"");
                }
                else
                {
                    var model = ReverseConvert(entity);
                    model.Id = BaseEntity.GenerateId;
                    DataBase.TeacherSubjectRepository.Add(model);
                    await DataBase.SaveAsync();
                    answer = new OperationDetails(true, $"Дисциплина \"{subject.SubjectName}\" успешно добавлена преподавателю \"{teacher.Profile}\"");
                }
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        private OperationDetails FullRemove(TeacherSubject model)
        {
            OperationDetails answer = null;

            try
            {
                answer = new OperationDetails(true, $"Дисцплина \"{model.Subject.SubjectName}\" для преподавателя \"{model.Teacher.Profile}\" полностью удалена из базы");
                DataBase.TeacherSubjectRepository.FullRemove(model);
                DataBase.Save();
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
                var model = GetTeacherSubjectIfExistById(id);
                answer = FullRemove(model);
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        public TeacherSubjectDTO Get(string id)
        {
            return Convert(DataBase.TeacherSubjectRepository[id]);
        }

        public OperationDetails Remove(string id)
        {
            OperationDetails answer = null;

            try
            {
                var model = DataBase.TeacherSubjectRepository[id];
                DataBase.TeacherSubjectRepository.Remove(model);
                DataBase.Save();
                answer = new OperationDetails(true, $"Дисцплина \"{model.Subject.SubjectName}\" для преподавателя \"{model.Teacher.Profile}\" помечена как 'Удалён'");
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        public Task<OperationDetails> UpdateAsync(TeacherSubjectDTO entity)
        {
            throw new NotImplementedException();
        }

        public async Task<OperationDetails> AddByTeacherEmail(string teacherEmail, TeacherSubjectDTO entity)
        {
            OperationDetails answer = null;

            try
            {
                var teacher = DataBase.TeacherRepository.GetByEmail(teacherEmail);
                entity.TeacherId = teacher.Id;
                answer = await AddAsync(entity);
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        public async Task<IEnumerable<OperationDetails>> AddByTeacherEmail(string teacherEmail, IEnumerable<TeacherSubjectDTO> entities)
        {
            List<OperationDetails> answers = new List<OperationDetails>();

            try
            {
                var teacher = DataBase.TeacherRepository.GetByEmail(teacherEmail);
                var listEntyties = entities.ToList();
                int length = listEntyties.Count;

                for (int i = 0; i < length; i++)
                {
                    listEntyties[i].TeacherId = teacher.Id;
                }                
                answers = (await Add(listEntyties)).ToList();
            }
            catch (Exception ex)
            {
                answers.Add(new OperationDetails(false, ex.Message));
            }

            return answers;
        }


        public async Task<IEnumerable<OperationDetails>> Add(IEnumerable<TeacherSubjectDTO> entities)
        {
            List<OperationDetails> answers = new List<OperationDetails>();

            if (entities == null || !entities.Any())
            {
                answers.Add(new OperationDetails(false, "Вы не выбрали ни одного предмета"));
            }
            else
            {
                foreach (var item in entities)
                {
                    answers.Add(await AddAsync(item));
                }
            }

            return answers;
        }

        public IEnumerable<TeacherSubjectDTO> GetTeacherSubjects(string teacherId)
        {
            return Convert(DataBase.TeacherSubjectRepository.Find(ts => ts.TeacherId == teacherId));
        }

        public TeacherSubjectDTO Get(string subjectId, string teacherId)
        {
            return Convert(GetTeacherSubjectIfExistBySubjectAndTeacherId(subjectId, teacherId));
        }

        public OperationDetails FullRemove(string subjectId, string teacherId)
        {
            OperationDetails answer = null;

            try
            {
                var model = GetTeacherSubjectIfExistBySubjectAndTeacherId(subjectId, teacherId);
                answer = FullRemove(model);
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        public OperationDetails AddBySubjectName(string subjectName, TeacherSubjectDTO entity)
        {
            OperationDetails answer = null;

            try
            {
                var subject = GetSubjectIfExistByName(subjectName);
                var teacher = GetTeacherIfExistById(entity.TeacherId);

                DataBase.TeacherSubjectRepository.Add(new TeacherSubject()
                {
                    SubjectId = subject.Id,
                    TeacherId = entity.TeacherId
                });
                DataBase.Save();
                answer = new OperationDetails(true, $"Дисциплина \"{subject.SubjectName}\" успешно добавлена преподавателю \"{teacher.Profile}\"");
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }
    }
}