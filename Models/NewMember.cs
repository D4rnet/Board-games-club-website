using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Диплом.Models
{
    public class NewMember
    {
        public IEnumerable<User> users { get; set; }
        public IEnumerable<OpenRent> openRents { get; set; }
        public Member member { get; set; }
    }
}
