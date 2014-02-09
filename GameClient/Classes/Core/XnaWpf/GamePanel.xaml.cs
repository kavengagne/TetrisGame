using System;
using System.Windows;
using System.Windows.Interop;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Classes.Core.XnaWpf
{
    public partial class GamePanel
    {
        public Game Game
        {
            get { return (Game)GetValue(GameProperty); }
            set { SetValue(GameProperty, value); }
        }

        public static readonly DependencyProperty GameProperty = DependencyProperty.Register("Game", typeof(Game), typeof(GamePanel));

        public GamePanel()
        {
            InitializeComponent();

            this.Loaded += GamePanel_Loaded;
        }

        void GamePanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (ReferenceEquals(Game, null))
            {
                return;
            }

            GameReflector.CreateGame(Game, XnaImage);
            //Set the back buffer for the D3DImage, since unlocking it without one will thrown and exception
            SetD3DImageBackBuffer(CreateRenderTarget(1, 1));

#if RESIZABLE
            //Register for size changed if using a RenderTarget2D for drawing
            XnaImage.SizeChanged += OnSizeChanged;
#endif
            //Register for Rendering to perform updates and drawing
            System.Windows.Media.CompositionTarget.Rendering += OnRendering;
        }

        private void OnRendering(object sender, EventArgs e)
        {
            D3DImage.Lock();
            //Update and draw the game, then invalidate the D3DImage
            Game.Tick();
            D3DImage.AddDirtyRect(new Int32Rect(0, 0, D3DImage.PixelWidth, D3DImage.PixelHeight));
            D3DImage.Unlock();

            var window = Window.GetWindow(this);
            if (window != null)
            {
                window.Title = Game.Window.Title;
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Can't create a RenderTarget2D with dimensions smaller than zero
            if ((int)XnaImage.ActualWidth <= 0 || (int)XnaImage.ActualHeight <= 0)
            {
                return;
            }

            var renderTarget = CreateRenderTarget((int)XnaImage.ActualWidth, (int)XnaImage.ActualHeight);
            if (!ReferenceEquals(renderTarget, null))
            {
                //Direct the game's drawing to the newly created RenderTarget2D,
                //whose surface will be displayed in the D3DImage
                Game.GraphicsDevice.SetRenderTarget(renderTarget);
                SetD3DImageBackBuffer(renderTarget);
            }
        }

        private RenderTarget2D CreateRenderTarget(int width, int height)
        {
            return new RenderTarget2D(Game.GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24);
        }

        private void SetD3DImageBackBuffer(RenderTarget2D renderTarget)
        {
            D3DImage.Lock();
#if RESIZABLE
            //Get the surface from the RenderTarget2D if using one for drawing
            D3DImage.SetBackBuffer(D3DResourceType.IDirect3DSurface9, GameReflector.GetRenderTargetSurface(renderTarget));
#else
            //If not using a RenderTarget for drawing, get the surface from the GraphicsDevice
            D3DImage.SetBackBuffer(D3DResourceType.IDirect3DSurface9, GameReflector.GetGraphicsDeviceSurface(Game.GraphicsDevice));
#endif
            D3DImage.Unlock();
        }
    }
}
