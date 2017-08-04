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
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(PersonDTO person);
        Task<ClaimsIdentity> Authenticate(PersonDTO person);
        Task SetInitialData(PersonDTO adminDto, List<string> roles);
    }
}