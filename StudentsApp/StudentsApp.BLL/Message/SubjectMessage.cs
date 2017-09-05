using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Message
{
    public class SubjectMessage : Message
    {
        public override string KeyWord => "Дисциплина";

        protected override string Ending => "а";
    }
}