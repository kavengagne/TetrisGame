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
// TODO: KG - Bug: Improve Ghost Piece look.
// TODO: KG - Bug: Change Board Background Color.
// TODO: KG - Bug: Make Pieces Colors Consistents.
// TODO: KG - Feature: Game Main Screen.
// TODO: KG - Feature: Server Statistics Logging.
// TODO: KG - Feature: Client-Side Scoreboard.
// TODO: KG - Feature: Add Game Time.
// TODO: KG - Feature: Game Options.
// TODO: KG - Feature: InputManager Key Settings Handling. (Keyboard, Mouse, Xbox Controller)
// TODO: KG - Feature: Show More Next Pieces. (Maybe 2 or 3)
// TODO: KG - Feature: Add small delay after moving piece. This will allow players to place the piece before it locks.
// TODO: KG - Tuning: Add more Error Handling.
// TODO: KG - Tuning: Optimize Score. (Wikipedia: Tetris)
// TODO: KG - Bug: Add OpenAL to Release Bundle.
// TODO: KG - Bug: Fix ScoreBoard Font Display.
// TODO: KG - Feature: Auto Update.
// TODO: KG - Feature: Create Installer Project.
// TODO: KG - Feature: Add Help Feature. (Default Key: H)
// TODO: KG - Feature: Add FullScreen Support. (Using Scaling)
// TODO: KG - Feature: Game Levels. Levels increase Game Speed. Level-up after N completed lines.
// TODO: KG - Feature?: Change Game Theme when leveling.
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
        private SpriteBatch _spriteBatch;
        private readonly Application _application;
        #endregion


        #region Properties
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
            _application = Application.Instance;
            Content.RootDirectory = "Content";
        }
        #endregion


        #region Methods Overrides
        protected override void Initialize()
        {
            InitializeSoundManager();
            InitializeInputManager();
            //InitializeTextureManager();
            
            InitializePieceGenerator(_application.Configuration.Pieces,
                                     _application.Configuration.PiecesColors,
                                     _application.Configuration.Board.BlockSize);

            InitializeTetrisBoard();
            InitializePreviewPanel();
            InitializeScoreBoard();

            RegisterUserInputs();
            _application.IsRunning = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
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
            _spriteBatch.Begin();
            Board.Draw(_spriteBatch, gameTime);
            PreviewPanel.Draw(_spriteBatch, gameTime);
            ScoreBoard.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();
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

        private void InitializePieceGenerator(PieceInformation[] pieces, Color[] colors, Rectangle blockSize)
        {
            PieceGenerator = new PieceGenerator(this, pieces, colors, blockSize);
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
            InputManager.RegisterKeyPressed(Keys.Escape, Application.Exit);
        }

        private void TogglePause()
        {
            _application.IsRunning = !_application.IsRunning;
            SoundManager.Play("Pause");
            //TogglePauseMenu();
        }

        private void ExchangePiece()
        {
            if (CanExchangePiece)
            {
                // Save Board.CurrentPiece in TemporaryPiece.
                var tempPiece = new PreviewPiece(this, Board.CurrentPiece.Color, Board.CurrentPiece.Model,
                                                 Board.CurrentPiece.RotationIndex, Board.CurrentPiece.BlockSize,
                                                 Board.CurrentPiece.Position);
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
