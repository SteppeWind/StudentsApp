using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentsApp.WEB.Models.Entities.ComplexEntities
{
    public class ComplexMark : MarkViewModel
    {
        [DisplayName("Список преподавателей")]
        public SelectList ListTeachers { get; set; }

        [DisplayName("Список предметов")]
        public SelectList ListSubjects { get; set; }

        [DisplayName("Список студентов")]
        public SelectList ListStudents { get; set; }

        [DisplayName("Выберите оценку")]
        public SelectList Options { get; set; }

        public string SelectedVariant { get; set; }
    }
}