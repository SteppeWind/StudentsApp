using StudentsApp.BLL.DTO;
using StudentsApp.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Contracts
{
    public interface IStudentSubjectService : IBaseService<StudentSubjectDTO>
    {
        Task<OperationDetails> AddByEmail(string studentEmail, StudentSubjectDTO model);
        Task<IEnumerable<OperationDetails>> AddByEmail(string studentEmail, IEnumerable<StudentSubjectDTO> models);
        Task<IEnumerable<OperationDetails>> Add(IEnumerable<StudentSubjectDTO> models);
        IEnumerable<StudentSubjectDTO> GetStudentSubjects(string subjectId, string teacherId);
        IEnumerable<StudentSubjectDTO> GetStudentSubjectsByStudentId(string studentId);
        IEnumerable<StudentSubjectDTO> GetStudentSubjectsByTeacherId(string teacherId);
    }
}
