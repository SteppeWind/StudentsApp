using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Entities
{
    [Table("Teachers")]
    public class Teacher : Person
    {
        public virtual ICollection<TeacherFaculty> ListTeacherFaculty { get; set; }


        public virtual ICollection<StudentSubject> ListStudents { get; set; }


        public virtual ICollection<TeacherSubject> ListSubjects { get; set; }


        public virtual ICollection<Mark> ListMarks { get; set; }

        public Teacher()
        {
            ListTeacherFaculty = new List<TeacherFaculty>();
            ListSubjects = new List<TeacherSubject>();
            ListMarks = new List<Mark>();
        }
    }
}
