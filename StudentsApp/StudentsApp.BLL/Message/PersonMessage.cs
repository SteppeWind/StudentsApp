using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Message
{
    public abstract class PersonMessage : Message
    {
        public override string IsExist(string name)
        {
            return $"{KeyWord} с email {name} уже сущесвует";
        }

        public string NotFoundByEmail(string email) => $"{KeyWord} по {email} не найден{Ending}";
    }
}