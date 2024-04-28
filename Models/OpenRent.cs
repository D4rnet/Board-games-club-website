using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Диплом.Models
{
    public class OpenRent
    {
        [Key]
        [DisplayName("Номер")]
        public int id { get; set; }

        [Required(ErrorMessage = "Укажите номер брони")]
        [DisplayName("Номер брони")]
        public int rentId { get; set; }

        [Required(ErrorMessage = "Укажите сколько вас будет")]
        [Range(0, int.MaxValue, ErrorMessage = "Количество человек не может быть отрицательным числом")]
        [DisplayName("Количество человек")]
        public int? peopleCount { get; set; }

        [MaxLength(1000, ErrorMessage = "Описание не может содержать больше 1000 символов")]
        [DisplayName("Описание")]
        public string descriptionOf { get; set; }
    }
}
