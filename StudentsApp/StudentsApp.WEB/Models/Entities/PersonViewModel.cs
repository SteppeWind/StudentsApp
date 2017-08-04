using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentsApp.WEB.Models.Entities
{
    public class PersonViewModel : BaseViewModel
    {
        [DisplayName("Имя")]
        [Required(ErrorMessage ="Укажите имя")]
        public string Name { get; set; }

        [DisplayName("Фамилия")]
        [Required(ErrorMessage = "Укажите фамилию")]
        public string Surname { get; set; }

        [DisplayName("Отчество")]
        [Required(ErrorMessage = "Укажите отчество")]
        public string MiddleName { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Пароль")]
        public string Password { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string Abbr
        {
            get => $"{Surname} {Name.First()}. {MiddleName.First()}.";
            private set { }
        }

        [HiddenInput(DisplayValue = false)]
        public string FullName
        {
            get => $"{Surname} {Name} {MiddleName}";
            private set { }
        }
    }
}