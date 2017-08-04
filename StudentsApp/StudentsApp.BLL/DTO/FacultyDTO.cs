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

        public int? DeanId { get; set; }

        public List<int> ListIdSubjects { get; set; }

        public List<int> ListIdGroups { get; set; }

        public List<int> ListIdTeachers { get; set; }

        public FacultyDTO()
        {
            ListIdSubjects = new List<int>();
            ListIdGroups = new List<int>();
            ListIdTeachers = new List<int>();
        }
    }
}