using StudentsApp.DAL.EF;
using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Repositories
{
    public class SubjectRepository : BaseRepository<Subject>
    {
        public SubjectRepository(StudentsAppContext context) : base(context) { }
    }
}