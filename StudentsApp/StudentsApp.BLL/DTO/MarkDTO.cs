using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.DTO
{
    public class MarkDTO : BaseDTO
    {
        public string StudentId { get; set; }
        
        public string SubjectId { get; set; }

        public string SubjectName { get; set; }

        public string TeacherId { get; set; }

        public SubjectTypeDTO Type { get; set; }

        public DateTime DateSubjectPassing { get; set; }

        public byte SemesterNumber { get; set; }
    }
}