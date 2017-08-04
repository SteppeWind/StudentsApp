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
        IEnumerable<SubjectDTO> GetSubjectsInFaculty(int idFaculty);
        IEnumerable<SubjectDTO> GetStudentSubjects(int studentId);
        IEnumerable<SubjectDTO> GetSubjectsInFacultyFromTeacher(int idFaculty, int idTeacher);
    }
}