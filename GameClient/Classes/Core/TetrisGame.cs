using GameClient.Classes.Core.Managers;
using GameClient.Classes.Core.Randomizer;
using GameClient.Classes.Core.Settings;
using GameClient.Classes.GameBoard;
using GameClient.Classes.ParticleSystem;
using GameClient.Classes.Screens;
using GameClient.Classes.StateManager;
using Microsoft.Xna.Framework;

// TODO: KG - Move All Strings to Localization Files.
// TODO: KG - Bug: Correct Pieces Starting Position.
// TODO: KG - Bug: Change Hold Behavior. (Should stay in Hold slot until used) (Should Reset Piece Position to 0) (Create HoldPanel)
// TODO: KG - Adhere to Tetris Guidelines: http://harddrop.com/wiki/Tetris_Guideline
// TODO: KG - *Tuning: Redesign the GameBoard.
// TODO: KG - *Tuning: Improve Ghost Piece look.
// TODO: KG - *Tuning: Change Board Background Color or Image.
// TODO: KG - *Tuning: Make Pieces Colors Consistents.
// TODO: KG - Feature: Show More Next Pieces. (Maybe 2 or 3)
// TODO: KG - Bug: Fix Fullscreen Resolution.
// TODO: KG - Bug: Correct Scaling Calculation Code.
// TODO: KG - Feature: Share Game with Friends.
// TODO: KG - Feature: Server Statistics Logging.
// TODO: KG - Feature: Client-Side Scoreboard.
// TODO: KG - Feature: Add Game Time.
// TODO: KG - Feature: InputManager Key Settings Handling. (Keyboard, Mouse, Xbox Controller)
// TODO: KG - Feature: InputManager Support for Key Combinations. (Ctrl + Key, Alt + Key, Shift + Key)
// TODO: KG - Feature: Add small delay after moving piece. This will allow players to place the piece before it locks.
// TODO: KG - Tuning: Add more Error Handling.
// TODO: KG - Tuning: Optimize Score. http://en.wikipedia.org/wiki/Tetris
// TODO: KG - Tuning: Implement T-Spin Bonus. http://harddrop.com/wiki/T-Spin
// TODO: KG - Tuning: Implement Following Bonuses: Single, Double, Tetris, BackToBack, T-Spin.
// TODO: KG - Tuning: Implement Combos x1, x2, x3, x4, ...
// TODO: KG - Bug: Fix ScoreBoard Font Display.
// TODO: KG - Feature: Auto Update.
// TODO: KG - Feature: Create Installer Project.
// TODO: KG - Feature: Game Levels. Levels increase Game Speed. Level-up after N completed lines.
// TODO: KG - Feature: Change Game Theme when leveling.
// TODO: KG - Feature: Add Musics. (Should create those myself)
// TODO: KG - Feature: Implement Sounds/Voices for Bonuses: Single, Double, Tetris, BackToBack, T-Spin.
// TODO: KG - Feature: Implement Sounds/Voices for Events: Go!, Game Over, Level Up.
// TODO: KG - Feature: Make Sounds much stronger when performing a Tetris.
// TODO: KG - Feature: Implement GameState Management.
// TODO: KG - Feature: Integrate WPF/WinForms Components inside XNA. (For Menus and Configurations / Statistics Widgets)
// TODO: KG - Feature: Game Main Screen.
// TODO: KG - Feature: Game Options.
// TODO: KG - Feature: Add Help Feature. (Default Key: H)
// TODO: KG - Feature: Add Pause Menu. (Default Key: Escape)
// TODO: KG - Feature: Game Over Handling.
// TODO: KG - Feature: Finish Game Reset. (Add Confirmation)
// TODO: KG - Feature: Finish Game Quit. (Add Confirmation)

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


        #region Properties
        public Color BackgroundColor { get; set; }
        public bool IsRunning { get; set; }
        public ParticleEngine ParticleEngine { get; set; }
        public Matrix SpriteScale { get; set; }
        #endregion


        #region Constructors
        private TetrisGame()
        {
            // ReSharper disable once ObjectCreationAsStatement
            new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Configuration.GetInstance().WindowWidth,
                PreferredBackBufferHeight = Configuration.GetInstance().WindowHeight,
                PreferMultiSampling = true,
                SynchronizeWithVerticalRetrace = true
            };
            IsMouseVisible = true;
            Content.RootDirectory = "Content";
            BackgroundColor = Defaults.Game.BackgroundColor;

            var screenManager = new ScreenManager(this) { TraceEnabled = false };
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);
            Components.Add(screenManager);
            IsRunning = true;
        }
        #endregion


        #region Public Methods
        public void ChangeBackgroundColor(Score score, int updateValue)
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
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteScale = GetSpriteScale(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            SoundManager.GetInstance();
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            SpriteScale = GetSpriteScale(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            SoundManager.GetInstance().Update(gameTime);
            //Console.WriteLine("w:{0}, h:{1}", GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            //ParticleEngine.EmitterLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            //ParticleEngine.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackgroundColor);
            base.Draw(gameTime);
        }
        #endregion


        #region Internal Implementation
        private static Matrix GetSpriteScale(int width, int height)
        {
            float xScale = (float)width / Configuration.GetInstance().WindowWidth;
            float yScale = (float)height / Configuration.GetInstance().WindowHeight;
            float chosenScale = (width > height) ? yScale : xScale;
            return Matrix.CreateScale(chosenScale, chosenScale, 1);
        }
        #endregion
    }
}
