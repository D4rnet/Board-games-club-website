using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Диплом.Models
{
    public class JoinToRent
    {
        public OpenRentInfo openRentInfo { get; set; }

        [Required(ErrorMessage = "Укажите сколько вас будет")]
        [Range(1, int.MaxValue, ErrorMessage = "Количество человек может быть только положительным числом")]
        [DisplayName("Количество человек")]
        public int? peopleCount { get; set; }
    }
}
