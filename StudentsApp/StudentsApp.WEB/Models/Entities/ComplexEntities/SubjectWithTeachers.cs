using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentsApp.WEB.Models.Entities.ComplexEntities
{
    public class SubjectWithTeachers : SubjectViewModel
    {
        public List<TeacherViewModel> Teachers { get; set; }

        public SubjectWithTeachers()
        {
            Teachers = new List<TeacherViewModel>();
        }
    }
}