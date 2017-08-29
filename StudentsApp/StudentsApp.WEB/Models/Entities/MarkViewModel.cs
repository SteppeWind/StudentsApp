using StudentsApp.BLL.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities
{
    public class MarkViewModel : BaseViewModel
    {
        public string StudentId { get; set; }
        
        public string SubjectId { get; set; }
        
        public string SubjectName { get; set; }

        public string TeacherId { get; set; }
        
        public PersonViewModel Teacher { get; set; }
        
        public SubjectTypeViewModel Type { get; set; }

        [DataType(DataType.Date, ErrorMessage ="Неверный формат даты")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name ="Дата сдачи")]        
        public DateTime DateSubjectPassing { get; set; }

        [DisplayName("Номер семестра")]
        [Range(1, 12, ErrorMessage ="Укажите число от 1ого до 12ти")]
        public byte SemesterNumber { get; set; }
    }
}