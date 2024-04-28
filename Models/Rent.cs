using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Диплом.Models
{
    public class Rent
    {
        [Key]
        [DisplayName("Номер")]
        public int id { get; set; }

        [Required(ErrorMessage = "Укажите адрес электронной почты организатора")]
        [DisplayName("Адрес электронной почты организатора")]
        public string orgnizerEmail { get; set; }

        [Required(ErrorMessage = "Укажите номер комнаты")]
        [DisplayName("Номер комнаты")]
        public int roomNumber { get; set; }

        [Required(ErrorMessage = "Укажите дату и время брони")]
        [DisplayName("Дата и время брони")]
        public DateTime? timeOf { get; set; }

        [Required(ErrorMessage = "Укажите продолжительность брони")]
        [Range(1, int.MaxValue, ErrorMessage = "Продолжительность брони может быть только положительным числом")]
        [DisplayName("Продолжительность брони(в часах)")]
        public int? duration { get; set; }

        [DisplayName("Цена")]
        public int price { get; set; }
    }
}
