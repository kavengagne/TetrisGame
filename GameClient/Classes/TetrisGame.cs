using GameClient.Classes.GameBoard;
using GameClient.Classes.Inputs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Point = Microsoft.Xna.Framework.Point;


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
            InputManager.RegisterKeyPressed(Keys.Space, _board.DropPiece);
            InputManager.RegisterKeyPressed(Keys.Up, _board.RotateLeft);
            InputManager.RegisterKeyPressed(Keys.Down, _board.RotateRight);
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
        }
        #endregion
    }
}
