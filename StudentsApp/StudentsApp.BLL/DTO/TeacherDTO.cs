using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.DTO
{
    public class TeacherDTO : PersonDTO
    {
        public List<string> ListIdSubjects { get; set; }

        public List<string> ListIdStudents { get; set; }

        public List<string> ListIdFaculties { get; set; }

        public int CountStudents { get; set; }

        public TeacherDTO()
        {
            ListIdSubjects = new List<string>();
            ListIdStudents = new List<string>();
            ListIdFaculties = new List<string>();
            Role = "teacher";
        }
    }
}