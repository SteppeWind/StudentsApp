using StudentsApp.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Contracts
{
    public interface IMarkService : IBaseService<MarkDTO>
    {
        IEnumerable<ExamMarkDTO> GetExams { get; }
        IEnumerable<TestMarkDTO> GetTests { get; }
        IEnumerable<MarkDTO> GetStudentMarks(int idStudent);
    }
}