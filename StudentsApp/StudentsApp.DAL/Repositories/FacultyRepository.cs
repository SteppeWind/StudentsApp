using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsApp.DAL.EF;

namespace StudentsApp.DAL.Repositories
{
    public class FacultyRepository : BaseRepository<Faculty>
    {
        public FacultyRepository(StudentsAppContext context) : base(context) { }
    }
}