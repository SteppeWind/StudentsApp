using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities
{
    public class SubjectViewModel : BaseViewModel
    {
        [DisplayName("Название дисциплины")]
        [Required(ErrorMessage = "Укажите наименование дисциплины")]
        public string SubjectName { get; set; }
        
        public string FacultyId { get; set; }

        public string FacultyName { get; set; }
    }
}