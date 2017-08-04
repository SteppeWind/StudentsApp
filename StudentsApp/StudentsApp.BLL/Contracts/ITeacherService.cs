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
        void AddTeacherPost(int idTeacher, int idFaculty, int idPost);
        void AddTeacherPost(string teacherEmail, int idFaculty, int idPost);
        void RemoveTeacherPost(int idTeacher, int idFaculty, int idPost);
        void UpdateTeacherPost(int idTeacher, int idFaculty, int idOldPost, int idNewPost);
        void AddSubject(int idSubject, int idTeacher);
        void AddSubject(IEnumerable<int> idSubjects, string teacherEmail);
        void AddSubject(int idSubject, string teacherEmail);
        void RemoveSubject(int idSubject, int idTeacher);

        IEnumerable<TeacherFacultyDTO> GetPosts(int idTeacher);
        IEnumerable<TeacherDTO> GetTeachersWithMinCountStudents(int idFaculty);
        IEnumerable<TeacherDTO> GetTeachersWithAllStudents(int idFaculty);

        IEnumerable<TeacherDTO> GetTeachers(int idFaculty);

        //void Initialize();
        //Task InitializeAsync();
    }
}