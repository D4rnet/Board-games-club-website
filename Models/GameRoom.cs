using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Диплом.Models
{
    public class GameRoom
    {
        [Key]
        [Required(ErrorMessage = "Укажите номер")]
        [Range(1, int.MaxValue, ErrorMessage = "Номер может быть только положительным числом")]
        [DisplayName("Номер")]
        public int? number { get; set; }

        [Required(ErrorMessage = "Укажите стоимость брони")]
        [Range(1, int.MaxValue, ErrorMessage = "Стоимость брони может быть только положительным числом")]
        [DisplayName("Стоимость брони")]
        public int? rentPrice { get; set; }

        [Required(ErrorMessage = "Укажите количество мест")]
        [Range(3, 20, ErrorMessage = "Количество мест должно быть в диапазоне от 3 до 20")]
        [DisplayName("Количество мест")]
        public int? places { get; set; }

        [Required(ErrorMessage = "Укажите ссылку на изображение")]
        [MaxLength(50, ErrorMessage = "Ссылка на изображение не может содержать больше 50 символов")]
        [DisplayName("Изображение")]
        public string imageOf { get; set; }
    }
}
