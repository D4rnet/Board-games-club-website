using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Диплом.Models
{
    public class NewRent
    {
        public IEnumerable<User> users { get; set; }

        public IEnumerable<GameRoom> gameRooms { get; set; }

        public Rent rent { get; set; }
    }
}
