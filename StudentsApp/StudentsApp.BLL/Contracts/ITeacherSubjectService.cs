using StudentsApp.BLL.DTO;
using StudentsApp.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Contracts
{
    public interface ITeacherSubjectService : IBaseService<TeacherSubjectDTO>
    {
        Task<OperationDetails> AddByTeacherEmail(string teacherEmail, TeacherSubjectDTO entity);
        Task<IEnumerable<OperationDetails>> AddByTeacherEmail(string teacherEmail, IEnumerable<TeacherSubjectDTO> entities);
        Task<IEnumerable<OperationDetails>> Add(IEnumerable<TeacherSubjectDTO> entities);
        OperationDetails AddBySubjectName(string subjectName, TeacherSubjectDTO entity);
        IEnumerable<TeacherSubjectDTO> GetTeacherSubjects(string teacherId);
        TeacherSubjectDTO Get(string subjectId, string teacherId);
        OperationDetails FullRemove(string subjectId, string teacherId);
    }
}