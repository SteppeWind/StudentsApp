using StudentsApp.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsApp.DAL.Entities;
using StudentsApp.DAL.EF;

namespace StudentsApp.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private StudentsAppContext dbContext;

        public EFUnitOfWork()
        {
            dbContext = StudentsAppContext.StudentsContext;
        }


        private AbstractRepository<DeanFaculty> deanFacultyRepository;
        public AbstractRepository<DeanFaculty> DeanFacultyRepository
        {
            get
            {
                if (deanFacultyRepository == null)
                {
                    deanFacultyRepository = new BaseRepository<DeanFaculty>(dbContext);
                }

                return deanFacultyRepository;
            }
        }

        private AbstractProfileRepository<Profile> personRepository;
        public AbstractProfileRepository<Profile> ProfileRepository
        {
            get
            {
                if (personRepository == null)
                {
                    personRepository = new ProfileRepository<Profile>(dbContext);
                }

                return personRepository;
            }
        }

        private AbstractGroupRepository<Group> groupRepository;
        public AbstractGroupRepository<Group> GroupRepository
        {
            get
            {
                if (groupRepository == null)
                {
                    groupRepository = new GroupRepository(dbContext);
                }

                return groupRepository;
            }
        }

        private AbstractRepository<PostTeacher> postTeacherRepository;
        public AbstractRepository<PostTeacher> PostTeacherRepository
        {
            get
            {
                if (postTeacherRepository == null)
                {
                    postTeacherRepository = new BaseRepository<PostTeacher>(dbContext);
                }

                return postTeacherRepository;
            }
        }

        private AbstractRepository<Faculty> facultyRepository;
        public AbstractRepository<Faculty> FacultyRepository
        {
            get
            {
                if (facultyRepository == null)
                {
                    facultyRepository = new FacultyRepository(dbContext);
                }

                return facultyRepository;
            }
        }

        private AbstractRepository<TeacherFaculty> teacherFacultyRepository;
        public AbstractRepository<TeacherFaculty> TeacherFacultyRepository
        {
            get
            {
                if (teacherFacultyRepository == null)
                {
                    teacherFacultyRepository = new BaseRepository<TeacherFaculty>(dbContext);
                }

                return teacherFacultyRepository;
            }
        }

        private AbstractStudentRepository<Student> studentRepository;
        public AbstractStudentRepository<Student> StudentRepository
        {
            get
            {
                if (studentRepository == null)
                {
                    studentRepository = new StudentRepository(dbContext);
                }

                return studentRepository;
            }
        }

        private AbstractTeacherRepository<Teacher> teacherRepository;
        public AbstractTeacherRepository<Teacher> TeacherRepository
        {
            get
            {
                if (teacherRepository == null)
                {
                    teacherRepository = new TeacherRepository(dbContext);
                }

                return teacherRepository;
            }
        }

        private AbstractRepository<Subject> subjectRepository;
        public AbstractRepository<Subject> SubjectRepository
        {
            get
            {
                if (subjectRepository == null)
                {
                    subjectRepository = new SubjectRepository(dbContext);
                }

                return subjectRepository;
            }
        }

        private AbstractDeanRepository<Dean> deanRepository;
        public AbstractDeanRepository<Dean> DeanRepository
        {
            get
            {
                if (deanRepository == null)
                {
                    deanRepository = new DeanRepository(dbContext);
                }

                return deanRepository;
            }
        }

        private AbstractRepository<Mark> markRepository;
        public AbstractRepository<Mark> MarkRepository
        {
            get
            {
                if (markRepository == null)
                {
                    markRepository = new BaseRepository<Mark>(dbContext);
                }

                return markRepository;
            }
        }

        private AbstractRepository<VisitSubject> visitSubjectRepository;
        public AbstractRepository<VisitSubject> VisitSubjectRepository
        {
            get
            {
                if (visitSubjectRepository == null)
                {
                    visitSubjectRepository = new BaseRepository<VisitSubject>(dbContext);
                }

                return visitSubjectRepository;
            }
        }

        private IStartRepository startRepository;
        public IStartRepository StartRepository
        {
            get
            {
                if (startRepository == null)
                {
                    startRepository = new StartRepository();
                }

                return startRepository;
            }
        }
        public void Save()
        {
            dbContext.SaveChanges();
        }
    }
}
