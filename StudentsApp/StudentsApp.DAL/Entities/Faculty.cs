using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Entities
{
    public class Faculty : BaseEntity
    {
        public string FacultyName { get; set; }


        public virtual ICollection<DeanFaculty> ListDeanFaculties { get; set; }


        public virtual ICollection<TeacherFaculty> ListTeacherFaculty { get; set; }


        public virtual ICollection<Group> ListGroups { get; set; }


        public virtual ICollection<Subject> ListSubjects { get; set; }

        public Faculty()
        {
            ListTeacherFaculty = new List<TeacherFaculty>();
            ListGroups = new List<Group>();
            ListSubjects = new List<Subject>();
            ListDeanFaculties = new List<DeanFaculty>();
        }
    }
}
