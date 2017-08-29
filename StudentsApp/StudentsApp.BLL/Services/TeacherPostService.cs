using StudentsApp.BLL.Contracts;
using StudentsApp.BLL.DTO;
using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsApp.DAL.Contracts;
using StudentsApp.BLL.Infrastructure;

namespace StudentsApp.BLL.Services
{
    public class TeacherPostService : BaseService<PostTeacherDTO, PostTeacher>, ITeacherPostService
    {
        public TeacherPostService(IUnitOfWork uow) : base(uow) { }

        private PostTeacher GetTeacherPostIfExist(string id)
        {
            var model = DataBase.PostTeacherRepository[id];

            if (model == null)
            {
                throw new ValidationException("Запись не найдена");
            }

            return model;
        }  

        public IEnumerable<PostTeacherDTO> GetAll => Convert(DataBase.PostTeacherRepository.GetAll);

        public int Count => DataBase.PostTeacherRepository.Count;

        public Task<OperationDetails> AddAsync(PostTeacherDTO entity)
        {
            throw new NotImplementedException();
        }

        public OperationDetails FullRemove(string id)
        {
            throw new NotImplementedException();
        }

        public PostTeacherDTO Get(string id)
        {
            return Convert(DataBase.PostTeacherRepository[id]);
        }

        public OperationDetails Remove(string id)
        {
            throw new NotImplementedException();
        }

        public Task<OperationDetails> UpdateAsync(PostTeacherDTO entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PostTeacherDTO> GetPostsForTeacher(string teacherId)
        {
            return Convert(DataBase.PostTeacherRepository
                .Find(tp => tp.ListTeacherFaculties
                                .Select(tf => tf.TeacherId)
                                .Contains(teacherId)));
        }
    }
}