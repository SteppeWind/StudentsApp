using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Contracts
{
    public abstract class AbstractProfileRepository<TEntity> : AbstractRepository<TEntity> where TEntity : Profile
    {
        public abstract IEnumerable<TEntity> GetByMiddleName(string middleName);

        public abstract IEnumerable<TEntity> GetByName(string name);

        public abstract IEnumerable<TEntity> GetBySurname(string surname);

        public abstract TEntity GetByEmail(string email);
    }
}