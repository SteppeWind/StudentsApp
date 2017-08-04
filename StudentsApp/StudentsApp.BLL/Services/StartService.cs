using StudentsApp.BLL.Contracts;
using StudentsApp.DAL.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Services
{
    public class StartService : IStartService
    {
        private IUnitOfWork UnitOfWork;
        private IIndentityUnitOfWork IdentityUnitOfWork;

        public StartService(IUnitOfWork uow, IIndentityUnitOfWork iuow)
        {
            UnitOfWork = uow;
            IdentityUnitOfWork = iuow;
        }

        public void FillData()
        {
            UnitOfWork.StartRepository.FillDataDB();
        }
    }
}