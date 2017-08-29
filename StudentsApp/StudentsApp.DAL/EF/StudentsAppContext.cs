using Microsoft.AspNet.Identity.EntityFramework;
using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.EF
{
    public class StudentsAppContext : IdentityDbContext<Profile>
    {
        private StudentsAppContext() : base("StudentsDB") { }

        static StudentsAppContext()
        {
            context = new StudentsAppContext();            
        }

        public void FillData()
        {
            Seed();
            Seed2();
            //Seed3();
            Seed4();
        }

        public void Seed()
        {
            using (context = new StudentsAppContext())
            {
                #region Факультеты                           

                context.Faculties.Add(new Faculty() { FacultyName = "Факультет Бизнес-Информатика" });
                context.Faculties.Add(new Faculty() { FacultyName = "Управление персоналом" });
                context.Faculties.Add(new Faculty() { FacultyName = "Управление процессами перевозок" });

                #endregion
                
                #region Названия званий учителей

                context.PostTeachers.Add(new PostTeacher() { NamePostTeacher = "Ассистент" });
                context.PostTeachers.Add(new PostTeacher() { NamePostTeacher = "Старший преподаватель" });
                context.PostTeachers.Add(new PostTeacher() { NamePostTeacher = "Доцент" });
                context.PostTeachers.Add(new PostTeacher() { NamePostTeacher = "Профессор" });

                #endregion

                #region Деканы

                context.Deans.Add(new Dean()
                {
                    Profile = new Profile()
                    {
                        Name = "Валерий",
                        Surname = "Хабаров",
                        MiddleName = "Иванович",
                        Email = "fbi@stu.ru"                        
                    }
                });                

                context.Deans.Add(new Dean()
                {
                    Profile = new Profile()
                    {
                        Name = "Валентина",
                        Surname = "Николаенко",
                        MiddleName = "Михайловна",
                        Email = "up@stu.ru"                        
                    }
                });

                context.Deans.Add(new Dean()
                {
                    Profile = new Profile()
                    {
                        Name = "Александр",
                        Surname = "Климов",
                        MiddleName = "Александрович",
                        Email = "upp@stu.ru"
                    }
                });

                #endregion

                #region Преподаватели

                #region ФБИ

                context.Teachers.Add(new Teacher()
                {
                    Profile = new Profile()
                    {
                        Name = "Евгений",
                        Surname = "Тарасов",
                        MiddleName = "Борисович",
                        Email = "TarasovEB@mail.ru"
                    }
                });

                context.Teachers.Add(new Teacher()
                {
                    Profile = new Profile()
                    {
                        Name = "Максим",
                        Surname = "Жуков",
                        MiddleName = "Витальевич",
                        Email = "ZhikovMV@yandex.ru"
                    }
                });

                context.Teachers.Add(new Teacher()
                {
                    Profile = new Profile()
                    {
                        Name = "Валерий",
                        Surname = "Кобылянский",
                        MiddleName = "Григорьевич",
                        Email = "KobilynskiVG@inbox.ru"
                    }
                });

                context.Teachers.Add(new Teacher()
                {
                    Profile = new Profile()
                    {
                        Name = "Ксения",
                        Surname = "Землянская",
                        MiddleName = "Викторовна",
                        Email = "ZemlynskayKV1992@mail.ru"
                    }
                });

                context.Teachers.Add(new Teacher()
                {
                    Profile = new Profile()
                    {
                        Name = "Марина",
                        Surname = "Нартова",
                        MiddleName = "Владимировна",
                        Email = "NartovaMari@mail.ru"
                    }
                });

                context.Teachers.Add(new Teacher()
                {
                    Profile = new Profile()
                    {
                        Name = "Наиля",
                        Surname = "Агуленко",
                        MiddleName = "Ильдусовна",
                        Email = "evgen69@yandex.ru"
                    }
                });

                #endregion

                #region УП

                context.Teachers.Add(new Teacher()
                {
                    Profile = new Profile()
                    {
                        Name = "Елена",
                        Surname = "Тюнюкова",
                        MiddleName = "Владимировна",
                        Email = "TynykovaEV@mail.com"
                    }
                });

                context.Teachers.Add(new Teacher()
                {
                    Profile = new Profile()
                    {
                        Name = "Александра",
                        Surname = "Ломанова",
                        MiddleName = "Григорьевна",
                        Email = "Lomanova@mail.ru"
                    }
                });

                context.Teachers.Add(new Teacher()
                {
                    Profile = new Profile()
                    {
                        Name = "Александр",
                        Surname = "Лесовиченко",
                        MiddleName = "Михайлович",
                        Email = "LesovichenkoAM@mail.ru"
                    }
                });

                context.Teachers.Add(new Teacher()
                {
                    Profile = new Profile()
                    {
                        Name = "Афанасий",
                        Surname = "Шадт",
                        MiddleName = "Артемович",
                        Email = "Shadt@yandex.ru"
                    }
                });

                context.Teachers.Add(new Teacher()
                {
                    Profile = new Profile()
                    {
                        Name = "Олег",
                        Surname = "Кашник",
                        MiddleName = "Иванович",
                        Email = "CashnikOI@yandex.ru"
                    }
                });

                context.Teachers.Add(new Teacher()
                {
                    Profile = new Profile()
                    {
                        Name = "Николай",
                        Surname = "Лукъяненко",
                        MiddleName = "Васильевич",
                        Email = "LukynenkoNV@ymail.ru"
                    }
                });

                #endregion

                #region УПП

                context.Teachers.Add(new Teacher()
                {
                    Profile = new Profile()
                    {
                        Name = "Василий",
                        Surname = "Черненко",
                        MiddleName = "Михайлович",
                        Email = "Chernenko@mail.ru"
                    }
                });

                context.Teachers.Add(new Teacher()
                {
                    Profile = new Profile()
                    {
                        Name = "Елена",
                        Surname = "Псеровская",
                        MiddleName = "Дмитриевна",
                        Email = "PserovkayED@mail.ru"
                    }
                });

                context.Teachers.Add(new Teacher()
                {
                    Profile = new Profile()
                    {
                        Name = "Валентин",
                        Surname = "Натеров",
                        MiddleName = "Васильевич",
                        Email = "NaterovVV@mail.ru"
                    }
                });

                context.Teachers.Add(new Teacher()
                {
                    Profile = new Profile()
                    {
                        Name = "Дмитрий",
                        Surname = "Осипов",
                        MiddleName = "Васильевич",
                        Email = "Osipov@yandex.ru"
                    }
                });

                context.Teachers.Add(new Teacher()
                {
                    Profile = new Profile()
                    {
                        Name = "Ольга",
                        Surname = "Югрина",
                        MiddleName = "Петровна",
                        Email = "Yrgina94@mail.ru"
                    }
                });

                context.Teachers.Add(new Teacher()
                {
                    Profile = new Profile()
                    {
                        Name = "Дарья",
                        Surname = "Гришкова",
                        MiddleName = "Юрьевна",
                        Email = "Grishko@inbox.ru"
                    }
                });

                #endregion

                #endregion

                #region Студенты

                #region ФБИ

                #region БИСТ-312

                context.Students.Add(new Student()
                {
                    Profile = new Profile()
                    {
                        Name = "Максим",
                        Surname = "Климов",
                        MiddleName = "Дмитриевич",
                        Email = "zoyoo1994@mail.ru"
                    }
                });

                context.Students.Add(new Student()
                {
                    Profile = new Profile()
                    {
                        Name = "Анастасия",
                        Surname = "Орещенко",
                        MiddleName = "Дмитриевна",
                        Email = "OrehNasty@mail.ru"
                    }
                });

                context.Students.Add(new Student()
                {
                    Profile = new Profile()
                    {
                        Name = "Дмитрий",
                        Surname = "Тошилов",
                        MiddleName = "Дмитриевич",
                        Email = "ToshilovDD@yandex.ru"
                    }
                });

                context.Students.Add(new Student()
                {
                    Profile = new Profile()
                    {
                        Name = "Владислав",
                        Surname = "Коротаев",
                        MiddleName = "Станиславович",
                        Email = "vladislav-korotaev-97@mail.ru"
                    }
                });

                context.Students.Add(new Student()
                {
                    Profile = new Profile()
                    {
                        Name = "Вероника",
                        Surname = "Коренькова",
                        MiddleName = "Васильевна",
                        Email = "KorenkovaVV@mail.com"
                    }
                });

                #endregion

                #region БИСТ-311

                context.Students.Add(new Student()
                {
                    Profile = new Profile()
                    {
                        Name = "Максим",
                        Surname = "Драгаев",
                        MiddleName = "Иванович",
                        Email = "Dragaev@yandex.ru"
                    }
                });

                context.Students.Add(new Student()
                {
                    Profile = new Profile()
                    {
                        Name = "Максим",
                        Surname = "Малахов",
                        MiddleName = "Сергеевич",
                        Email = "MaxUltimate@yandex.com"
                    }
                });

                context.Students.Add(new Student()
                {
                    Profile = new Profile()
                    {
                        Name = "Мария",
                        Surname = "Шелест",
                        MiddleName = "Михайловна",
                        Email = "Shelest@mail.ru"
                    }
                });

                context.Students.Add(new Student()
                {
                    Profile = new Profile()
                    {
                        Name = "Полина",
                        Surname = "Прокопенко",
                        MiddleName = "Сергеевна",
                        Email = "ProkopenkoPS@mail.ru"
                    }
                });

                context.Students.Add(new Student()
                {
                    Profile = new Profile()
                    {
                        Name = "Роман",
                        Surname = "Сергеев",
                        MiddleName = "Витальевич",
                        Email = "Front94@mail.ru"
                    }
                });

                #endregion

                #endregion

                #region УП

                #region РСО-111

                context.Students.Add(new Student()
                {
                    Profile = new Profile()
                    {
                        Name = "Дмитрий",
                        Surname = "Уткин",
                        MiddleName = "Дмитриевич",
                        Email = "Larin@mail.ru"
                    }
                });

                context.Students.Add(new Student()
                {
                    Profile = new Profile()
                    {
                        Name = "Евгений",
                        Surname = "Баженов",
                        MiddleName = "Сергеевич",
                        Email = "BadComdian@mail.ru"
                    }
                });

                context.Students.Add(new Student()
                {
                    Profile = new Profile()
                    {
                        Name = "Эдик",
                        Surname = "Левин",
                        MiddleName = "Евгеньевич",
                        Email = "Kingsta@inbox.ru"
                    }
                });

                context.Students.Add(new Student()
                {
                    Profile = new Profile()
                    {
                        Name = "Александр",
                        Surname = "Тимарцев",
                        MiddleName = "Витальевич",
                        Email = "Restorator@inbox.ru"
                    }
                });

                context.Students.Add(new Student()
                {
                    Profile = new Profile()
                    {
                        Name = "Алексей",
                        Surname = "Шевцов",
                        MiddleName = "Петрович",
                        Email = "itpedia@mail.ru"
                    }
                });

                #endregion

                #endregion

                #region УПП

                #region Д-315

                context.Students.Add(new Student()
                {
                    Profile = new Profile()
                    {
                        Name = "Юрий",
                        Surname = "Хованский",
                        MiddleName = "Викторович",
                        Email = "Gangster@mail.ru"
                    }
                });

                context.Students.Add(new Student()
                {
                    Profile = new Profile()
                    {
                        Name = "Эльдар",
                        Surname = "Джарахов",
                        MiddleName = "Эдуардович",
                        Email = "UspeshnayGruppa@mail.ru"
                    }
                });

                context.Students.Add(new Student()
                {
                    Profile = new Profile()
                    {
                        Name = "Мирон",
                        Surname = "Федоров",
                        MiddleName = "Янович",
                        Email = "oxxximiron@mail.ru"
                    }
                });

                context.Students.Add(new Student()
                {
                    Profile = new Profile()
                    {
                        Name = "Антон",
                        Surname = "Ватлин",
                        MiddleName = "Артемович",
                        Email = "BumbleBeezy@mail.ru"
                    }
                });

                context.Students.Add(new Student()
                {
                    Profile = new Profile()
                    {
                        Name = "Федор",
                        Surname = "Игнатьев",
                        MiddleName = "Николаевич",
                        Email = "BukerDFred@mail.ru"
                    }
                });

                #endregion

                #endregion

                #endregion                

                context.SaveChanges();
            }
        }

        public void Seed2()
        {
            using (context = new StudentsAppContext())
            {
                var faculties = context.Faculties.ToList();
                var postTeachers = context.PostTeachers.ToList();
                var teachers = context.Teachers.ToList();
                var deans = context.Deans.ToList();

                #region Звания и факультеты преподавателей

                #region ФБИ

                //Тарасов Е.Б. - доцент
                context.TeacherFaculties.Add(new TeacherFaculty()
                {
                    FacultyId = faculties[0].Id,
                    PostTeacherId =postTeachers[2].Id,
                    TeacherId = teachers.First(t => t.Profile.Surname.Contains("Тарасов")).Id
                });

                //Жуков М.В. - старший преподаватель
                context.TeacherFaculties.Add(new TeacherFaculty()
                {
                    FacultyId = faculties.ElementAt(0).Id,
                    PostTeacherId = postTeachers.ElementAt(1).Id,
                    TeacherId = teachers.First(t => t.Profile.Surname.Contains("Жуков")).Id
                });

                //Кобылянский В.Г. - профессор
                context.TeacherFaculties.Add(new TeacherFaculty()
                {
                    FacultyId = faculties.ElementAt(0).Id,
                    PostTeacherId = postTeachers.ElementAt(3).Id,
                    TeacherId = teachers.First(t => t.Profile.Surname.Contains("Кобылянский")).Id
                });

                //Землянская К.В. - старший преподаватель
                context.TeacherFaculties.Add(new TeacherFaculty()
                {
                    FacultyId = faculties.ElementAt(0).Id,
                    PostTeacherId = postTeachers.ElementAt(1).Id,
                    TeacherId = teachers.First(t => t.Profile.Surname.Contains("Землянская")).Id
                });

                //Нартова М.В. - старший преподаватель
                context.TeacherFaculties.Add(new TeacherFaculty()
                {
                    FacultyId = faculties.ElementAt(0).Id,
                    PostTeacherId = postTeachers.ElementAt(1).Id,
                    TeacherId = teachers.First(t => t.Profile.Surname.Contains("Нартова")).Id
                });

                //Агуленко Н.И. - доцент
                context.TeacherFaculties.Add(new TeacherFaculty()
                {
                    FacultyId = faculties.ElementAt(0).Id,
                    PostTeacherId = postTeachers.ElementAt(2).Id,
                    TeacherId = teachers.First(t => t.Profile.Surname.Contains("Агуленко")).Id
                });

                #endregion

                #region УП

                //Тюнюкова Е.В. - доцент 
                context.TeacherFaculties.Add(new TeacherFaculty()
                {
                    FacultyId = faculties.ElementAt(1).Id,
                    PostTeacherId = postTeachers.ElementAt(2).Id,
                    TeacherId = teachers.First(t => t.Profile.Surname.Contains("Тюнюкова")).Id
                });

                //Ломанова А.Г. - доцент 
                context.TeacherFaculties.Add(new TeacherFaculty()
                {
                    FacultyId = faculties.ElementAt(1).Id,
                    PostTeacherId = postTeachers.ElementAt(2).Id,
                    TeacherId = teachers.First(t => t.Profile.Surname.Contains("Ломанова")).Id
                });

                //Лесовиченко А.М. - старший преподаватель
                context.TeacherFaculties.Add(new TeacherFaculty()
                {
                    FacultyId = faculties.ElementAt(1).Id,
                    PostTeacherId = postTeachers.ElementAt(1).Id,
                    TeacherId = teachers.First(t => t.Profile.Surname.Contains("Лесовиченко")).Id
                });

                //Шадт А.А. - профессор
                context.TeacherFaculties.Add(new TeacherFaculty()
                {
                    FacultyId = faculties.ElementAt(1).Id,
                    PostTeacherId = postTeachers.ElementAt(3).Id,
                    TeacherId = teachers.First(t => t.Profile.Surname.Contains("Шадт")).Id
                });

                //Кашник О.И. - старший преподаватель
                context.TeacherFaculties.Add(new TeacherFaculty()
                {
                    FacultyId = faculties.ElementAt(1).Id,
                    PostTeacherId = postTeachers.ElementAt(1).Id,
                    TeacherId = teachers.First(t => t.Profile.Surname.Contains("Кашник")).Id
                });

                //Лукъяненко Н.В. - старший преподаватель
                context.TeacherFaculties.Add(new TeacherFaculty()
                {
                    FacultyId = faculties.ElementAt(1).Id,
                    PostTeacherId = postTeachers.ElementAt(1).Id,
                    TeacherId = teachers.First(t => t.Profile.Surname.Contains("Лукъяненко")).Id
                });

                #endregion

                #region УПП

                //Черненко В.М. - старший преподаватель
                context.TeacherFaculties.Add(new TeacherFaculty()
                {
                    FacultyId = faculties.Last().Id,
                    PostTeacherId = postTeachers.ElementAt(1).Id,
                    TeacherId = teachers.First(t => t.Profile.Surname.Contains("Черненко")).Id
                });

                //Псеровская Е.Д. - доцент
                context.TeacherFaculties.Add(new TeacherFaculty()
                {
                    FacultyId = faculties.Last().Id,
                    PostTeacherId = postTeachers.ElementAt(2).Id,
                    TeacherId = teachers.First(t => t.Profile.Surname.Contains("Псеровская")).Id
                });

                //Натеров В.В. - доцент
                context.TeacherFaculties.Add(new TeacherFaculty()
                {
                    FacultyId = faculties.Last().Id,
                    PostTeacherId = postTeachers.ElementAt(2).Id,
                    TeacherId = teachers.First(t => t.Profile.Surname.Contains("Натеров")).Id
                });

                //Осипов Д.В. - старший преподаватель
                context.TeacherFaculties.Add(new TeacherFaculty()
                {
                    FacultyId = faculties.Last().Id,
                    PostTeacherId = postTeachers.ElementAt(1).Id,
                    TeacherId = teachers.First(t => t.Profile.Surname.Contains("Осипов")).Id
                });

                //Югрина О.П. - ассистент
                context.TeacherFaculties.Add(new TeacherFaculty()
                {
                    FacultyId = faculties.Last().Id,
                    PostTeacherId = postTeachers.ElementAt(0).Id,
                    TeacherId = teachers.First(t => t.Profile.Surname.Contains("Югрина")).Id
                });

                //Гришкова Д.Ю. - старший преподаватель
                context.TeacherFaculties.Add(new TeacherFaculty()
                {
                    FacultyId = faculties.Last().Id,
                    PostTeacherId = postTeachers.ElementAt(0).Id,
                    TeacherId = teachers.First(t => t.Profile.Surname.Contains("Гришкова")).Id
                });

                #endregion

                #endregion

                #region Предметы 

                #region ФБИ

                context.Subjects.Add(new Subject()
                {
                    SubjectName = "Объекто ориентированное программирование",
                    FacultyId = faculties.First().Id
                });

                context.Subjects.Add(new Subject()
                {
                    SubjectName = "Технологии программирования",
                    FacultyId = faculties.First().Id
                });

                context.Subjects.Add(new Subject()
                {
                    SubjectName = "Информатика",
                    FacultyId = faculties.First().Id
                });

                context.Subjects.Add(new Subject()
                {
                    SubjectName = "Операционные системы, среды и оболочки",
                    FacultyId = faculties.First().Id
                });

                context.Subjects.Add(new Subject()
                {
                    SubjectName = "Архитектура электронных вычислительных машин",
                    FacultyId = faculties.First().Id
                });

                context.Subjects.Add(new Subject()
                {
                    SubjectName = "Инфокоммуникационные системы и сети",
                    FacultyId = faculties.First().Id
                });

                context.Subjects.Add(new Subject()
                {
                    SubjectName = "Управление данными",
                    FacultyId = faculties.First().Id
                });

                #endregion

                #region УП

                context.Subjects.Add(new Subject()
                {
                    SubjectName = "Основы теории управления",
                    FacultyId = faculties.ElementAt(1).Id
                });

                context.Subjects.Add(new Subject()
                {
                    SubjectName = "Маркетинг персонала",
                    FacultyId = faculties.ElementAt(1).Id
                });

                context.Subjects.Add(new Subject()
                {
                    SubjectName = "Управленческий учет и учет персонала",
                    FacultyId = faculties.ElementAt(1).Id
                });

                context.Subjects.Add(new Subject()
                {
                    SubjectName = "Основы управления персоналом",
                    FacultyId = faculties.ElementAt(1).Id
                });

                context.Subjects.Add(new Subject()
                {
                    SubjectName = "Основы организации труда",
                    FacultyId = faculties.ElementAt(1).Id
                });

                context.Subjects.Add(new Subject()
                {
                    SubjectName = "Экономика управления персоналом",
                    FacultyId = faculties.ElementAt(1).Id
                });

                #endregion

                #region УПП

                context.Subjects.Add(new Subject()
                {
                    SubjectName = "Общий курс транспорта",
                    FacultyId = faculties.Last().Id
                });

                context.Subjects.Add(new Subject()
                {
                    SubjectName = "Управление грузовой и коммерческой работой",
                    FacultyId = faculties.Last().Id
                });

                context.Subjects.Add(new Subject()
                {
                    SubjectName = "Транспортная безопасность",
                    FacultyId = faculties.Last().Id
                });

                context.Subjects.Add(new Subject()
                {
                    SubjectName = "Математическое моделирование систем и процессов",
                    FacultyId = faculties.Last().Id
                });

                context.Subjects.Add(new Subject()
                {
                    SubjectName = "Управление эксплуатационной работой",
                    FacultyId = faculties.Last().Id
                });

                context.Subjects.Add(new Subject()
                {
                    SubjectName = "Сервис на транспорте",
                    FacultyId = faculties.Last().Id
                });

                #endregion

                #endregion

                #region Группы

                context.Groups.Add(new Group()
                {
                    GroupName = "БИСТ-311",
                    FacultyId = faculties.First().Id
                });

                context.Groups.Add(new Group()
                {
                    GroupName = "БИСТ-312",
                    FacultyId = faculties.First().Id
                });

                context.Groups.Add(new Group()
                {
                    GroupName = "РСО-111",
                    FacultyId = faculties.ElementAt(1).Id
                });

                context.Groups.Add(new Group()
                {
                    GroupName = "Д-315",
                    FacultyId = faculties.Last().Id
                });

                #endregion

                #region Деканы факультетов

                context.DeanFaculties.Add(new DeanFaculty()
                {
                    DeanId = deans.First().Id,
                    FacultyId = faculties.First().Id,
                    StartManage = DateTime.Now
                });

                context.DeanFaculties.Add(new DeanFaculty()
                {
                    DeanId = deans.ElementAt(1).Id,
                    FacultyId = faculties.ElementAt(1).Id,
                    StartManage = DateTime.Now
                });

                context.DeanFaculties.Add(new DeanFaculty()
                {
                    DeanId = deans.Last().Id,
                    FacultyId = faculties.Last().Id,
                    StartManage = DateTime.Now
                });

                #endregion

                context.SaveChanges();
            }
        }

        /* public void Seed3()
         {
             #region Оценки

             context.Marks.Add(new ExamMark()
             {
                 StudentId = 22,
                 SubjectId = 1,
                 TeacherId = 5,
                 DateSubjectPassing = new DateTime(2016, 6, 15),
                 SemesterNumber = 4,
                 Mark = 2
             });

             context.Marks.Add(new ExamMark()
             {
                 StudentId = 22,
                 SubjectId = 1,
                 TeacherId = 5,
                 DateSubjectPassing = new DateTime(2016, 9, 20),
                 SemesterNumber = 5,
                 Mark = 3
             });

             context.Marks.Add(new ExamMark()
             {
                 StudentId = 23,
                 SubjectId = 1,
                 TeacherId = 5,
                 DateSubjectPassing = new DateTime(2016, 6, 15),
                 SemesterNumber = 4,
                 Mark = 4
             });

             context.Marks.Add(new ExamMark()
             {
                 StudentId = 24,
                 SubjectId = 1,
                 TeacherId = 5,
                 DateSubjectPassing = new DateTime(2016, 6, 15),
                 SemesterNumber = 4,
                 Mark = 3
             });

             context.Marks.Add(new ExamMark()
             {
                 StudentId = 25,
                 SubjectId = 1,
                 TeacherId = 5,
                 DateSubjectPassing = new DateTime(2016, 6, 15),
                 SemesterNumber = 4,
                 Mark = 2
             });

             context.Marks.Add(new ExamMark()
             {
                 StudentId = 25,
                 SubjectId = 1,
                 TeacherId = 5,
                 DateSubjectPassing = new DateTime(2016, 9, 20),
                 SemesterNumber = 5,
                 Mark = 3
             });

             context.Marks.Add(new ExamMark()
             {
                 StudentId = 26,
                 SubjectId = 1,
                 TeacherId = 5,
                 DateSubjectPassing = new DateTime(2016, 6, 15),
                 SemesterNumber = 4,
                 Mark = 5
             });

             context.Marks.Add(new ExamMark()
             {
                 StudentId = 22,
                 SubjectId = 2,
                 TeacherId = 4,
                 DateSubjectPassing = new DateTime(2016, 6, 24),
                 SemesterNumber = 4,
                 Mark = 3
             });

             context.Marks.Add(new ExamMark()
             {
                 StudentId = 24,
                 SubjectId = 2,
                 TeacherId = 4,
                 DateSubjectPassing = new DateTime(2016, 6, 24),
                 SemesterNumber = 4,
                 Mark = 3
             });

             context.Marks.Add(new ExamMark()
             {
                 StudentId = 32,
                 SubjectId = 8,
                 TeacherId = 10,
                 DateSubjectPassing = new DateTime(2015, 1, 10),
                 SemesterNumber = 2,
                 Mark = 4
             });

             context.Marks.Add(new ExamMark()
             {
                 StudentId = 33,
                 SubjectId = 8,
                 TeacherId = 10,
                 DateSubjectPassing = new DateTime(2015, 1, 10),
                 SemesterNumber = 2,
                 Mark = 3
             });

             context.Marks.Add(new ExamMark()
             {
                 StudentId = 34,
                 SubjectId = 8,
                 TeacherId = 10,
                 DateSubjectPassing = new DateTime(2015, 1, 10),
                 SemesterNumber = 2,
                 Mark = 2
             });

             context.Marks.Add(new ExamMark()
             {
                 StudentId = 34,
                 SubjectId = 8,
                 TeacherId = 10,
                 DateSubjectPassing = new DateTime(2015, 2, 23),
                 SemesterNumber = 2,
                 Mark = 4
             });

             context.Marks.Add(new ExamMark()
             {
                 StudentId = 37,
                 SubjectId = 15,
                 TeacherId = 17,
                 DateSubjectPassing = new DateTime(2015, 6, 5),
                 SemesterNumber = 4,
                 Mark = 4
             });

             context.Marks.Add(new ExamMark()
             {
                 StudentId = 38,
                 SubjectId = 15,
                 TeacherId = 17,
                 DateSubjectPassing = new DateTime(2015, 6, 5),
                 SemesterNumber = 4,
                 Mark = 3
             });

             context.Marks.Add(new ExamMark()
             {
                 StudentId = 39,
                 SubjectId = 15,
                 TeacherId = 17,
                 DateSubjectPassing = new DateTime(2015, 6, 5),
                 SemesterNumber = 4,
                 Mark = 5
             });

             #endregion

             #region Преподаватели ведут предметы

             #region ФБИ

             //Тарасов ведёт дисциплину ТП
             teachers.First(t => t.Profile.Surname == "Тарасов").ListSubjects.Add(new TeacherSubject()
             {

             });

             //Тарасов ведёт дисциплину УП
             teachers.First(t => t.Id == 4).ListSubjects.Add(context.Subjects.First(s => s.Id == 7));

             //Жуков ведёт дисциплину ООП
             teachers.First(t => t.Id == 5).ListSubjects.Add(context.Subjects.First(s => s.Id == 1));

             //Жуков ведёт дисциплину Сети
             teachers.First(t => t.Id == 5).ListSubjects.Add(context.Subjects.First(s => s.Id == 6));

             //Кобылянский ведёт дисциплину архитектура ЭВМ
             teachers.First(t => t.Id == 6).ListSubjects.Add(context.Subjects.First(s => s.Id == 4));

             //Кобылянский ведёт дисциплину ОСИ
             teachers.First(t => t.Id == 6).ListSubjects.Add(context.Subjects.First(s => s.Id == 5));

             //Землянская ведёт дисциплину Сети
             teachers.First(t => t.Id == 7).ListSubjects.Add(context.Subjects.First(s => s.Id == 6));

             //Нартова ведёт дисциплину информатика
             teachers.First(t => t.Id == 8).ListSubjects.Add(context.Subjects.First(s => s.Id == 3));

             //Наиля ведёт дисциплину информатика
             teachers.First(t => t.Id == 9).ListSubjects.Add(context.Subjects.First(s => s.Id == 3));

             #endregion

             #region УП

             for (int i = 7; i <= 12; i++)
             {
                 teachers.First(t => t.Id == i + 3).ListSubjects.Add(context.Subjects.First(s => s.Id == i + 1));
             }

             #endregion

             #region УПП

             for (int i = 13; i <= 18; i++)
             {
                 teachers.First(t => t.Id == i + 3).ListSubjects.Add(context.Subjects.First(s => s.Id == i + 1));
             }

             #endregion

             #endregion

             #region Студенты в группах 

             int count = context.Groups.Count();
             for (int i = 1; i <= count; i++)
             {
                 int curr = i * 5;
                 for (int j = curr - count; j <= curr; j++)
                 {
                     context.Groups.First(g => g.Id == i).ListStudents.Add(context.Students.First(s => s.Id == j + 21));
                 }
             }

             #endregion

             context.SaveChanges();
         }*/

        public void Seed4()
        {
            #region Студенты изучают предметы

            //foreach (var student in context.Students)
            //{
            //    var fir = student.ListGroups.First();
            //    VisitSubjects.Add(new VisitSubject()
            //    {
            //        IdStudent = student.Id,
            //        IdSubject = 
            //    });
            //    student.ListSubjects.Add(context.Subjects.FirstOrDefault(subj => subj.FacultyId == fir.FacultyId));
            //}

            #endregion

            //context.SaveChanges();
        }


        private static StudentsAppContext context;
        public static StudentsAppContext StudentsContext => context;

        public DbSet<TeacherSubject> TeacherSubjects { get; set; }

        public DbSet<StudentGroup> StudentGroups { get; set; }

        public DbSet<StudentSubject> StudentSubjects { get; set; }

        public DbSet<DeanFaculty> DeanFaculties { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<PostTeacher> PostTeachers { get; set; }

        public DbSet<Faculty> Faculties { get; set; }

        public DbSet<TeacherFaculty> TeacherFaculties { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Dean> Deans { get; set; }

        public DbSet<Mark> Marks { get; set; }

        public DbSet<ExamMark> ExamSubjects { get; set; }

        public DbSet<TestMark> TestSubjects { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Primary keys

            modelBuilder.Entity<DeanFaculty>().HasKey(df => df.Id);

            modelBuilder.Entity<Profile>().HasKey(p => p.Id);

            modelBuilder.Entity<Group>().HasKey(g => g.Id);

            modelBuilder.Entity<PostTeacher>().HasKey(pt => pt.Id);

            modelBuilder.Entity<Faculty>().HasKey(f => f.Id);            

            modelBuilder.Entity<TeacherFaculty>().HasKey(tf => tf.Id);

            modelBuilder.Entity<Subject>().HasKey(s => s.Id);

            modelBuilder.Entity<Mark>().HasKey(m => m.Id);

            #endregion

            modelBuilder.Entity<Mark>()
                .HasRequired(m => m.Subject)
                .WithMany(s => s.ListMarks)
                .HasForeignKey(m => m.SubjectId);

            modelBuilder.Entity<Student>()
                .HasRequired(s => s.Profile)
                .WithOptional();


            modelBuilder.Entity<Teacher>()
                .HasRequired(s => s.Profile)
                .WithOptional();

            modelBuilder.Entity<Dean>()
                 .HasRequired(s => s.Profile)
                .WithOptional();

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
        }
    }
}