using StudentsApp.DAL.Contracts;
using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsApp.DAL.EF;

namespace StudentsApp.DAL.Repositories
{
    public class PersonRepository<TPerson> : AbstractPersonRepository<TPerson> where TPerson : Person
    {
        public PersonRepository(StudentsAppContext context) : base(context) { }

        public override TPerson GetByEmail(string email)
        {
            return Find(p => p.Profile.Email
            .ToUpper()
            .Equals(email.ToUpper()))
            .FirstOrDefault();
        }

        public override IEnumerable<TPerson> GetByMiddleName(string middleName)
        {
            return Find(p => p.Profile.MiddleName
            .ToUpper()
            .Contains(middleName.ToUpper()));
        }

        public override IEnumerable<TPerson> GetByName(string name)
        {
            return Find(p => p.Profile.Name
           .ToUpper()
           .Contains(name.ToUpper()));
        }

        public override IEnumerable<TPerson> GetBySurname(string surname)
        {
            return Find(p => p.Profile.Surname
           .ToUpper()
           .Contains(surname.ToUpper()));
        }
    }
}