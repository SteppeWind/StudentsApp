using StudentsApp.DAL.Entities;
using StudentsApp.DAL.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IStartRepository StartRepository { get; }

        AbstractRepository<TeacherSubject> TeacherSubjectRepository { get; }
        
        AbstractRepository<StudentGroup> StudentGroupRepository { get; }

        AbstractRepository<StudentSubject> StudentSubjectRepository { get; }

        AbstractRepository<DeanFaculty> DeanFacultyRepository { get; }

        AbstractRepository<Group> GroupRepository { get; }

        AbstractRepository<PostTeacher> PostTeacherRepository { get; }

        AbstractRepository<Faculty> FacultyRepository { get; }

        AbstractRepository<TeacherFaculty> TeacherFacultyRepository { get; }

        AbstractPersonRepository<Student> StudentRepository { get; }

        AbstractPersonRepository<Teacher> TeacherRepository { get; }

        AbstractRepository<Subject> SubjectRepository { get; }

        AbstractPersonRepository<Dean> DeanRepository { get; }

        AbstractRepository<Mark> MarkRepository { get; }

        ApplicationProfileManager ProfileManager { get; }

        ApplicationRoleManager RoleManager { get; }

        Task SaveAsync();

        void Save();
    }
}