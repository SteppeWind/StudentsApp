using StudentsApp.BLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Contracts
{
    public interface IHistoryFacultyService : IBaseService<DeanFacultyDTO>
    {
        IEnumerable<DeanFacultyDTO> GetHistory(string idFaculty);
    }
}