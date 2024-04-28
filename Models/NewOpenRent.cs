using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Диплом.Models
{
    public class NewOpenRent
    {
        public IEnumerable<Rent> rents { get; set; }

        public OpenRent openRent { get; set; }
    }
}
