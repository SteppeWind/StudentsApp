using Ninject.Modules;
using StudentsApp.DAL.Contracts;
using StudentsApp.DAL.EF;
using StudentsApp.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private StudentsAppContext dbContext;

        public ServiceModule()
        {
            dbContext = StudentsAppContext.StudentsContext;
        }

        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>();
        }
    }
}