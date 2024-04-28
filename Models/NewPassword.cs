using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Диплом.Models
{
    public class NewPassword
    {
        [Required(ErrorMessage = "Укажите текущий пароль")]
        [DisplayName("Текущий пароль")]
        public string currentPassword { get; set; }

        [Required(ErrorMessage = "Укажите новый пароль")]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "Пароль должен быть больше 5 и меньше 20 символов")]
        [DisplayName("Новый пароль")]
        public string passwordOf { get; set; }

        [Required(ErrorMessage = "Подтвердите пароль")]
        [DisplayName("Подтверждение пароля")]
        [Compare("passwordOf", ErrorMessage = "Пароли не совпадают")]
        public string passwordСonfirm { get; set; }
    }
}
