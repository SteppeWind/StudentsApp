using StudentsApp.BLL.DTO;
using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsApp.DAL.Contracts;
using StudentsApp.BLL.Contracts;
using StudentsApp.BLL.Infrastructure;

namespace StudentsApp.BLL.Services
{
    public class TeacherFacultyService : BaseService<TeacherFacultyDTO, TeacherFaculty>, ITeacherFacultyService
    {
        public TeacherFacultyService(IUnitOfWork uow) : base(uow) { }

        protected override TeacherFacultyDTO Convert(TeacherFaculty entity)
        {
            var result = base.Convert(entity);

            result.FacultyName = entity.Faculty.FacultyName;
            result.NamePostTeacher = entity.Post.NamePostTeacher;

            return result;
        }

        private Teacher GetTeacherIfExistById(string id)
        {
            var result = DataBase.TeacherRepository[id];

            if (result == null)
            {
                throw new PersonNotFoundException("Преподаватель не найден");
            }

            return result;
        }

        private Teacher GetTeacherIfExistByEmail(string email)
        {
            var result = DataBase.TeacherRepository.GetByEmail(email);

            if (result == null)
            {
                throw new PersonNotFoundException("Преподаватель не найден");
            }

            return result;
        }

        private Faculty GetFacultyIfExistById(string id)
        {
            var result = DataBase.FacultyRepository[id];

            if (result == null)
            {
                throw new ValidationException("Факультет не найден");
            }

            return result;
        }

        private PostTeacher GetTeacherPostIfExistById(string id)
        {
            var result = DataBase.PostTeacherRepository[id];

            if (result == null)
            {
                throw new ValidationException("Звание не найдено");
            }

            return result;
        }


        private bool IsTeacherHaveFaculty(TeacherFacultyDTO entity)
        {
            return DataBase.TeacherFacultyRepository
                .FindFirst(tf => tf.TeacherId == entity.TeacherId && tf.FacultyId == entity.FacultyId) != null;
        }

        public IEnumerable<TeacherFacultyDTO> GetAll => Convert(DataBase.TeacherFacultyRepository.GetAll);

        public int Count => DataBase.TeacherFacultyRepository.Count;

        private async Task<OperationDetails> Add(Teacher teacher, Faculty faculty, PostTeacher post)
        {
            OperationDetails answer = null;
            var entity = new TeacherFacultyDTO()
            {
                TeacherId = teacher.Id,
                PostTeacherId = post.Id,
                FacultyId = faculty.Id
            };

            if (IsTeacherHaveFaculty(entity))
            {
                answer = new OperationDetails(false, $"Преподаватель '{teacher.Profile}' уже преподаёт на факультете '{faculty.FacultyName}'");
            }
            else
            {
                var model = ReverseConvert(entity);
                model.Id = BaseEntity.GenerateId;
                DataBase.TeacherFacultyRepository.Add(model);
                await DataBase.SaveAsync();
                answer = new OperationDetails(true, $"Преподавателю '{teacher.Profile}' добавлено новое звание '{post.NamePostTeacher}' на факультете '{faculty.FacultyName}'");
            }

            return answer;
        }

        public async Task<OperationDetails> AddAsync(TeacherFacultyDTO entity)
        {
            OperationDetails answer = null;

            try
            {
                var faculty = GetFacultyIfExistById(entity.FacultyId);
                var teacher = GetTeacherIfExistById(entity.TeacherId);
                var post = GetTeacherPostIfExistById(entity.PostTeacherId);

                answer = await Add(teacher, faculty, post);
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
                var model = DataBase.TeacherFacultyRepository[id];
                DataBase.TeacherFacultyRepository.FullRemove(model);
                DataBase.Save();
                string message = $"Для преподавателя '{model.Teacher.Profile}' звание '{model.Post.NamePostTeacher}' на факультете '{model.Faculty.FacultyName}' полностью удалено из базы";
                answer = new OperationDetails(true, message);
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        public TeacherFacultyDTO Get(string id)
        {
            return Convert(DataBase.TeacherFacultyRepository[id]);
        }

        public OperationDetails Remove(string id)
        {
            OperationDetails answer = null;

            try
            {
                var model = DataBase.TeacherFacultyRepository[id];
                DataBase.TeacherFacultyRepository.Remove(model);
                DataBase.Save();
                string message = $"Для преподавателя '{model.Teacher.Profile}' звание '{model.Post.NamePostTeacher}' на факультете '{model.Faculty.FacultyName}' помечено как 'Удалён'";
                answer = new OperationDetails(true, message);
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }

        public Task<OperationDetails> UpdateAsync(TeacherFacultyDTO entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TeacherFacultyDTO> GetTeacherPosts(string teacherId)
        {
            return Convert(DataBase.TeacherFacultyRepository.Find(tf => tf.TeacherId == teacherId));
        }

        public async Task<OperationDetails> AddByTeacherEmail(string email, TeacherFacultyDTO entity)
        {
            OperationDetails answer = null;

            try
            {
                var faculty = GetFacultyIfExistById(entity.FacultyId);
                var teacher = GetTeacherIfExistByEmail(email);
                var post = GetTeacherPostIfExistById(entity.PostTeacherId);

                answer = await Add(teacher, faculty, post);
            }
            catch (Exception ex)
            {
                answer = new OperationDetails(false, ex.Message);
            }

            return answer;
        }
    }
}