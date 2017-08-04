using StudentsApp.DAL.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Contracts
{
    public interface IIndentityUnitOfWork
    {
        ApplicationUserManager UserManager { get; }

        ApplicationRoleManager RoleManager { get; }

        Task SaveAsync();

        void Save();
    }
}