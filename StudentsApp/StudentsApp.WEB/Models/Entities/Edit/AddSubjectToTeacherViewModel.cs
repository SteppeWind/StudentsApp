using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities.Edit
{
    public class AddSubjectToTeacherViewModel
    {
        public string FacultyId { get; set; }

        public string TeacherId { get; set; }

        [Display(Name ="Дисциплины, которые будет вести преподаватель")]
        public List<SubjectViewModel> Subjects { get; set; }

        public List<string> SelectedIdSubjects { get; set; }

        public AddSubjectToTeacherViewModel()
        {
            Subjects = new List<SubjectViewModel>();
            SelectedIdSubjects = new List<string>();
        }
    }
}