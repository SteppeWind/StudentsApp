using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Message
{
    public abstract class Message
    {
        public abstract string KeyWord { get; }

        protected virtual string Ending { get; } = string.Empty;

        public virtual string NotFound(string name = "") => $"{KeyWord} \"{name}\" не найден{Ending}";

        public virtual string IsExist(string name) => $"{KeyWord} с наименованием \"{name}\" уже существует";

        public virtual string Create(string name) => $"{KeyWord} \"{name}\" успешно создан{Ending}";

        public virtual string FullRemove(string name) => $"{KeyWord} \"{name}\" полностью удален{Ending} из базы";

        public virtual string Remove(string name) => $"{KeyWord} \"{name}\" помечен как \"Удален\"";

        public virtual string Update(string name = "") => $"{KeyWord} успешно обновлен{Ending}";

        public virtual string Error => "Информация не была сохранена";        
    }
}