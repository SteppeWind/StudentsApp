using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities
{
    public class FacultyViewModel : BaseViewModel
    {
        [Required(ErrorMessage ="Укажите название факультета")]
        [Display(Name ="Наименование факультета")]
        public string FacultyName { get; set; }              
    }
}