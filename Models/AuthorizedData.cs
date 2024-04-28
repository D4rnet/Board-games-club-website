using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Диплом.Models
{
    public class AuthorizedData
    {
        [DisplayName("Адрес электронной почты")]
        public string email { get; set; }

        [DisplayName("Пароль")]
        public string passwordOf { get; set; }
    }
}
