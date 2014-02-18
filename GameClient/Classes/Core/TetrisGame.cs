using GameClient.Classes.Core.Inputs;
using GameClient.Classes.Core.Managers;
using GameClient.Classes.Core.Randomizer;
using GameClient.Classes.Core.Settings;
using GameClient.Classes.GameBoard;
using GameClient.Classes.ParticleSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Point = Microsoft.Xna.Framework.Point;


// TODO: KG - URGENT: SEPARATE GHOST PIECE FROM PIECE.
// TODO: KG - Adhere to Tetris Guidelines: http://harddrop.com/wiki/Tetris_Guideline
// TODO: KG - Bug: Fix Fullscreen Resolution.
// TODO: KG - Bug: Fix Objects Life Cycle Issues.
// TODO: KG - Bug: Correct Scaling Calculation Code.
// TODO: KG - Feature: Finish Game Reset. (Add Confirmation)
// TODO: KG - Feature: Finish Game Quit. (Add Confirmation)
// TODO: KG - Feature: Integrate WPF Components inside XNA. (For Menus and Configurations / Statistics Widgets)
// TODO: KG - Bug: Change Hold Behavior. (Should stay in Hold slot until used) (Should Reset Piece Position to 0) (Create HoldPanel)
// TODO: KG - Bug: Change Board Background Color or Image.
// TODO: KG - Bug: Make Pieces Colors Consistents.
// TODO: KG - Bug: Correct Pieces Starting Position.
// TODO: KG - Tuning: Make sure RandomBag is implemented as detailed in: http://harddrop.com/wiki/Random_Generator
// TODO: KG - Feature: Game Main Screen.
// TODO: KG - Feature: Share Game with Friends.
// TODO: KG - Feature: Server Statistics Logging.
// TODO: KG - Feature: Client-Side Scoreboard.
// TODO: KG - Feature: Add Game Time.
// TODO: KG - Feature: Game Options.
// TODO: KG - Feature: InputManager Key Settings Handling. (Keyboard, Mouse, Xbox Controller)
// TODO: KG - Feature: Show More Next Pieces. (Maybe 2 or 3)
// TODO: KG - Feature: Add small delay after moving piece. This will allow players to place the piece before it locks.
// TODO: KG - Tuning: Redesign the GameBoard.
// TODO: KG - Tuning: Improve Ghost Piece look.
// TODO: KG - Tuning: Add more Error Handling.
// TODO: KG - Tuning: Optimize Score. http://en.wikipedia.org/wiki/Tetris
// TODO: KG - Tuning: Implement T-Spin Bonus. http://harddrop.com/wiki/T-Spin
// TODO: KG - Bug: Add OpenAL to Release Bundle.
// TODO: KG - Bug: Fix ScoreBoard Font Display.
// TODO: KG - Feature: Auto Update.
// TODO: KG - Feature: Create Installer Project.
// TODO: KG - Feature: Add Help Feature. (Default Key: H)
// TODO: KG - Feature: Game Levels. Levels increase Game Speed. Level-up after N completed lines.
// TODO: KG - Feature: Change Game Theme when leveling.
// TODO: KG - Feature: Add Pause Menu. (Default Key: Escape)
// TODO: KG - Feature: Game Over Handling.
// TODO: KG - Feature: Add Musics. (Should create those myself)
// TODO: KG - Feature: Make Sounds much stronger when performing a Tetris.

namespace GameClient.Classes.Core
{
    public class TetrisGame : Game
    {
        #region Singleton Pattern
        private static TetrisGame Instance { get; set; }

        public static TetrisGame GetInstance()
        {
            return Instance ?? (Instance = new TetrisGame());
        }
        #endregion


        #region Fields
        private readonly GraphicsDeviceManager _graphics;
        #endregion


        #region Properties
        public Color BackgroundColor { get; set; }
        public bool IsRunning { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public ParticleEngine ParticleEngine { get; set; }
        public Board Board { get; set; }
        public InputManager InputManager { get; set; }
        public Matrix SpriteScale { get; set; }
        #endregion


        #region Constructors
        private TetrisGame()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Configuration.GetInstance().WindowWidth,
                PreferredBackBufferHeight = Configuration.GetInstance().WindowHeight,
                PreferMultiSampling = true,
                SynchronizeWithVerticalRetrace = true
            };
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
            BackgroundColor = Defaults.Game.BackgroundColor;
        }
        #endregion


        #region Public Methods
        public void ChangeGameBackgroundColor(Score score, int updateValue)
        {
            // TODO: KG - Move to configuration.
            const int divider = 10;
            int oldValue = score.Lines - updateValue;
            int oldTenth = oldValue / divider;
            int newTenth = score.Lines / divider;
            if (newTenth > oldTenth)
            {
                BackgroundColor = new Color(StaticRandom.Next(256), StaticRandom.Next(256), StaticRandom.Next(256));
            }
        }
        #endregion


        #region Methods Overrides
        protected override void Initialize()
        {
            Window.Title = Defaults.Window.Name;
            InputManager = new InputManager();
            Board = new Board(this, new Point(40, 40));
            
            RegisterUserInputs();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            SpriteScale = GetSpriteScale(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            SoundManager.GetInstance();
            TextureManager.GetInstance();
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            SpriteScale = GetSpriteScale(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            //Console.WriteLine("w:{0}, h:{1}", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            InputManager.HandleInputs(gameTime);
            Board.Update(gameTime);
            //ParticleEngine.EmitterLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            //ParticleEngine.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackgroundColor);
            SpriteBatch.Begin(0, BlendState.AlphaBlend, null, null, null, null, SpriteScale);
            Board.Draw(SpriteBatch, gameTime);
            SpriteBatch.End();
            //ParticleEngine.Draw(_spriteBatch, gameTime);
            base.Draw(gameTime);
        }
        #endregion


        #region Internal Implementation
        private void RegisterUserInputs()
        {
            InputManager.RegisterKeyPressed(Keys.P, TogglePause);
            InputManager.RegisterKeyPressed(Keys.LeftShift, Board.ExchangePiece);
            InputManager.RegisterKeyPressed(Keys.Space, Board.DropPieceAllTheWay);
            InputManager.RegisterKeyPressed(Keys.Up, Board.RotateLeft);
            InputManager.RegisterKeyPressed(Keys.LeftControl, Board.RotateRight);
            InputManager.RegisterKeyPressed(Keys.Down, Board.DropPieceByOne, true, 100);
            // TODO: KG - Adjust left/right speed
            InputManager.RegisterKeyPressed(Keys.Left, Board.MoveLeft, true, 120);
            InputManager.RegisterKeyPressed(Keys.Right, Board.MoveRight, true, 120);
            // TODO: KG - Restart Game (With Confirmation)
            // TODO: KG - Quit Game (With Confirmation)
            InputManager.RegisterKeyPressed(Keys.Escape, Application.Exit);
            InputManager.RegisterKeyPressed(Keys.R, RestartGame);
            InputManager.RegisterKeyPressed(Keys.F, _graphics.ToggleFullScreen);
        }

        private void RestartGame()
        {
            Board.Reset();
        }

        private void TogglePause()
        {
            IsRunning = !IsRunning;
            SoundManager.GetInstance().Play("Pause");
            //TogglePauseMenu();
        }

        private Matrix GetSpriteScale(int width, int height)
        {
            float xScale = (float)width / Configuration.GetInstance().WindowWidth;
            float yScale = (float)height / Configuration.GetInstance().WindowHeight;
            float chosenScale = (width > height) ? yScale : xScale;
            return Matrix.CreateScale(chosenScale, chosenScale, 1);
        }
        #endregion
    }
}
