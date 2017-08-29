using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities.Edit
{
    public class AddStudentToTeacherViewModel
    {
        public string TeacherId { get; set; }

        public string FacultyId { get; set; }

        [Display(Name = "Список студентов")]
        public List<StudentViewModel> Students { get; set; }

        [Display(Name = "Список предметов")]
        public List<SubjectViewModel> Subjects { get; set; }


        public IEnumerable<IGrouping<char, StudentViewModel>> GropedStudents 
            => Students.OrderBy(s => s.FullName.First()).GroupBy(s => s.FullName.First());


        public List<string> SelectedIdStudents { get; set; }

        public List<string> SelectedIdSubjects { get; set; }


        public AddStudentToTeacherViewModel()
        {
            Students = Students ?? new List<StudentViewModel>();
            Subjects = Subjects ?? new List<SubjectViewModel>();
            SelectedIdStudents = SelectedIdStudents ?? new List<string>();
            SelectedIdSubjects = SelectedIdSubjects ?? new List<string>();
        }
    }
}