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


        public virtual ICollection<Teacher> ListTeachers { get; set; }


        public int FacultyId { get; set; }

        public virtual Faculty Faculty { get; set; }


        public virtual ICollection<VisitSubject> ListVisitSubjects { get; set; }

        public Subject()
        {
            ListMarks = new List<Mark>();
            ListTeachers = new List<Teacher>();
            ListVisitSubjects = new List<VisitSubject>();
        }
    }
}