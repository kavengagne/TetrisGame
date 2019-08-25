using GameClient.Classes.GameBoard.Pieces;
using Microsoft.Xna.Framework;

namespace GameClient.Classes.Core.Settings
{
    public static class Defaults
    {
        public static class Window
        {
            public static readonly string Name = "TetrisGame";
            public static readonly int Width = 800; // Do not access directly
            public static readonly int Height = 600; // Do not access directly
            public static readonly int MinimumWidth = 800;
            public static readonly int MinimumHeight = 600;
        }

        public static class Server
        {
            public static readonly string Address = "http://kavenserver.no-ip.org:88"; // Do not access directly

            public static class Query
            {
                public static readonly string Configs = "/configs";
                public static readonly string Games = "/games";
                public static readonly string Stats = "/stats";
                public static readonly string Errors = "/errors";
                public static readonly string Users = "/users";
            }
        }

        public static class Game
        {
            public static readonly Color BackgroundColor = Color.CornflowerBlue;
        }

        public static class Board
        {
            public static readonly int Rows = 22;
            public static readonly int Columns = 10;
            public static readonly Color BackgroundColor = Color.Black;
            public static readonly Rectangle BlockSize = new Rectangle(0, 0, 20, 20);
            public static readonly Rectangle PreviewBlockSize = new Rectangle(0, 0, 15, 15);
            public static readonly int Speed = 400;
        }

        public static readonly PieceInformation[] Pieces =
        {
            new PieceInformation
            {
                Name = "I",
                Positions = new[] { new Point(0, 0), new Point(-1, 0), new Point(1, 0), new Point(2, 0) },
                RotationsCount = 2,
                Color = Color.Cyan
            },
            new PieceInformation
            {
                Name = "J",
                Positions = new[] { new Point(0, 0), new Point(1, 0), new Point(-1, 0), new Point(-1, -1) },
                RotationsCount = 4,
                Color = Color.DeepSkyBlue
            },
            new PieceInformation
            {
                Name = "L",
                Positions = new[] { new Point(0, 0), new Point(-1, 0), new Point(1, 0), new Point(1, -1) },
                RotationsCount = 4,
                Color = Color.DarkOrange
            },
            new PieceInformation
            {
                Name = "O",
                Positions = new[] { new Point(0, 0), new Point(1, 0), new Point(0, 1), new Point(1, 1) },
                RotationsCount = 1,
                Color = Color.Yellow
            },
            new PieceInformation
            {
                Name = "S",
                Positions = new[] { new Point(0, 0), new Point(-1, 0), new Point(0, -1), new Point(1, -1) },
                RotationsCount = 2,
                Color = Color.LimeGreen
            },
            new PieceInformation
            {
                Name = "T",
                Positions = new[] { new Point(0, 0), new Point(-1, 0), new Point(1, 0), new Point(0, -1) },
                RotationsCount = 4,
                Color = Color.MediumPurple
            },
            new PieceInformation
            {
                Name = "Z",
                Positions = new[] { new Point(0, 0), new Point(1, 0), new Point(0, -1), new Point(-1, -1) },
                RotationsCount = 2,
                Color = Color.OrangeRed
            }
        };

        //"PiecesColors": [
        //    "255, 0, 0, 255",
        //    "173, 216, 230, 255",
        //    "255, 255, 0, 255",
        //    "255, 165, 0, 255",
        //    "144, 238, 144, 255",
        //    "255, 192, 203, 255",
        //    "238, 130, 238, 255"
        //]
    }
}
