using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities.ComplexEntities
{
    public class ComplexStudent : StudentViewModel
    {
        public List<GroupViewModel> Groups { get; set; }

        public List<SubjectViewModel> Subjects { get; set; }

        public List<ExamMarkViewModel> ExamMarks => Marks
            .Where(m => m.Type == SubjectTypeViewModel.Exam)
            .Select(m => m as ExamMarkViewModel).ToList();

        public List<TestMarkViewModel> TestMarks => Marks
            .Where(m => m.Type == SubjectTypeViewModel.Test)
            .Select(m => m as TestMarkViewModel).ToList();

        public List<MarkViewModel> Marks { get; set; }

        public ComplexStudent()
        {
            Groups = new List<GroupViewModel>();
            Subjects = new List<SubjectViewModel>();
            Marks = new List<MarkViewModel>();
        }
    }
}