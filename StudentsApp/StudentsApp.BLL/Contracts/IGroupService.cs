using StudentsApp.BLL.DTO;
using StudentsApp.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Contracts
{
    public interface IGroupService : IBaseService<GroupDTO>
    {
        OperationDetails AddStudentToGroupById(string idGroup, string idStudent);
        OperationDetails AddStudentToGroupByEmail(string idGroup, string email);
        OperationDetails RemoveStudentFromGroupById(string idGroup, string idStudent, bool isFullRemove = false);

        IEnumerable<GroupDTO> GetGroupsInFaculty(string idFaculty);
        IEnumerable<GroupDTO> GetStudentGroups(string idStudent);
    }
}