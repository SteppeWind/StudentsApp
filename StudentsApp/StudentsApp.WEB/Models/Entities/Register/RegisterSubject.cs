using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentsApp.WEB.Models.Entities.Register
{
    public class RegisterSubject : SubjectViewModel
    {
        [Display(Name ="Список преподавателей, которые будут вести данную дисциплину")]
        public List<TeacherViewModel> Teachers { get; set; }

        public List<string> SelectedIdTeachers { get; set; }

        public RegisterSubject()
        {
            SelectedIdTeachers = new List<string>();
            Teachers = new List<TeacherViewModel>();
        }
    }
}