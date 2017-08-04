using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Contracts
{
    public interface IUnitOfWork 
    {
        IStartRepository StartRepository { get; }

        AbstractRepository<VisitSubject> VisitSubjectRepository { get; }

        AbstractRepository<DeanFaculty> DeanFacultyRepository { get; }

        AbstractProfileRepository<Profile> ProfileRepository { get; }

        AbstractGroupRepository<Group> GroupRepository { get; }

        AbstractRepository<PostTeacher> PostTeacherRepository { get; }

        AbstractRepository<Faculty> FacultyRepository { get; }

        AbstractRepository<TeacherFaculty> TeacherFacultyRepository { get; }

        AbstractStudentRepository<Student> StudentRepository { get; }

        AbstractTeacherRepository<Teacher> TeacherRepository { get; }

        AbstractRepository<Subject> SubjectRepository { get; }

        AbstractDeanRepository<Dean> DeanRepository { get; }

        AbstractRepository<Mark> MarkRepository { get; }

        void Save();
    }
}