using StudentsApp.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Contracts
{
    public interface ITeacherService : IPersonService<TeacherDTO>, IBaseService<TeacherDTO>
    {
        IEnumerable<TeacherDTO> GetTeachersWithMinCountStudents(string idFaculty);
        IEnumerable<TeacherDTO> GetTeachersWithAllStudents(string idFaculty);
        IEnumerable<TeacherDTO> GetTeachers(string idFaculty);
        IEnumerable<TeacherDTO> GetTeachersBySubjectId(string subjectId);

        //void Initialize();
        //Task InitializeAsync();
    }
}