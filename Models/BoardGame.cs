using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Диплом.Models
{
    public class BoardGame
    {
        [Key]
        [Required(ErrorMessage = "Укажите название")]
        [MaxLength(50, ErrorMessage = "Название не может содержать больше 50 символов")]
        [DisplayName("Название")]
        public string nameOf { get; set; }

        [Required(ErrorMessage = "Укажите минимальное количество игроков")]
        [Range(1, int.MaxValue, ErrorMessage = "Минимальное количество игроков может быть только положительным числом")]
        [DisplayName("Минимальное количество игроков")]
        public int? minPlayers { get; set; }

        [Required(ErrorMessage = "Укажите максимальное количество игроков")]
        [Range(1, int.MaxValue, ErrorMessage = "Максимальное количество игроков может быть только положительным числом")]
        [DisplayName("Максимальное количество игроков")]
        public int? maxPlayers { get; set; }

        [Required(ErrorMessage = "Укажите среднюю продолжительность(в минутах)")]
        [Range(1, int.MaxValue, ErrorMessage = "Средняя продолжительность может быть только положительным числом")]
        [DisplayName("Средняя продолжительность")]
        public int? avgDuration { get; set; }

        [Required(ErrorMessage = "Укажите возрастную категорию")]
        [Range(0, int.MaxValue, ErrorMessage = "Возрастная категория не может быть отрицательным числом")]
        [DisplayName("Возрастная категория")]
        public int? ageCategory { get; set; }

        [Required(ErrorMessage = "Укажите тип")]
        [MaxLength(20, ErrorMessage = "Тип не может содержать больше 20 символов")]
        [DisplayName("Тип")]
        public string typeOf { get; set; }

        [Required(ErrorMessage = "Укажите производителя")]
        [MaxLength(30, ErrorMessage = "Производитель не может содержать больше 30 символов")]
        [DisplayName("Производитель")]
        public string manufacturer { get; set; }

        [Required(ErrorMessage = "Укажите описание")]
        [MaxLength(1000, ErrorMessage = "Описание не может содержать больше 1000 символов")]
        [DisplayName("Описание")]
        public string descriptionOf { get; set; }

        [Required(ErrorMessage = "Укажите ссылку на изображение")]
        [MaxLength(200, ErrorMessage = "Ссылка на изображение не может содержать больше 200 символов")]
        [DisplayName("Изображение")]
        public string imageOf { get; set; }

        [Required(ErrorMessage = "Укажите стоимость брони")]
        [Range(1, int.MaxValue, ErrorMessage = "Стоимость брони может быть только положительным числом")]
        [DisplayName("Стоимость брони")]
        public int? rentPrice { get; set; }
    }
}
