using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.DAL.Contracts
{
    public interface IStartRepository
    {
        bool IsExistDB { get; }
        void ClearDB();
        void FillDataDB();
    }
}