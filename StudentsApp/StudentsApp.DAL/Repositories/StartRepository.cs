using StudentsApp.DAL.Contracts;
using StudentsApp.DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Repositories
{
    public class StartRepository : IStartRepository
    {
        StudentsAppContext studentsContext;
        IdentityContext identityContext;

        public StartRepository()
        {
            studentsContext = StudentsAppContext.StudentsContext;
            identityContext = IdentityContext.IdentityUserContext;
        }

        public void FillDataDB()
        {
            studentsContext.FillData();
        }

        public void FillDataIdentityDB()
        {

        }
    }
}