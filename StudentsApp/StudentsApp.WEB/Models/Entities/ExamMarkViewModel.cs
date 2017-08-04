using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsApp.WEB.Models.Entities
{
    public class ExamMarkViewModel : MarkViewModel
    {
        public byte Mark { get; set; }

        public ExamMarkViewModel()
        {
            Type = SubjectTypeViewModel.Exam;
        }
    }
}