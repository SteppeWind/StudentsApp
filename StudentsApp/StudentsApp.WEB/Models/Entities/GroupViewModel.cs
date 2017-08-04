using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities
{
    public class GroupViewModel : BaseViewModel
    {
        [Required(ErrorMessage ="Укажите название группы")]      
        [Display(Name ="Название группы")]
        public string GroupName { get; set; }        
        
        public int FacultyId { get; set; }

        public string FacultyName { get; set; }
    }
}