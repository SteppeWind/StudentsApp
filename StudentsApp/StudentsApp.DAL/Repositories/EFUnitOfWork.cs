using StudentsApp.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentsApp.DAL.Entities;
using StudentsApp.DAL.EF;
using StudentsApp.DAL.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

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

        private AbstractRepository<Group> groupRepository;
        public AbstractRepository<Group> GroupRepository
        {
            get
            {
                if (groupRepository == null)
                {
                    groupRepository = new BaseRepository<Group>(dbContext);
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
                    facultyRepository = new BaseRepository<Faculty>(dbContext);
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

        private AbstractPersonRepository<Student> studentRepository;
        public AbstractPersonRepository<Student> StudentRepository
        {
            get
            {
                if (studentRepository == null)
                {
                    studentRepository = new PersonRepository<Student>(dbContext);
                }

                return studentRepository;
            }
        }

        private AbstractPersonRepository<Teacher> teacherRepository;
        public AbstractPersonRepository<Teacher> TeacherRepository
        {
            get
            {
                if (teacherRepository == null)
                {
                    teacherRepository = new PersonRepository<Teacher>(dbContext);
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
                    subjectRepository = new BaseRepository<Subject>(dbContext);
                }

                return subjectRepository;
            }
        }

        private AbstractPersonRepository<Dean> deanRepository;
        public AbstractPersonRepository<Dean> DeanRepository
        {
            get
            {
                if (deanRepository == null)
                {
                    deanRepository = new PersonRepository<Dean>(dbContext);
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

        private AbstractRepository<StudentSubject> visitSubjectRepository;
        public AbstractRepository<StudentSubject> StudentSubjectRepository
        {
            get
            {
                if (visitSubjectRepository == null)
                {
                    visitSubjectRepository = new BaseRepository<StudentSubject>(dbContext);
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

        private AbstractRepository<TeacherSubject> teacherSubjectRepository;
        public AbstractRepository<TeacherSubject> TeacherSubjectRepository
        {
            get
            {
                if (teacherSubjectRepository == null)
                {
                    teacherSubjectRepository = new BaseRepository<TeacherSubject>(dbContext);
                }

                return teacherSubjectRepository;
            }
        }

        private AbstractRepository<StudentGroup> studentGroupRepository;
        public AbstractRepository<StudentGroup> StudentGroupRepository
        {
            get
            {
                if (studentGroupRepository == null)
                {
                    studentGroupRepository = new BaseRepository<StudentGroup>(dbContext);
                }

                return studentGroupRepository;
            }
        }


        private ApplicationProfileManager profileManager;
        public ApplicationProfileManager ProfileManager
        {
            get
            {
                if (profileManager == null)
                {
                    profileManager = new ApplicationProfileManager(new UserStore<Profile>(dbContext));
                }

                return profileManager;
            }
        }

        private ApplicationRoleManager roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                if (roleManager == null)
                {
                    roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(dbContext));
                }

                return roleManager;
            }
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
