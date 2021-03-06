﻿using System;

namespace GameModel.Models
{
    public class Game
    {
        public int GameID { get; set; }
        public int UserID { get; set; }

        public DateTime PlayedDate { get; set; }
        public TimeSpan Duration { get; set; }

        public int Score { get; set; }
        public int Level { get; set; }
        public int Lines { get; set; }
        public int Doubles { get; set; }
        public int Tetris { get; set; }
        public int BackToBack { get; set; }
        public int Tspin { get; set; }

        public virtual User User { get; set; }
    }
}
