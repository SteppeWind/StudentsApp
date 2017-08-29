using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentsApp.WEB.Models.Entities.ComplexEntities
{
    public class ComplexDean : DeanViewModel
    {
        public FacultyViewModel Faculty { get; set; }

        [Display(Name="Группы")]
        public List<GroupViewModel> Groups { get; set; }

        [Display(Name="Преподаваемые дисциплины")]
        public List<SubjectViewModel> Subjects { get; set; }

        [Display(Name ="Список студентов, чей средний балл выше среднего")]
        public List<StudentViewModel> StudentsMoreAverageMark { get; set; }

        [Display(Name = "Преподаватели, предметы которых посещают меньше всех студентов")]
        public List<TeacherViewModel> TeachersWithMinCountStudents { get; set; }

        [Display(Name = "Преподаватели, предметы которых посещают все студенты")]
        public List<TeacherViewModel> TeachersWithAllStudents { get; set; }

        public ComplexDean()
        {
            Groups = Groups ?? new List<GroupViewModel>();
            Subjects = Subjects ?? new List<SubjectViewModel>();
            StudentsMoreAverageMark = StudentsMoreAverageMark ?? new List<StudentViewModel>();
            TeachersWithMinCountStudents = TeachersWithMinCountStudents ?? new List<TeacherViewModel>();
            TeachersWithAllStudents = TeachersWithAllStudents ?? new List<TeacherViewModel>();
        }
    }
}