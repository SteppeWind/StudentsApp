using StudentsApp.DAL.Entities;
using StudentsApp.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsApp.DAL.EF;

namespace StudentsApp.DAL.Contracts
{
    public abstract class AbstractPersonRepository<TPerson> : BaseRepository<TPerson> where TPerson : Person
    {
        public AbstractPersonRepository(StudentsAppContext context) : base(context) { }

        public abstract IEnumerable<TPerson> GetByMiddleName(string middleName);

        public abstract IEnumerable<TPerson> GetByName(string name);

        public abstract IEnumerable<TPerson> GetBySurname(string surname);

        public abstract TPerson GetByEmail(string email);  
    }
}