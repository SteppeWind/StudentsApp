using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities.ComplexEntities
{
    public class ComplexExamMark : ComplexMark
    {
        [DisplayName("Оценка")]
        [Range(2, 5, ErrorMessage ="Выставите оценку по 5ти бальной шкале (от 2х до 5ти)")]
        public byte? Mark { get; set; }


        public ComplexExamMark()
        {
            Type = SubjectTypeViewModel.Exam;
        }
    }
}