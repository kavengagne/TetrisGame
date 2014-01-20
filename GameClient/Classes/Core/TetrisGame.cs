using GameClient.Classes.Core.Inputs;
using GameClient.Classes.GameBoard;
using GameConfiguration.DataObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// TODO: KG - Tuning: Add more delay before Storing Piece to array. This will allow players to get a chance to move the piece before it locks.
// TODO: KG - Tuning: Add more Error Handling
// TODO: KG - Tuning: Optimize Score
// TODO: KG - Bug: Fix Pieces Randomization.
// TODO: KG - Bug: ?Fix Shape Rotation Against Wall. (Should Move it Away From Wall but remember where it was if rotating again)
// TODO: KG - Bug: ?Fix L Shape Rotation.
// TODO: KG - Bug: Fix ScoreBoard Font.
// TODO: KG - Feature: Add FullScreen Support. (Using Scaling)
// TODO: KG - Feature: Game Levels. Levels increase Game Speed.
// TODO: KG - Feature: Show More Next Pieces (Maybe 2 or 3).
// TODO: KG - Feature: Show a Preview of the Piece Position if Dropped.
// TODO: KG - Feature: Change Pieces colors when leveling.
// TODO: KG - Feature: Make Sounds much stronger when performing a Tetris.
// TODO: KG - Feature: Holds the CurrentPiece into a buffer so you can use it later. (Will switch with the CurrentPiece at this time) (Default Key: Shift)
// TODO: KG - Feature: Auto Update.
// TODO: KG - Feature: Game Reset.
// TODO: KG - Feature: Game Over Handling.
// TODO: KG - Feature: InputManager Key Settings Handling. (Keyboard, Mouse, Xbox Controller)
// TODO: KG - Feature: Game Options. (Windows Form Project, Maybe)
// TODO: KG - Feature: Add Musics (Should create those myself)
// TODO: KG - Feature: Add Pause Menu. (Default Key: P)

namespace GameClient.Classes.Core
{
    public class TetrisGame : Game
    {
        #region Fields
        private SpriteBatch _spriteBatch;
        private readonly Application _application;
        #endregion


        #region Properties
        public PieceGenerator PieceGenerator { get; set; }
        public Board Board { get; set; }
        public PreviewPanel PreviewPanel { get; set; }
        public ScoreBoard ScoreBoard { get; set; }
        public SoundManager SoundManager { get; set; }
        public InputManager InputManager { get; set; }
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
            var bounds = new Rectangle(Board.Bounds.X + Board.Bounds.Width + 5, 40 + 100 + 5, 100, 100);
            ScoreBoard = new ScoreBoard(this, bounds, _application.Configuration.Board.BackgroundColor);
        }

        private void RegisterUserInputs()
        {
            InputManager.RegisterKeyPressed(Keys.P, TogglePause);
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
        }
        #endregion
    }
}
