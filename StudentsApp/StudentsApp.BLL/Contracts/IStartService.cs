using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Contracts
{
    public interface IStartService
    {
        bool IsExistDB { get; }
        void ClearData();
        Task FillDataAsync();
    }
}