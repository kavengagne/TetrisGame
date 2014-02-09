using GameClient.Classes.Core.Inputs;
using GameClient.Classes.GameBoard;
using GameClient.Classes.ParticleSystem;
using GameConfiguration.DataObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Point = Microsoft.Xna.Framework.Point;


// TODO: KG - Adhere to Tetris Guidelines: http://harddrop.com/wiki/Tetris_Guideline
// TODO: KG - Bug: Change Hold Behavior. (Should stay in Hold slot until used) (Create HoldPanel)
// TODO: KG - Bug: Change Board Background Color or Image.
// TODO: KG - Bug: Make Pieces Colors Consistents.
// TODO: KG - Bug: Correct Pieces Starting Position.
// TODO: KG - Feature: Game Main Screen.
// TODO: KG - Feature: Server Statistics Logging.
// TODO: KG - Feature: Client-Side Scoreboard.
// TODO: KG - Feature: Add Game Time.
// TODO: KG - Feature: Game Options.
// TODO: KG - Feature: InputManager Key Settings Handling. (Keyboard, Mouse, Xbox Controller)
// TODO: KG - Feature: Show More Next Pieces. (Maybe 2 or 3)
// TODO: KG - Feature: Add small delay after moving piece. This will allow players to place the piece before it locks.
// TODO: KG - Tuning: Redesing the GameBoard. (Handle Panel Positioning and Borders)
// TODO: KG - Tuning: Improve Ghost Piece look.
// TODO: KG - Tuning: Add more Error Handling.
// TODO: KG - Tuning: Optimize Score. http://en.wikipedia.org/wiki/Tetris
// TODO: KG - Tuning: Implement T-Spin Bonus. http://harddrop.com/wiki/T-Spin
// TODO: KG - Tuning: Make sure RandomBag is implemented as detailed in: http://harddrop.com/wiki/Random_Generator
// TODO: KG - Bug: Add OpenAL to Release Bundle.
// TODO: KG - Bug: Fix ScoreBoard Font Display.
// TODO: KG - Feature: Auto Update.
// TODO: KG - Feature: Create Installer Project.
// TODO: KG - Feature: Add Help Feature. (Default Key: H)
// TODO: KG - Feature: Add FullScreen Support. (Using Scaling)
// TODO: KG - Feature: Game Levels. Levels increase Game Speed. Level-up after N completed lines.
// TODO: KG - Feature: Change Game Theme when leveling.
// TODO: KG - Feature: Game Reset.
// TODO: KG - Feature: Add Pause Menu. (Default Key: Escape)
// TODO: KG - Feature: Game Over Handling.
// TODO: KG - Feature: Add Musics. (Should create those myself)
// TODO: KG - Feature: Make Sounds much stronger when performing a Tetris.

namespace GameClient.Classes.Core
{
    public class TetrisGame : Game
    {
        #region Fields
        private readonly App _application;
        private GraphicsDeviceManager _graphics;
        #endregion


        #region Properties
        public bool IsRunning { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        public ParticleEngine ParticleEngine { get; set; }
        public PieceGenerator PieceGenerator { get; set; }
        public Board Board { get; set; }
        public PreviewPanel PreviewPanel { get; set; }
        public ScoreBoard ScoreBoard { get; set; }
        public SoundManager SoundManager { get; set; }
        public InputManager InputManager { get; set; }
        public bool CanExchangePiece { get; set; }
        #endregion


        #region Constructors
        public TetrisGame()
        {
            _application = App.Instance;
            _application.Game = this;
            _graphics = new GraphicsDeviceManager(this)
            {
                // TODO: KG - Find how to make this useful.
                //PreferredBackBufferWidth = _application.Client.WindowWidth,
                //PreferredBackBufferHeight = _application.Client.WindowHeight
            };
            Content.RootDirectory = "Content";
        }
        #endregion


        #region Methods Overrides
        protected override void Initialize()
        {
            Window.Title = _application.Client.WindowName;

            InitializeSoundManager();
            InitializeInputManager();
            //InitializeTextureManager();

            InitializePieceGenerator(_application.Configuration.Pieces,
                                     _application.Configuration.PiecesColors);

            InitializeTetrisBoard();
            InitializePreviewPanel();
            InitializeScoreBoard();

            RegisterUserInputs();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            
            //var textures = new List<Texture2D>
            //{
            //    Content.Load<Texture2D>("Graphics/circle"),
            //    Content.Load<Texture2D>("Graphics/star"),
            //    Content.Load<Texture2D>("Graphics/diamond")
            //};
            //ParticleEngine = new ParticleEngine(textures, new Vector2(400, 240));
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.HandleInputs(gameTime);
            Board.Update(gameTime);
            PreviewPanel.Update(gameTime);
            ScoreBoard.Update(gameTime);
            //ParticleEngine.EmitterLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            //ParticleEngine.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_application.Configuration.Game.BackgroundColor);
            SpriteBatch.Begin();
            Board.Draw(SpriteBatch, gameTime);
            PreviewPanel.Draw(SpriteBatch, gameTime);
            ScoreBoard.Draw(SpriteBatch, gameTime);
            SpriteBatch.End();
            //ParticleEngine.Draw(_spriteBatch, gameTime);
            base.Draw(gameTime);
        }
        #endregion


        #region Public Methods
        public Piece GetNextPiece()
        {
            return PieceGenerator.GetPiece();
        }

        public PreviewPiece PeekNextPiece()
        {
            return PieceGenerator.PeekNextPiece();
        }
        #endregion


        #region Internal Implementation
        private void InitializeSoundManager()
        {
            SoundManager = new SoundManager(Content);
        }

        private void InitializeInputManager()
        {
            InputManager = new InputManager();
        }

        private void InitializePieceGenerator(PieceInformation[] pieces, Color[] colors)
        {
            PieceGenerator = new PieceGenerator(this, pieces, colors);
        }

        private void InitializeTetrisBoard()
        {
            // TODO: KG - Move to config or something
            Board = new Board(this, new Point(40, 40));
        }

        private void InitializePreviewPanel()
        {
            // TODO: KG - Move to config or something
            var bounds = new Rectangle(Board.Bounds.X + Board.Bounds.Width + 5, 40, 100, 100);
            PreviewPanel = new PreviewPanel(this, bounds, _application.Configuration.Board.BackgroundColor);
        }

        private void InitializeScoreBoard()
        {
            // TODO: KG - Move to config or something
            var bounds = new Rectangle(Board.Bounds.X + Board.Bounds.Width + 5, 40 + 100 + 5, 100, 110);
            ScoreBoard = new ScoreBoard(this, bounds, _application.Configuration.Board.BackgroundColor);
        }

        private void RegisterUserInputs()
        {
            InputManager.RegisterKeyPressed(Keys.P, TogglePause);
            InputManager.RegisterKeyPressed(Keys.LeftShift, ExchangePiece);
            InputManager.RegisterKeyPressed(Keys.Space, Board.DropPieceAllTheWay);
            InputManager.RegisterKeyPressed(Keys.Up, Board.RotateLeft);
            InputManager.RegisterKeyPressed(Keys.LeftControl, Board.RotateRight);
            InputManager.RegisterKeyPressed(Keys.Down, Board.DropPieceByOne, true, 100);
            // TODO: KG - Adjust left/right speed
            InputManager.RegisterKeyPressed(Keys.Left, Board.MoveLeft, true, 100);
            InputManager.RegisterKeyPressed(Keys.Right, Board.MoveRight, true, 100);
            // TODO: KG - Restart Game (With Confirmation)
            // TODO: KG - Quit Game (With Confirmation)
            InputManager.RegisterKeyPressed(Keys.Escape, App.Exit);
            InputManager.RegisterKeyPressed(Keys.R, RestartGame);
        }

        private void RestartGame()
        {
            Board.Reset();
        }

        private void TogglePause()
        {
            _application.Game.IsRunning = !_application.Game.IsRunning;
            SoundManager.Play("Pause");
            //TogglePauseMenu();
        }

        private void ExchangePiece()
        {
            if (_application.Game.IsRunning && CanExchangePiece)
            {
                // Save Board.CurrentPiece in TemporaryPiece.
                var tempPiece = new PreviewPiece(this, Board.CurrentPiece.Color, Board.CurrentPiece.Model,
                                                 Board.CurrentPiece.RotationIndex, Board.CurrentPiece.Position);
                // Save NextPiece to CurrentPiece.
                Board.CurrentPiece = new Piece(this, PieceGenerator.PeekNextPiece(), Board.CurrentPiece.Position);
                // Save TemporaryPiece to NextPiece.
                PieceGenerator.SetNextPiece(tempPiece);
                // Set Flag to Prevent Another Exchange.
                CanExchangePiece = false;
            }
        }
        #endregion
    }
}
