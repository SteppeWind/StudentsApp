using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.DTO
{
    public class MarkDTO : BaseDTO
    {
        public int StudentId { get; set; }
        
        public int SubjectId { get; set; }

        public string SubjectName { get; set; }

        public int TeacherId { get; set; }

        public SubjectTypeDTO Type { get; set; }

        public DateTime DateSubjectPassing { get; set; }

        public byte SemesterNumber { get; set; }
    }
}