using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Contracts
{
    public abstract class AbstractGroupRepository<TEntity> : AbstractRepository<TEntity> where TEntity : BaseEntity
    {
        public abstract void AddStudentToGroup(TEntity group, int idStudent);

        public abstract void RemoveStudentFromGroup(TEntity group, int idStudent);

        public abstract void AddStudentToGroup(TEntity group, string email);

        public abstract bool IsContaintsStudentFromGroup(TEntity group, int idStudent);

        public void AddStudentToGroup(int idGroup, string email)
        {
            AddStudentToGroup(this[idGroup], email);
        }

        public void AddStudentToGroup(int idGroup, int idStudent)
        {            
            AddStudentToGroup(this[idGroup], idStudent);
        }
    }
}