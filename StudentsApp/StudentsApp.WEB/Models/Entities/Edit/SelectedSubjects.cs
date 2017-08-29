using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities.Edit
{
    public class SelectedSubjects
    {
        [Display(Name = "Список предметов")]
        public List<SubjectViewModel> Subjects { get; set; }

        public List<string> SelectedIdSubjects { get; set; }
    }
}