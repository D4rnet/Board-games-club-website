using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Диплом.Models
{
    public class OpenRentInfo
    {
        public int id { get; set; }

        public GameRoom gameRoom { get; set; }

        public int occupiedPlaces { get; set; }

        public DateTime? timeOf { get; set; }

        public int? duration { get; set; }

        public string descriptionOf { get; set; }
    }
}
