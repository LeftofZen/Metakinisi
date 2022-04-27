using Endeavour.Services;
using Endeavour.UI;
using Graph;
using Metakinisi.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Gui;
using System.Runtime.InteropServices;

namespace Metakinisi
{
	public class MainGame : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		GuiSystem _guiSystem;

		Panel gamePanel;

		public MainGame()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;

			Window.AllowUserResizing = true;
			//Window.ClientSizeChanged += WindowOnClientSizeChanged;
			Window.Title = "Metakinisi";
			//Window.IsBorderless = true;

			GameServices.Game = this;
			GameServices.AddService(new GridWorld(16, 16));
		}

		//private void WindowOnClientSizeChanged(object sender, EventArgs eventArgs)
		//{
		//	_guiSystem.ClientSizeChanged();
		//}

		protected override void Initialize()
		{
			//_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
			//_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			//_graphics.IsFullScreen = true;



			//_graphics.PreferredBackBufferHeight = 1440;
			//_graphics.PreferredBackBufferWidth = 2560;
			//_graphics.ApplyChanges();

			//var form = (Form)Form.FromHandle(Window.Handle);
			//form.WindowState = FormWindowState.Maximized;

			//var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 800, 600);
			//_camera = new OrthographicCamera(viewportadapter);
			//world = new GridWorld(24, 16);

			base.Initialize();
			MaximiseWindow();

			GameServices.GetService<GridWorld>().RenderTarget = new RenderTarget2D(
				GraphicsDevice,
				1600,//GraphicsDevice.PresentationParameters.BackBufferWidth,
				900,//GraphicsDevice.PresentationParameters.BackBufferHeight,
				false,
				GraphicsDevice.PresentationParameters.BackBufferFormat,
				DepthFormat.Depth24);

			//Form form = (Form)Control.FromHandle(Window.Handle);
			//form.WindowState = FormWindowState.Maximized;
		}

		#region Maximise Window

		private const string SDL = "SDL2.dll";
		[DllImport(SDL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_MaximizeWindow(IntPtr window);

		private void MaximiseWindow()
		{
			Window.Position = new Point(0, 0);
			SDL_MaximizeWindow(Window.Handle);
		}

		#endregion

		protected override void LoadContent()
		{
			GameServices.Fonts.Add("Calibri", Content.Load<SpriteFont>("Fonts\\Calibri"));
			GameServices.Textures.Add("tileset", Content.Load<Texture2D>("Textures\\tileset1"));
			GameServices.Textures.Add("ui", Content.Load<Texture2D>("Textures\\ui"));

			_spriteBatch = new SpriteBatch(GraphicsDevice);

			// create UI here

			var trackPlacementWindow = new TrackPlacementWindow(new Rectangle(600, 20, 600, 400), "Track Placement");
			trackPlacementWindow.BackColor = Color.Gray;
			trackPlacementWindow.BorderStyle = new BorderStyle { Color = Color.Black, Thickness = 2 };
			trackPlacementWindow.ZIndex = 10;
			GameServices.GetService<GridWorld>().SetCurrentTool(trackPlacementWindow.tool);

			var vehicleManagementWindow = new VehicleManagementWindow(new Rectangle(800, 30, 800, 600), "Vehicle Management");
			vehicleManagementWindow.ZIndex = 20;

			var gameWindow = new Window(new Rectangle(100, 100, 1600, 900), "Main Game");
			gameWindow.ZIndex = 0;
			gameWindow.LockZIndex = true; // nothing can move this up, this is the base game layer
			gameWindow.BorderStyle = new BorderStyle(Color.White, 0);
			gamePanel = new Panel(new Rectangle(0, 24, 1600, 900 - 24));
			gamePanel.Name = "Game Panel";
			gamePanel.BackColor = Color.Yellow;
			gamePanel.ZIndex = 0;
			gameWindow.AddControl(gamePanel);

			//UIParent = new Panel(new Rectangle(0, 0, 1600, 900));
			//UIParent.AddControl(trackPlacementWindow);
			////UIParent.AddControl(demoWindow);
			//UIParent.AddControl(gamePanel);

			GameServices.UIManager.Windows.Add(trackPlacementWindow);
			GameServices.UIManager.Windows.Add(gameWindow);
			GameServices.UIManager.Windows.Add(vehicleManagementWindow);
			//GameServices.UIManager.TopLevelControl = UIParent;
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}

			GameServices.InputManager.Update(gameTime);
			GameServices.UIManager.Update(gameTime);
			GameServices.GetService<GridWorld>().Update(gameTime);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.SteelBlue);

			GameServices.GetService<GridWorld>().Draw(_spriteBatch);
			gamePanel.renderTarget = GameServices.GetService<GridWorld>().RenderTarget;

			_spriteBatch.Begin();

			GameServices.UIManager.Draw(_spriteBatch);

			_spriteBatch.DrawString(GameServices.Fonts["Calibri"], GameServices.InputManager.CurrentMouse.Position.ToString(), new Vector2(20, 200), Color.White);

			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
