using StudentsApp.BLL.DTO;
using StudentsApp.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Contracts
{
    public interface IPersonService<TPerson> : IBaseService<TPerson> where TPerson : PersonDTO
    {
        Task<IEnumerable<TPerson>> GetByNameAsync(string name);
        Task<IEnumerable<TPerson>> GetBySurnameAsync(string surname);
        Task<IEnumerable<TPerson>> GetByMiddleNameAsync(string middleName);
        Task<TPerson> GetByEmailAsync(string email);
    }
}