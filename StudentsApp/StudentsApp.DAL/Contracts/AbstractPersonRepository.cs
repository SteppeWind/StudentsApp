using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Contracts
{
    public abstract class AbstractPersonRepository<TPerson> : AbstractRepository<TPerson>, IPersonRepository<TPerson> where TPerson : Person
    {
        protected abstract AbstractProfileRepository<Profile> ProfileRepository { get; }

        public TPerson GetByEmail(string email)
        {
            var res = ProfileRepository.GetByEmail(email);

            if (res != null)
            {
                return GetById(res.Id);
            }

            return null;
        }

        public IEnumerable<TPerson> GetByMiddleName(string middleName)
        {
            var res = ProfileRepository.GetByMiddleName(middleName);
            return Find(p => res
                    .Select(prof => prof.MiddleName.ToUpper())
                    .Contains(p.Profile.MiddleName.ToUpper()));
        }

        public IEnumerable<TPerson> GetByName(string name)
        {
            var res = ProfileRepository.GetByName(name);
            return Find(p => res
                    .Select(prof => prof.Name.ToUpper())
                    .Contains(p.Profile.Name.ToUpper()));
        }

        public IEnumerable<TPerson> GetBySurname(string surname)
        {
            var res = ProfileRepository.GetBySurname(surname);
            return Find(p => res
                    .Select(prof => prof.Surname.ToUpper())
                    .Contains(p.Profile.Surname.ToUpper()));
        }
    }
}