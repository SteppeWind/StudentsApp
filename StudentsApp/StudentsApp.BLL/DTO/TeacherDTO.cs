using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.DTO
{
    public class TeacherDTO : PersonDTO
    {
        public List<int> ListIdSubjects { get; set; }

        public List<int> ListIdStudents { get; set; }

        public List<int> ListIdFaculties { get; set; }

        public int CountStudents { get; set; }

        public TeacherDTO()
        {
            ListIdSubjects = new List<int>();
            ListIdStudents = new List<int>();
            ListIdFaculties = new List<int>();
            Role = "teacher";
        }
    }
}