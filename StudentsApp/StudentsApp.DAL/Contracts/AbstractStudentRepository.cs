using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Contracts
{
    public abstract class AbstractStudentRepository<TPerson> : AbstractPersonRepository<TPerson> where TPerson : Person
    {
        public abstract void AddSubject(TPerson student, int idSubject, int idTeacher);

        public abstract void RemoveSubject(TPerson student, int idSubject);
    }
}