using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.DTO
{
    public class FacultyDTO : BaseDTO
    {
        public string FacultyName { get; set; }

        public string DeanId { get; set; }

        public List<string> ListIdSubjects { get; set; }

        public List<string> ListIdGroups { get; set; }

        public List<string> ListIdTeachers { get; set; }

        public FacultyDTO()
        {
            ListIdSubjects = new List<string>();
            ListIdGroups = new List<string>();
            ListIdTeachers = new List<string>();
        }
    }
}