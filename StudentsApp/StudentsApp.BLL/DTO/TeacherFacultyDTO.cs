using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.DTO
{
    public class TeacherFacultyDTO : BaseDTO
    {
        public string FacultyId { get; set; }
        
        public string FacultyName { get; set; }


        public string PostTeacherId { get; set; }

        public string NamePostTeacher { get; set; }


        public string TeacherId { get; set; }
    }
}