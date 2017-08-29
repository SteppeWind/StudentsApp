using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.DTO
{
    public class TeacherSubjectDTO : BaseDTO
    {
        public string SubjectId { get; set; }

        public string SubjectName { get; set; }


        public string FacultyId { get; set; }

        public string FacultyName { get; set; }


        public string TeacherId { get; set; }

        public string FullTeacherName { get; set; }
    }
}