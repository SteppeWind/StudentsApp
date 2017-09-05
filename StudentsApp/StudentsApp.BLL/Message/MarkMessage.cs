using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentsApp.BLL.Message
{
    public class MarkMessage : Message
    {
        public override string KeyWord => "Оценка";

        protected override string Ending => "а";

        public string Create() => "Оценка успешно создана";

        public string FullRemove(string studentName, string subjectName) => $"Оценка для студента \"{studentName}\" по дисциплине \"{subjectName}\" полностью удалена из базы";

        public string Remove(string studentName, string subjectName) => $"Оценка для студента \"{studentName}\" по дисциплине \"{subjectName}\" помечена как \"Удален\"";

        public string Update(string studentName, string subjectName) => $"Для студента \"{studentName}\" по дисциплине \"{subjectName}\" оценка успешно обновлена";
    }
}