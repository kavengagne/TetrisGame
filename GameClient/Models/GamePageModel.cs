using GameClient.Classes.Core;

namespace GameClient.Models
{
    class GamePageModel
    {
        public GamePageModel()
        {
            Game = new TetrisGame();
        }

        public TetrisGame Game { get; set; }

        /*public Vector3 Scale
        {
            get
            {
                return ((ITeapotService)Game.Services.GetService(typeof(ITeapotService))).Scale;
            }
            set
            {
                ((ITeapotService)Game.Services.GetService(typeof(ITeapotService))).Scale = value;
            }
        }*/
    }
}
