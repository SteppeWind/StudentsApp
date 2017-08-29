using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities.ComplexEntities
{
    public class ComplexTeacher : TeacherViewModel
    {
        public List<TeacherSubjectViewModel> TeacherSubjects { get; set; }

        public List<TeacherFacultyVIewModel> Posts { get; set; }
        
        public List<ComplexStudentSubject> StudentsSubjects { get; set; }

        public IEnumerable<IGrouping<string, ComplexStudentSubject>> GroupedStudentsSubjects => StudentsSubjects.GroupBy(ss => ss.SubjectName);

        public ComplexTeacher()
        {
            TeacherSubjects = new List<TeacherSubjectViewModel>();
            Posts = new List<TeacherFacultyVIewModel>();
            StudentsSubjects = new List<ComplexStudentSubject>();
        }
    }
}