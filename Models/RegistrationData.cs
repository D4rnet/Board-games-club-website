using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Диплом.Models
{
    public class RegistrationData
    {
        [Key]
        [Required(ErrorMessage = "Укажите адрес электронной почты")]
        [EmailAddress(ErrorMessage = "Электронная почта указана не верно")]
        [DisplayName("Адрес электронной почты")]
        public string email { get; set; }

        [Required(ErrorMessage = "Укажите пароль")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Пароль должен быть больше 5 и меньше 20 символов")]
        [DisplayName("Пароль")]
        public string passwordOf { get; set; }

        [Required(ErrorMessage = "Подтвердите пароль")]
        [DisplayName("Подтверждение пароля")]
        [Compare("passwordOf", ErrorMessage = "Пароли не совпадают")]
        public string passwordСonfirm { get; set; }

        [Required(ErrorMessage = "Укажите имя")]
        [DisplayName("Имя")]
        public string firstname { get; set; }

        [Required(ErrorMessage = "Укажите фамилию")]
        [DisplayName("Фамилия")]
        public string lastname { get; set; }
    }
}
