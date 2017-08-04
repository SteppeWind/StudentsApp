using Microsoft.AspNet.Identity.EntityFramework;
using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.EF
{
    public class IdentityContext : IdentityDbContext<ApplicationUser>
    {
        static IdentityContext()
        {
            context = new IdentityContext();
            //Database.SetInitializer(new AppDbInitializer());
        }

        private static IdentityContext context;
        public static IdentityContext IdentityUserContext => context;

        private IdentityContext() : base("IdentityDB") { }
    }

    public class AppDbInitializer : DropCreateDatabaseAlways<IdentityContext>
    {
        protected override void Seed(IdentityContext context)
        {
            ApplicationRole studentRole = new ApplicationRole() { Name = "student" };
            ApplicationRole teacherRole = new ApplicationRole() { Name = "teacher" };
            ApplicationRole deanRole = new ApplicationRole() { Name = "dean" };
            ApplicationRole adminRole = new ApplicationRole() { Name = "admin" };

            context.Roles.Add(studentRole);
            context.Roles.Add(teacherRole);
            context.Roles.Add(deanRole);
            context.Roles.Add(adminRole);

            var persons = StudentsAppContext.StudentsContext.Profiles;

            foreach (var item in persons)
            {
                context.Users.Add(new ApplicationUser() { Email = item.Email, PasswordHash = "qwe123" });
            }

            //context.Users.Add(new ApplicationUser() { Email = "fbi@stu.ru", PasswordHash = "qwe123" });
            //context.Users.Add(new ApplicationUser() { Email = "up@stu.ru", PasswordHash = "qwe123" });
            //context.Users.Add(new ApplicationUser() { Email = "upp@stu.ru", PasswordHash = "qwe123" });

            //context.Users.Add(new ApplicationUser() { Email = "TarasovEB@mail.ru", PasswordHash = "qwe123" });
            //context.Users.Add(new ApplicationUser() { Email = "ZhikovMV@yandex.ru", PasswordHash = "qwe123" });
            //context.Users.Add(new ApplicationUser() { Email = "KobilynskiVG@inbox.ru", PasswordHash = "qwe123" });
            //context.Users.Add(new ApplicationUser() { Email = "NartovaMari@mail.ru", PasswordHash = "qwe123" });
            //context.Users.Add(new ApplicationUser() { Email = "evgen69@yandex.ru", PasswordHash = "qwe123" });
            //context.Users.Add(new ApplicationUser() { Email = "TynykovaEV@mail.com", PasswordHash = "qwe123" });
            //context.Users.Add(new ApplicationUser() { Email = "Lomanova@mail.ru", PasswordHash = "qwe123" });
            //context.Users.Add(new ApplicationUser() { Email = "LesovichenkoAM@mail.ru", PasswordHash = "qwe123" });
            //context.Users.Add(new ApplicationUser() { Email = "Shadt@yandex.ru", PasswordHash = "qwe123" });
            //context.Users.Add(new ApplicationUser() { Email = "CashnikOI@yandex.ru", PasswordHash = "qwe123" });
            //context.Users.Add(new ApplicationUser() { Email = "LukynenkoNV@ymail.ru", PasswordHash = "qwe123" });
            //context.Users.Add(new ApplicationUser() { Email = "Chernenko@mail.ru", PasswordHash = "qwe123" });
            //context.Users.Add(new ApplicationUser() { Email = "PserovkayED@mail.ru", PasswordHash = "qwe123" });
            //context.Users.Add(new ApplicationUser() { Email = "NaterovVV@mail.ru", PasswordHash = "qwe123" });
            //context.Users.Add(new ApplicationUser() { Email = "Osipov@yandex.ru", PasswordHash = "qwe123" });
            //context.Users.Add(new ApplicationUser() { Email = "Yrgina94@mail.ru", PasswordHash = "qwe123" });
            //context.Users.Add(new ApplicationUser() { Email = "Grishko@inbox.ru", PasswordHash = "qwe123" });

            //context.Users.Add(new ApplicationUser() { Email = "Grishko@inbox.ru", PasswordHash = "qwe123" });


            base.Seed(context);
        }
    }
}