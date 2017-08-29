using StudentsApp.BLL.DTO;
using StudentsApp.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Contracts
{
    public interface IStudentService : IPersonService<StudentDTO>
    {        
        IEnumerable<StudentDTO> GetStudentsMoreMiddleAverageMark(string idFaculty);
        IEnumerable<StudentDTO> GetStudents(string idFaculty);
        IEnumerable<StudentDTO> GetStudents(string idSubject, string idTeacher);
    }
}