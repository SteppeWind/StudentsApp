using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Contracts
{
    public interface IPersonRepository<TEntity> where TEntity : IProfile
    {
        IEnumerable<TEntity> GetByName(string name);
        IEnumerable<TEntity> GetBySurname(string surname);
        IEnumerable<TEntity> GetByMiddleName(string middleName);
        TEntity GetByEmail(string email);
    }
}