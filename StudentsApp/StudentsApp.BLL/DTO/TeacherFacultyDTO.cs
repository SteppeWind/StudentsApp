using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.DTO
{
    public class TeacherFacultyDTO : BaseDTO
    {
        public int FacultyId { get; set; }
        
        public string FacultyName { get; set; }

        public int PostTeacherId { get; set; }

        public string NamePostTeacher { get; set; }
    }
}