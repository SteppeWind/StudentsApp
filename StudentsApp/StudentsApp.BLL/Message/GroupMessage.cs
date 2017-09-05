using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Message
{
    public class GroupMessage : Message
    {
        public override string KeyWord => "Группа";

        protected override string Ending => "а";
    }
}