using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Диплом.Models
{
    public class Member
    {
        [Key]
        [DisplayName("Номер")]
        public int id { get; set; }

        [Required(ErrorMessage = "Укажите номер брони")]
        [DisplayName("Номер брони")]
        public int rentId { get; set; }

        [Required(ErrorMessage = "Укажите адрес электронной почты")]
        [DisplayName("Адрес электронной почты")]
        public string userEmail { get; set; }

        [Required(ErrorMessage = "Укажите сколько вас будет")]
        [Range(1, int.MaxValue, ErrorMessage = "Количество человек  может быть только положительным числом")]
        [DisplayName("Количество человек")]
        public int? peopleCount { get; set; }
    }
}
