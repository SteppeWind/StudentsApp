using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentsApp.WEB.Models.Entities.Edit
{
    public class AddSubjectToStudentViewModel
    {
        public string StudentId { get; set; }

        public string TeacherId { get; set; }

        [Display(Name ="Список доступных дисциплин")]
        public SelectList ListSubjects { get; set; }

        public List<string> ListIdSubjects { get; set; }

        public AddSubjectToStudentViewModel()
        {
            ListIdSubjects = new List<string>();
        }
    }
}