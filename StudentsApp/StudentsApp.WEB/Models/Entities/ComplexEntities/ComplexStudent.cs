using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities.ComplexEntities
{
    public class ComplexStudent : StudentViewModel
    {
        public List<GroupViewModel> Groups { get; set; }

        public List<StudentSubjectViewModel> StudentSubjects { get; set; }        

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
            StudentSubjects = new List<StudentSubjectViewModel>();
            Marks = new List<MarkViewModel>();
        }
    }
}