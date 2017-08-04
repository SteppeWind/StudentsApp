using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities
{
    public enum SubjectTypeViewModel
    {
        [Display(Name = "Зачет")]
        Test,

        [Display(Name = "Экзмен")]
        Exam
    }
}