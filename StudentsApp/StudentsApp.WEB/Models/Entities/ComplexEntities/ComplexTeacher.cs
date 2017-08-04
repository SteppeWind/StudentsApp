using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities.ComplexEntities
{
    public class ComplexTeacher : TeacherViewModel
    {
        public List<SubjectViewModel> Subjects { get; set; }

        public List<TeacherFacultyVIewModel> Posts { get; set; }
        
        public List<SubjectWithStudents> StudentsSubjects { get; set; }

        public ComplexTeacher()
        {
            Subjects = new List<SubjectViewModel>();
            Posts = new List<TeacherFacultyVIewModel>();
            StudentsSubjects = new List<SubjectWithStudents>();
        }
    }
}