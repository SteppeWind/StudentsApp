using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Entities
{
    [Table("ExamMarks")]
    public class ExamMark : Mark
    {
        public byte Mark { get; set; }

        public ExamMark()
        {
            Type = SubjectType.Exam;
        }
    }
}