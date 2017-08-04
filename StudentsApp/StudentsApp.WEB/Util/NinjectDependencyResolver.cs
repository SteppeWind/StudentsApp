using Ninject;
using StudentsApp.BLL.Contracts;
using StudentsApp.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentsApp.WEB.Util
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }


        private void AddBindings()
        {
            kernel.Bind<IDeanService>().To<DeanService>();
            kernel.Bind<IFacultyService>().To<FacultyService>();
            kernel.Bind<IGroupService>().To<GroupService>();
            kernel.Bind<IStudentService>().To<StudentService>();
            kernel.Bind<ISubjectService>().To<SubjectService>();
            kernel.Bind<ITeacherService>().To<TeacherService>();
            kernel.Bind<IMarkService>().To<MarkService>();
            kernel.Bind<IHistoryFacultyService>().To<HistoryFacultyService>();
            kernel.Bind<ITeacherPostService>().To<TeacherPostService>();
            kernel.Bind<IStartService>().To<StartService>();
            kernel.Bind<IUserService>().To<UserService>();
        }
    }
}