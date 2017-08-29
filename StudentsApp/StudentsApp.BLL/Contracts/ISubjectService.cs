using StudentsApp.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Contracts
{
    public interface ISubjectService : IBaseService<SubjectDTO>
    {
        IEnumerable<SubjectDTO> GetSubjectsInFaculty(string idFaculty);
        IEnumerable<SubjectDTO> GetStudentSubjects(string studentId);
        IEnumerable<SubjectDTO> GetSubjectsInFacultyFromTeacher(string idFaculty, string idTeacher);
    }
}