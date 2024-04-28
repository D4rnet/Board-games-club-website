using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Диплом.Models
{
    public class User
    {
        [Key]
        [Required(ErrorMessage = "Укажите адрес электронной почты")]
        [MaxLength(30, ErrorMessage = "Электронная почта не может содержать больше 30 символов")]
        [EmailAddress(ErrorMessage = "Электронная почта указана не верно")]
        [DisplayName("Адрес электронной почты")]
        public string email { get; set; }

        [Required(ErrorMessage = "Укажите пароль")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Пароль должен быть больше 5 и меньше 21 символов")]
        [DisplayName("Пароль")]
        public string passwordOf { get; set; }

        [Required(ErrorMessage = "Укажите имя")]
        [MaxLength(20, ErrorMessage = "Имя не может содержать больше 20 символов")]
        [DisplayName("Имя")]
        public string firstname { get; set; }

        [Required(ErrorMessage = "Укажите фамилию")]
        [MaxLength(20, ErrorMessage = "Фамилия не может содержать больше 20 символов")]
        [DisplayName("Фамилия")]
        public string lastname { get; set; }

        [Required(ErrorMessage = "Выберите роль")]
        [MaxLength(10, ErrorMessage = "Роль не может содержать больше 10 символов")]
        [DisplayName("Роль")]
        public string roleOf { get; set; }
    }
}
