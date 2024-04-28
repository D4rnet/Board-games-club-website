using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Диплом.Models
{
    public class FiltredGames
    {
        public IEnumerable<BoardGame> boardGameList { get; set; }

        public string gameName { get; set; }

        public int minPlayers { get; set; } = 1;

        public int maxPlayers { get; set; } = 100;

        public Dictionary<string, bool> gameDuration { get; set; } = new Dictionary<string, bool>
        {
            {"0-15", false},
            {"16-30", false},
            {"31-60", false},
            {"61-120", false},
            {"121-9999", false}
        };

        public Dictionary<string, bool> ageCategory { get; set; } = new Dictionary<string, bool>
        {
            {"3-5", false},
            {"6-7", false},
            {"8-12", false},
            {"13-15", false},
            {"16-17", false},
            {"18-100", false}
        };

        public Dictionary<string, bool> typeOf { get; set; } = new Dictionary<string, bool> { };

        public Dictionary<string, bool> manufacturer { get; set; } = new Dictionary<string, bool> { };
    }
}
