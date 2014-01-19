using GameClient.Classes.GameBoard;
using GameClient.Classes.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Point = Microsoft.Xna.Framework.Point;


// TODO: KG - Tuning: Add more delay before Storing Piece to array. This will allow players to get a chance to move the piece before it locks.
// TODO: KG - Bug: ?Fix Shape Rotation Against Wall. (Should Move it Away From Wall but remember where it was if rotating again)
// TODO: KG - Bug: ?Fix L Shape Rotation.
// TODO: KG - Bug: Fix PreviewPanel Piece Position.
// TODO: KG - Bug: Fix ScoreBoard Font and Alignment.
// TODO: KG - Bug: Make all Pieces of the same shape the same color.
// TODO: KG - Feature: Add FullScreen Support. (Using Scaling)
// TODO: KG - Feature: Game Levels. Levels increase Game Speed.
// TODO: KG - Feature: Show More Next Pieces (Maybe 2 or 3).
// TODO: KG - Feature: Show a Preview of the Piece Position if Dropped.
// TODO: KG - Feature: Change Pieces colors when leveling.
// TODO: KG - Feature: Make Sounds much stronger when performing a Tetris.
// TODO: KG - Feature: Shift Key - Holds the CurrentPiece into a buffer so you can use it later. (Will switch with the CurrentPiece at this time)
// TODO: KG - Feature: Auto Update.
// TODO: KG - Feature: Game Reset.
// TODO: KG - Feature: Failure Handling.
// TODO: KG - Feature: InputManager Key Settings Handling. (Keyboard, Mouse, Xbox Controller)
// TODO: KG - Feature: Game Options. (Windows Form Project)
// TODO: KG - Feature: Add Musics (Should create those myself)

namespace GameClient.Classes
{
    public class TetrisGame : Game
    {
        #region Fields
        private SpriteBatch _spriteBatch;
        private Board _board;
        private PreviewPanel _previewPanel;
        private ScoreBoard _scoreBoard;
        private readonly Application _application;
        #endregion


        #region Properties
        public SoundManager SoundManager { get; set; }
        public InputManager InputManager { get; set; }
        public Score Score { get; set; }
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
            InitializeTetrisBoard();
            InitializePreviewPanel();
            InitializeScoreBoard();
            // InitializePanelSomething();
            InitializeInputManager();
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
            _board.Update(gameTime);
            _previewPanel.Update(gameTime);
            _scoreBoard.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(_application.Configuration.Game.BackgroundColor);
            _spriteBatch.Begin();
            _board.Draw(_spriteBatch, gameTime);
            _previewPanel.Draw(_spriteBatch, gameTime);
            _scoreBoard.Draw(_spriteBatch, gameTime);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
        #endregion


        #region Public Methods

        #endregion


        #region Internal Implementation
        private void InitializeSoundManager()
        {
            SoundManager = new SoundManager(Content);
        }

        private void InitializeTetrisBoard()
        {
            // TODO: KG - Move to config or something
            _board = new Board(this, new Point(40, 40));
        }

        private void InitializePreviewPanel()
        {
            var bounds = new Rectangle(_board.Bounds.X + _board.Bounds.Width + 5, 40, 80, 80);
            _previewPanel = new PreviewPanel(this, _board, bounds, _application.Configuration.Board.BackgroundColor);
        }

        private void InitializeScoreBoard()
        {
            // TODO: KG - Move to config or something
            var bounds = new Rectangle(_board.Bounds.X + _board.Bounds.Width + 5, 40 + 80 + 5, 80, 80);
            _scoreBoard = new ScoreBoard(this, bounds, _application.Configuration.Board.BackgroundColor);
        }

        private void InitializeInputManager()
        {
            InputManager = new InputManager();
        }

        private void RegisterUserInputs()
        {
            InputManager.RegisterKeyPressed(Keys.P, TogglePause);
            InputManager.RegisterKeyPressed(Keys.Space, _board.DropPieceAllTheWay);
            InputManager.RegisterKeyPressed(Keys.Up, _board.RotateLeft);
            InputManager.RegisterKeyPressed(Keys.LeftControl, _board.RotateRight);
            InputManager.RegisterKeyPressed(Keys.Down, _board.DropPieceByOne, true, 100);
            // TODO: KG - Adjust left/right speed
            InputManager.RegisterKeyPressed(Keys.Left, _board.MoveLeft, true, 100);
            InputManager.RegisterKeyPressed(Keys.Right, _board.MoveRight, true, 100);
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
