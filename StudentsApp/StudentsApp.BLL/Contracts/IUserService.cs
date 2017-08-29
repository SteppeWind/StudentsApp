using StudentsApp.BLL.DTO;
using StudentsApp.BLL.Infrastructure;
using StudentsApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Contracts
{
    public interface IUserService
    {
        Task<ClaimsIdentity> Authenticate(PersonDTO person);
        Task SetRoles(List<string> roles);
        Task SetRoles(params string[] roles);
        Task<OperationDetails> UpdateProfile(PersonDTO person);
        Task<PersonDTO> Get(string id);
    }
}