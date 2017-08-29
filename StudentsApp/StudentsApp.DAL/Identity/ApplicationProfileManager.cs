using Microsoft.AspNet.Identity;
using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Identity
{
    public class ApplicationProfileManager : UserManager<Profile>
    {
        public ApplicationProfileManager(IUserStore<Profile> store) : base(store) { }
        
        public IQueryable<Profile> GetByMiddleName(string middleName)
        {
            return Users
                .Where(u => u.MiddleName.ToUpper()
                .Contains(middleName.ToUpper()));
        }

        public IQueryable<Profile> GetByName(string name)
        {
            return Users
                .Where(u => u.Name.ToUpper()
                .Contains(name.ToUpper()));
        }

        public IQueryable<Profile> GetBySurname(string surname)
        {
            return Users
                .Where(u => u.Surname.ToUpper()
                .Contains(surname.ToUpper()));
        }
    }
}