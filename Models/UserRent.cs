using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Диплом.Models
{
    public class UserRent
    {
        public GameRoom gameRoom { get; set; }

        public Rent rent { get; set; }

        public OpenRent openRent { get; set; }
    }
}
