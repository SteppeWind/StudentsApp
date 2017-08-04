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

        private PostTeacher GetTeacherPostIfExist(int id)
        {
            var model = DataBase.PostTeacherRepository[id];

            if (model == null)
            {
                throw new ValidationException("Запись не найдена");
            }

            return model;
        } 

        public IEnumerable<PostTeacherDTO> GetAll => Convert(DataBase.PostTeacherRepository.GetAll);

        public void Add(PostTeacherDTO entity)
        {
            throw new NotImplementedException();
        }

        public void FullRemove(int id)
        {
            throw new NotImplementedException();
        }

        public PostTeacherDTO Get(int id)
        {
            return Convert(DataBase.PostTeacherRepository[id]);
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(PostTeacherDTO entity)
        {
            throw new NotImplementedException();
        }
    }
}