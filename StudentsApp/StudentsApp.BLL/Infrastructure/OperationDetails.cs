using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Infrastructure
{
    public class OperationDetails
    {
        public bool Succedeed { get; private set; }

        public string Message { get; private set; }

        public OperationDetails(bool succedeed = false, string message = "Изменения не были сохранены")
        {
            Succedeed = succedeed;
            Message = message;
        }
    }
}