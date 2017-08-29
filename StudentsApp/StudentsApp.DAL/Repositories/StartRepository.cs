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

        public StartRepository()
        {
            studentsContext = StudentsAppContext.StudentsContext;
        }

        public bool IsExistDB => studentsContext.Database.Exists();

        public void ClearDB()
        {
            studentsContext.Database.Delete();
        }

        public void FillDataDB()
        {
            studentsContext.FillData();
        }
    }
}