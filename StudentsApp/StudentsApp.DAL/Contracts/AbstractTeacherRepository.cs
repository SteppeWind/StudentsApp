using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Contracts
{
    public abstract class AbstractTeacherRepository<TEntity> : AbstractPersonRepository<TEntity> where TEntity : Person
    {
        public abstract IEnumerable<TEntity> GetTeachers(int idFaculty);

        public abstract void AddSubject(TEntity teacher, int idSubject);

        public abstract void RemoveSubject(TEntity teacher, int idSubject);

        public abstract void AddTeacherPost(int idTeacher, int idFaculty, int idPost);

        public void AddTeacherPost(TEntity entity, int idFaculty, int idPost)
        {
            AddTeacherPost(entity.Id, idFaculty, idPost);
        }

        public abstract void RemoveTeacherPost(int idTeacher, int idFaculty, int idPost);

        public abstract void UpdateTeacherPost(int idTeacher, int idFaculty, int idOldPost, int idNewPost);
    }
}