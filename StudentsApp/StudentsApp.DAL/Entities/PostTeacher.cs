using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Entities
{
    public class PostTeacher : BaseEntity
    {
        public string NamePostTeacher { get; set; }

        public virtual ICollection<TeacherFaculty> ListTeacherFaculties { get; set; }

        public PostTeacher()
        {
            ListTeacherFaculties = new List<TeacherFaculty>();
        }
    }
}
