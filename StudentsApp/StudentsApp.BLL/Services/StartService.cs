using StudentsApp.BLL.Contracts;
using StudentsApp.DAL.Contracts;
using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Services
{
    public class StartService : IStartService
    {
        private IUnitOfWork DataBase;

        public StartService(IUnitOfWork uow)
        {
            DataBase = uow;
        }

        public bool IsExistDB => DataBase.StartRepository.IsExistDB;

        public void ClearData()
        {
            DataBase.StartRepository.ClearDB();
        }

        public async Task FillDataAsync()
        {
            List<Profile> profiles = new List<Profile>();

            #region Seed 1

            #region Факультеты                           

            DataBase.FacultyRepository.Add(new Faculty() { FacultyName = "Управление процессами перевозок" });
            DataBase.FacultyRepository.Add(new Faculty() { FacultyName = "Управление персоналом" });
            DataBase.FacultyRepository.Add(new Faculty() { FacultyName = "Факультет Бизнес-Информатика" });

            #endregion

            #region Названия званий учителей

            DataBase.PostTeacherRepository.Add(new PostTeacher() { NamePostTeacher = "Ассистент" });
            DataBase.PostTeacherRepository.Add(new PostTeacher() { NamePostTeacher = "Старший преподаватель" });
            DataBase.PostTeacherRepository.Add(new PostTeacher() { NamePostTeacher = "Доцент" });
            DataBase.PostTeacherRepository.Add(new PostTeacher() { NamePostTeacher = "Профессор" });

            #endregion

            #region Деканы

            var Profile = new Profile()
            {
                Name = "Валерий",
                Surname = "Хабаров",
                MiddleName = "Иванович",
                Email = "fbi@stu.ru",
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Валентина",
                Surname = "Николаенко",
                MiddleName = "Михайловна",
                Email = "up@stu.ru",
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Александр",
                Surname = "Климов",
                MiddleName = "Александрович",
                Email = "upp@stu.ru",
            };
            profiles.Add(Profile);

            #endregion

            #region Преподаватели

            #region ФБИ

            Profile = new Profile()
            {
                Name = "Евгений",
                Surname = "Тарасов",
                MiddleName = "Борисович",
                Email = "TarasovEB@mail.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Максим",
                Surname = "Жуков",
                MiddleName = "Витальевич",
                Email = "ZhikovMV@yandex.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Валерий",
                Surname = "Кобылянский",
                MiddleName = "Григорьевич",
                Email = "KobilynskiVG@inbox.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Ксения",
                Surname = "Землянская",
                MiddleName = "Викторовна",
                Email = "ZemlynskayKV1992@mail.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Марина",
                Surname = "Нартова",
                MiddleName = "Владимировна",
                Email = "NartovaMari@mail.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Наиля",
                Surname = "Агуленко",
                MiddleName = "Ильдусовна",
                Email = "evgen69@yandex.ru"
            };
            profiles.Add(Profile);

            #endregion

            #region УП

            Profile = new Profile()
            {
                Name = "Елена",
                Surname = "Тюнюкова",
                MiddleName = "Владимировна",
                Email = "TynykovaEV@mail.com"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Александра",
                Surname = "Ломанова",
                MiddleName = "Григорьевна",
                Email = "Lomanova@mail.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Александр",
                Surname = "Лесовиченко",
                MiddleName = "Михайлович",
                Email = "LesovichenkoAM@mail.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Афанасий",
                Surname = "Шадт",
                MiddleName = "Артемович",
                Email = "Shadt@yandex.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Олег",
                Surname = "Кашник",
                MiddleName = "Иванович",
                Email = "CashnikOI@yandex.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Николай",
                Surname = "Лукъяненко",
                MiddleName = "Васильевич",
                Email = "LukynenkoNV@ymail.ru"
            };
            profiles.Add(Profile);

            #endregion

            #region УПП

            Profile = new Profile()
            {
                Name = "Василий",
                Surname = "Черненко",
                MiddleName = "Михайлович",
                Email = "Chernenko@mail.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Елена",
                Surname = "Псеровская",
                MiddleName = "Дмитриевна",
                Email = "PserovkayED@mail.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Валентин",
                Surname = "Натеров",
                MiddleName = "Васильевич",
                Email = "NaterovVV@mail.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Дмитрий",
                Surname = "Осипов",
                MiddleName = "Васильевич",
                Email = "Osipov@yandex.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Ольга",
                Surname = "Югрина",
                MiddleName = "Петровна",
                Email = "Yrgina94@mail.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Дарья",
                Surname = "Гришкова",
                MiddleName = "Юрьевна",
                Email = "Grishko@inbox.ru"
            };
            profiles.Add(Profile);

            #endregion

            #endregion

            #region Студенты

            #region ФБИ

            #region БИСТ-312

            Profile = new Profile()
            {
                Name = "Максим",
                Surname = "Климов",
                MiddleName = "Дмитриевич",
                Email = "zoyoo1994@mail.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Анастасия",
                Surname = "Орещенко",
                MiddleName = "Дмитриевна",
                Email = "OrehNasty@mail.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Дмитрий",
                Surname = "Тошилов",
                MiddleName = "Дмитриевич",
                Email = "ToshilovDD@yandex.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Владислав",
                Surname = "Коротаев",
                MiddleName = "Станиславович",
                Email = "vladislav-korotaev-97@mail.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Вероника",
                Surname = "Коренькова",
                MiddleName = "Васильевна",
                Email = "KorenkovaVV@mail.com"
            };
            profiles.Add(Profile);

            #endregion

            #region БИСТ-311

            Profile = new Profile()
            {
                Name = "Максим",
                Surname = "Драгаев",
                MiddleName = "Иванович",
                Email = "Dragaev@yandex.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Максим",
                Surname = "Малахов",
                MiddleName = "Сергеевич",
                Email = "MaxUltimate@yandex.com"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Мария",
                Surname = "Шелест",
                MiddleName = "Михайловна",
                Email = "Shelest@mail.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Полина",
                Surname = "Прокопенко",
                MiddleName = "Сергеевна",
                Email = "ProkopenkoPS@mail.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Роман",
                Surname = "Сергеев",
                MiddleName = "Витальевич",
                Email = "Front94@mail.ru"
            };
            profiles.Add(Profile);

            #endregion

            #endregion

            #region УП

            #region РСО-111

            Profile = new Profile()
            {
                Name = "Дмитрий",
                Surname = "Уткин",
                MiddleName = "Дмитриевич",
                Email = "Larin@mail.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Евгений",
                Surname = "Баженов",
                MiddleName = "Сергеевич",
                Email = "BadComdian@mail.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Эдик",
                Surname = "Левин",
                MiddleName = "Евгеньевич",
                Email = "Kingsta@inbox.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Александр",
                Surname = "Тимарцев",
                MiddleName = "Витальевич",
                Email = "Restorator@inbox.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Алексей",
                Surname = "Шевцов",
                MiddleName = "Петрович",
                Email = "itpedia@mail.ru"
            };
            profiles.Add(Profile);

            #endregion

            #endregion

            #region УПП

            #region Д-315

            Profile = new Profile()
            {
                Name = "Юрий",
                Surname = "Хованский",
                MiddleName = "Викторович",
                Email = "Gangster@mail.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Эльдар",
                Surname = "Джарахов",
                MiddleName = "Эдуардович",
                Email = "UspeshnayGruppa@mail.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Мирон",
                Surname = "Федоров",
                MiddleName = "Янович",
                Email = "oxxximiron@mail.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Антон",
                Surname = "Ватлин",
                MiddleName = "Артемович",
                Email = "BumbleBeezy@mail.ru"
            };
            profiles.Add(Profile);

            Profile = new Profile()
            {
                Name = "Федор",
                Surname = "Игнатьев",
                MiddleName = "Николаевич",
                Email = "BukerDFred@mail.ru"
            };
            profiles.Add(Profile);

            #endregion

            #endregion

            #endregion

            int count = 0;
            foreach (var item in profiles)
            {
                item.UserName = item.Email;
                await DataBase.ProfileManager.CreateAsync(item, "qwe123");

                if (count >= 0 && count < 3)
                {
                    DataBase.DeanRepository.Add(new Dean() { Profile = item });
                    await DataBase.ProfileManager.AddToRoleAsync(item.Id, "dean");
                }

                if (count >= 3 && count < 21)
                {
                    DataBase.TeacherRepository.Add(new Teacher() { Profile = item });
                    await DataBase.ProfileManager.AddToRoleAsync(item.Id, "teacher");
                }

                if (count >= 21)
                {
                    DataBase.StudentRepository.Add(new Student() { Profile = item });
                    await DataBase.ProfileManager.AddToRoleAsync(item.Id, "student");
                }
                count++;
            }

            #endregion

            await DataBase.SaveAsync();

            #region Seed 2

            var faculties = DataBase.FacultyRepository.GetAll.ToList();
            var postTeachers = DataBase.PostTeacherRepository.GetAll.ToList();
            var teachers = DataBase.TeacherRepository.GetAll.ToList();
            var deans = DataBase.DeanRepository.GetAll.ToList();

            #region Звания и факультеты преподавателей

            #region ФБИ

            //Тарасов Е.Б. - доцент
            DataBase.TeacherFacultyRepository.Add(new TeacherFaculty()
            {
                FacultyId = faculties[0].Id,
                PostTeacherId = postTeachers[2].Id,
                TeacherId = teachers.First(t => t.Profile.Surname.Contains("Тарасов")).Id
            });

            //Жуков М.В. - старший преподаватель
            DataBase.TeacherFacultyRepository.Add(new TeacherFaculty()
            {
                FacultyId = faculties.ElementAt(0).Id,
                PostTeacherId = postTeachers.ElementAt(1).Id,
                TeacherId = teachers.First(t => t.Profile.Surname.Contains("Жуков")).Id
            });

            //Кобылянский В.Г. - профессор
            DataBase.TeacherFacultyRepository.Add(new TeacherFaculty()
            {
                FacultyId = faculties.ElementAt(0).Id,
                PostTeacherId = postTeachers.ElementAt(3).Id,
                TeacherId = teachers.First(t => t.Profile.Surname.Contains("Кобылянский")).Id
            });

            //Землянская К.В. - старший преподаватель
            DataBase.TeacherFacultyRepository.Add(new TeacherFaculty()
            {
                FacultyId = faculties.ElementAt(0).Id,
                PostTeacherId = postTeachers.ElementAt(1).Id,
                TeacherId = teachers.First(t => t.Profile.Surname.Contains("Землянская")).Id
            });

            //Нартова М.В. - старший преподаватель
            DataBase.TeacherFacultyRepository.Add(new TeacherFaculty()
            {
                FacultyId = faculties.ElementAt(0).Id,
                PostTeacherId = postTeachers.ElementAt(1).Id,
                TeacherId = teachers.First(t => t.Profile.Surname.Contains("Нартова")).Id
            });

            //Агуленко Н.И. - доцент
            DataBase.TeacherFacultyRepository.Add(new TeacherFaculty()
            {
                FacultyId = faculties.ElementAt(0).Id,
                PostTeacherId = postTeachers.ElementAt(2).Id,
                TeacherId = teachers.First(t => t.Profile.Surname.Contains("Агуленко")).Id
            });

            #endregion

            #region УП

            //Тюнюкова Е.В. - доцент 
            DataBase.TeacherFacultyRepository.Add(new TeacherFaculty()
            {
                FacultyId = faculties.ElementAt(1).Id,
                PostTeacherId = postTeachers.ElementAt(2).Id,
                TeacherId = teachers.First(t => t.Profile.Surname.Contains("Тюнюкова")).Id
            });

            //Ломанова А.Г. - доцент 
            DataBase.TeacherFacultyRepository.Add(new TeacherFaculty()
            {
                FacultyId = faculties.ElementAt(1).Id,
                PostTeacherId = postTeachers.ElementAt(2).Id,
                TeacherId = teachers.First(t => t.Profile.Surname.Contains("Ломанова")).Id
            });

            //Лесовиченко А.М. - старший преподаватель
            DataBase.TeacherFacultyRepository.Add(new TeacherFaculty()
            {
                FacultyId = faculties.ElementAt(1).Id,
                PostTeacherId = postTeachers.ElementAt(1).Id,
                TeacherId = teachers.First(t => t.Profile.Surname.Contains("Лесовиченко")).Id
            });

            //Шадт А.А. - профессор
            DataBase.TeacherFacultyRepository.Add(new TeacherFaculty()
            {
                FacultyId = faculties.ElementAt(1).Id,
                PostTeacherId = postTeachers.ElementAt(3).Id,
                TeacherId = teachers.First(t => t.Profile.Surname.Contains("Шадт")).Id
            });

            //Кашник О.И. - старший преподаватель
            DataBase.TeacherFacultyRepository.Add(new TeacherFaculty()
            {
                FacultyId = faculties.ElementAt(1).Id,
                PostTeacherId = postTeachers.ElementAt(1).Id,
                TeacherId = teachers.First(t => t.Profile.Surname.Contains("Кашник")).Id
            });

            //Лукъяненко Н.В. - старший преподаватель
            DataBase.TeacherFacultyRepository.Add(new TeacherFaculty()
            {
                FacultyId = faculties.ElementAt(1).Id,
                PostTeacherId = postTeachers.ElementAt(1).Id,
                TeacherId = teachers.First(t => t.Profile.Surname.Contains("Лукъяненко")).Id
            });

            #endregion

            #region УПП

            //Черненко В.М. - старший преподаватель
            DataBase.TeacherFacultyRepository.Add(new TeacherFaculty()
            {
                FacultyId = faculties.Last().Id,
                PostTeacherId = postTeachers.ElementAt(1).Id,
                TeacherId = teachers.First(t => t.Profile.Surname.Contains("Черненко")).Id
            });

            //Псеровская Е.Д. - доцент
            DataBase.TeacherFacultyRepository.Add(new TeacherFaculty()
            {
                FacultyId = faculties.Last().Id,
                PostTeacherId = postTeachers.ElementAt(2).Id,
                TeacherId = teachers.First(t => t.Profile.Surname.Contains("Псеровская")).Id
            });

            //Натеров В.В. - доцент
            DataBase.TeacherFacultyRepository.Add(new TeacherFaculty()
            {
                FacultyId = faculties.Last().Id,
                PostTeacherId = postTeachers.ElementAt(2).Id,
                TeacherId = teachers.First(t => t.Profile.Surname.Contains("Натеров")).Id
            });

            //Осипов Д.В. - старший преподаватель
            DataBase.TeacherFacultyRepository.Add(new TeacherFaculty()
            {
                FacultyId = faculties.Last().Id,
                PostTeacherId = postTeachers.ElementAt(1).Id,
                TeacherId = teachers.First(t => t.Profile.Surname.Contains("Осипов")).Id
            });

            //Югрина О.П. - ассистент
            DataBase.TeacherFacultyRepository.Add(new TeacherFaculty()
            {
                FacultyId = faculties.Last().Id,
                PostTeacherId = postTeachers.ElementAt(0).Id,
                TeacherId = teachers.First(t => t.Profile.Surname.Contains("Югрина")).Id
            });

            //Гришкова Д.Ю. - старший преподаватель
            DataBase.TeacherFacultyRepository.Add(new TeacherFaculty()
            {
                FacultyId = faculties.Last().Id,
                PostTeacherId = postTeachers.ElementAt(0).Id,
                TeacherId = teachers.First(t => t.Profile.Surname.Contains("Гришкова")).Id
            });

            #endregion

            #endregion

            #region Предметы 

            #region ФБИ

            DataBase.SubjectRepository.Add(new Subject()
            {
                SubjectName = "Объекто ориентированное программирование",
                FacultyId = faculties.First().Id
            });

            DataBase.SubjectRepository.Add(new Subject()
            {
                SubjectName = "Технологии программирования",
                FacultyId = faculties.First().Id
            });

            DataBase.SubjectRepository.Add(new Subject()
            {
                SubjectName = "Информатика",
                FacultyId = faculties.First().Id
            });

            DataBase.SubjectRepository.Add(new Subject()
            {
                SubjectName = "Операционные системы, среды и оболочки",
                FacultyId = faculties.First().Id
            });

            DataBase.SubjectRepository.Add(new Subject()
            {
                SubjectName = "Архитектура электронных вычислительных машин",
                FacultyId = faculties.First().Id
            });

            DataBase.SubjectRepository.Add(new Subject()
            {
                SubjectName = "Инфокоммуникационные системы и сети",
                FacultyId = faculties.First().Id
            });

            DataBase.SubjectRepository.Add(new Subject()
            {
                SubjectName = "Управление данными",
                FacultyId = faculties.First().Id
            });

            #endregion

            #region УП

            DataBase.SubjectRepository.Add(new Subject()
            {
                SubjectName = "Основы теории управления",
                FacultyId = faculties.ElementAt(1).Id
            });

            DataBase.SubjectRepository.Add(new Subject()
            {
                SubjectName = "Маркетинг персонала",
                FacultyId = faculties.ElementAt(1).Id
            });

            DataBase.SubjectRepository.Add(new Subject()
            {
                SubjectName = "Управленческий учет и учет персонала",
                FacultyId = faculties.ElementAt(1).Id
            });

            DataBase.SubjectRepository.Add(new Subject()
            {
                SubjectName = "Основы управления персоналом",
                FacultyId = faculties.ElementAt(1).Id
            });

            DataBase.SubjectRepository.Add(new Subject()
            {
                SubjectName = "Основы организации труда",
                FacultyId = faculties.ElementAt(1).Id
            });

            DataBase.SubjectRepository.Add(new Subject()
            {
                SubjectName = "Экономика управления персоналом",
                FacultyId = faculties.ElementAt(1).Id
            });

            #endregion

            #region УПП

            DataBase.SubjectRepository.Add(new Subject()
            {
                SubjectName = "Общий курс транспорта",
                FacultyId = faculties.Last().Id
            });

            DataBase.SubjectRepository.Add(new Subject()
            {
                SubjectName = "Управление грузовой и коммерческой работой",
                FacultyId = faculties.Last().Id
            });

            DataBase.SubjectRepository.Add(new Subject()
            {
                SubjectName = "Транспортная безопасность",
                FacultyId = faculties.Last().Id
            });

            DataBase.SubjectRepository.Add(new Subject()
            {
                SubjectName = "Математическое моделирование систем и процессов",
                FacultyId = faculties.Last().Id
            });

            DataBase.SubjectRepository.Add(new Subject()
            {
                SubjectName = "Управление эксплуатационной работой",
                FacultyId = faculties.Last().Id
            });

            DataBase.SubjectRepository.Add(new Subject()
            {
                SubjectName = "Сервис на транспорте",
                FacultyId = faculties.Last().Id
            });

            #endregion

            #endregion

            #region Группы

            DataBase.GroupRepository.Add(new Group()
            {
                GroupName = "БИСТ-311",
                FacultyId = faculties.First().Id
            });

            DataBase.GroupRepository.Add(new Group()
            {
                GroupName = "БИСТ-312",
                FacultyId = faculties.First().Id
            });

            DataBase.GroupRepository.Add(new Group()
            {
                GroupName = "РСО-111",
                FacultyId = faculties.ElementAt(1).Id
            });

            DataBase.GroupRepository.Add(new Group()
            {
                GroupName = "Д-315",
                FacultyId = faculties.Last().Id
            });

            #endregion

            #region Деканы факультетов

            DataBase.DeanFacultyRepository.Add(new DeanFaculty()
            {
                DeanId = deans.First().Id,
                FacultyId = faculties.First().Id,
                StartManage = DateTime.Now
            });

            DataBase.DeanFacultyRepository.Add(new DeanFaculty()
            {
                DeanId = deans.ElementAt(1).Id,
                FacultyId = faculties.ElementAt(1).Id,
                StartManage = DateTime.Now
            });

            DataBase.DeanFacultyRepository.Add(new DeanFaculty()
            {
                DeanId = deans.Last().Id,
                FacultyId = faculties.Last().Id,
                StartManage = DateTime.Now
            });

            #endregion

            await DataBase.SaveAsync();

            #region Студенты учатся в группах

            count = 0;
            foreach (var item in DataBase.StudentRepository.GetAll)
            {
                if (count >= 0 && count < 5)
                {
                    DataBase.StudentGroupRepository.Add(new StudentGroup()
                    {
                        GroupId = DataBase.GroupRepository.FindFirst(g => g.GroupName.Contains("312")).Id,
                        StudentId = item.Id
                    });
                }

                if (count >= 5 && count < 10)
                {
                    DataBase.StudentGroupRepository.Add(new StudentGroup()
                    {
                        GroupId = DataBase.GroupRepository.FindFirst(g => g.GroupName.Contains("311")).Id,
                        StudentId = item.Id
                    });
                }

                if (count >= 10 && count < 15)
                {
                    DataBase.StudentGroupRepository.Add(new StudentGroup()
                    {
                        GroupId = DataBase.GroupRepository.FindFirst(g => g.GroupName.Contains("111")).Id,
                        StudentId = item.Id
                    });
                }

                if (count >= 15 && count < 20)
                {
                    DataBase.StudentGroupRepository.Add(new StudentGroup()
                    {
                        GroupId = DataBase.GroupRepository.FindFirst(g => g.GroupName.Contains("315")).Id,
                        StudentId = item.Id
                    });
                }
                count++;
            }

            #endregion

            var subjects = DataBase.SubjectRepository.GetAll.ToList();

            #region Преподаватели изучают предметы

            foreach (var t in teachers)
            {
                foreach (var s in subjects)
                {
                    if (t.ListTeacherFaculty.Select(tf => tf.FacultyId).Contains(s.FacultyId))
                    {
                        DataBase.TeacherSubjectRepository.Add(new TeacherSubject()
                        {
                            SubjectId = s.Id,
                            TeacherId = t.Id
                        });
                    }
                }
            }

            #endregion

            #endregion

            await DataBase.SaveAsync();
        }
    }
}