using StudentsApp.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Contracts
{
    public interface IStudentService : IPersonService<StudentDTO>
    {
        void AddSubject(int idStudent, int idSubject, int idTeacher);
        void AddSubject(string emailStudent, int idSubject, int idTeacher);
        void AddSubject(int idStudent, IEnumerable<int> idsSubjects, int teacherId);
        void AddSubject(int idStudent, IEnumerable<int> idsSubjects, IEnumerable<int> idsTeachers);
        void AddSubject(string emailStudent, IEnumerable<int> idsSubjects, IEnumerable<int> idsTeachers);
        void RemoveSubject(int idStudent, int idSubject);

        IEnumerable<StudentDTO> GetStudentsMoreMiddleAverageMark(int idFaculty);
        IEnumerable<StudentDTO> GetStudents(int idFaculty);
        IEnumerable<StudentDTO> GetStudents(int idSubject, int idTeacher);
    }
}