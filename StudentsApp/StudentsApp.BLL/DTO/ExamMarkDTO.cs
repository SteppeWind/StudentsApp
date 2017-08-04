using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.DTO
{
    public class ExamMarkDTO : MarkDTO
    {
        public byte Mark { get; set; }

        public ExamMarkDTO()
        {
            Type = SubjectTypeDTO.Exam;
        }
    }
}