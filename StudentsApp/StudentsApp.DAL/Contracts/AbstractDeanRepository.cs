using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Contracts
{
    public abstract class AbstractDeanRepository<TPerson> : AbstractPersonRepository<TPerson> where TPerson : Person
    {
        /// <summary>
        /// This method means that dean finishes manage, but not delete from DB
        /// </summary>
        /// <param name="dean"></param>
        /// <param name="idFaculty"></param>
        public abstract void RemoveFaculty(TPerson dean, int idFaculty);

        public abstract void AddFaculty(TPerson dean, int idFaculty);
    }
}