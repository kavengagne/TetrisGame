using System;
using Microsoft.Xna.Framework;

namespace GameClient.Classes.GameBoard.Pieces
{
    public struct PieceInformation
    {
        public String Name { get; set; }
        public Point[] Positions { get; set; }
        public int RotationsCount { get; set; }
        public Color Color { get; set; }
    }
}