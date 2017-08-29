using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.DTO
{
    public class SubjectDTO : BaseDTO
    {
        public string SubjectName { get; set; }
        
        public string FacultyId { get; set; }

        public string FacultyName { get; set; }

        public IEnumerable<string> ListIdStudents { get; set; }

        public IEnumerable<string> ListIdTeachers { get; set; }

        public SubjectDTO()
        {
            ListIdStudents = new List<string>();
            ListIdTeachers = new List<string>();
        }
    }
}