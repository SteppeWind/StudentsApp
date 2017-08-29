using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Entities
{
    public class Subject : BaseEntity
    {
        public string SubjectName { get; set; }

        public virtual ICollection<Mark> ListMarks { get; set; }


        public virtual ICollection<TeacherSubject> ListTeachers { get; set; }


        public string FacultyId { get; set; }

        public virtual Faculty Faculty { get; set; }


        public virtual ICollection<StudentSubject> ListVisitSubjects { get; set; }

        public Subject()
        {
            ListMarks = new List<Mark>();
            ListTeachers = new List<TeacherSubject>();
            ListVisitSubjects = new List<StudentSubject>();
        }
    }
}