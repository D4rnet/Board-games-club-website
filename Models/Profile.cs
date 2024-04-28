using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Диплом.Models
{
    public class Profile
    {
        [DisplayName("Имя")]
        public string firstname { get; set; }

        [DisplayName("Фамилия")]
        public string lastname { get; set; }

        public List<UserRent> rents { get; set; }

        public List<OpenRentInfo> joinRents { get; set; }
    }
}
