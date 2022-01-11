using Graph;
using Metakinisi.Input;
using Metakinisi.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Runtime.InteropServices;

namespace Metakinisi
{
	public static class GameServices
	{
		public static readonly Dictionary<string, SpriteFont> Fonts = new();
		public static readonly Dictionary<string, Texture2D> Textures = new();
		public static readonly InputManager InputManager = new();
		public static readonly Random Random = new(1);
		//public static readonly GameLogger Logger = new();
		//public static readonly Dictionary<string, IGameEntity> EntityDefinitions = new();

		public static Game Game;
		public static int GameWidth => Game.GraphicsDevice.Viewport.Width;
		public static int GameHeight => Game.GraphicsDevice.Viewport.Height;

		public const int GridSize = 32;

		public static readonly UIManager UIManager = new();

		public static IGridWorld GridWorld;
	}

	public class MainGame : Game
	{
		#region Maximise Window

		private const string SDL = "SDL2.dll";
		[DllImport(SDL, CallingConvention = CallingConvention.Cdecl)]
		public static extern void SDL_MaximizeWindow(IntPtr window);

		private void MaximizeWindow()
		{
			SDL_MaximizeWindow(Window.Handle);
		}

		#endregion

		//GridWorld world;
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;

		Panel UIParent;
		Panel gamePanel;

		public MainGame()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			Window.AllowUserResizing = true;
			Window.Title = "Metakinisi";
			//Window.IsBorderless = true;
			GameServices.Game = this;
			GameServices.GridWorld = new GridWorld(16, 16);
		}

		protected override void Initialize()
		{
			//_graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
			//_graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
			//_graphics.IsFullScreen = true;

			_graphics.PreferredBackBufferHeight = 1080;
			_graphics.PreferredBackBufferWidth = 1920;
			_graphics.ApplyChanges();

			//var form = (Form)Form.FromHandle(Window.Handle);
			//form.WindowState = FormWindowState.Maximized;

			//var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 800, 600);
			//_camera = new OrthographicCamera(viewportadapter);
			//world = new GridWorld(24, 16);

			base.Initialize();

			GameServices.GridWorld.RenderTarget = new RenderTarget2D(
				GraphicsDevice,
				1600,//GraphicsDevice.PresentationParameters.BackBufferWidth,
				900,//GraphicsDevice.PresentationParameters.BackBufferHeight,
				false,
				GraphicsDevice.PresentationParameters.BackBufferFormat,
				DepthFormat.Depth24);

			//Form form = (Form)Control.FromHandle(Window.Handle);
			//form.WindowState = FormWindowState.Maximized;

			//MaximizeWindow();
		}

		protected override void LoadContent()
		{
			//_tiledMap = Content.Load<TiledMap>("tiles\\samplemap");
			//_tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);

			GameServices.Fonts.Add("Calibri", Content.Load<SpriteFont>("Calibri"));
			GameServices.Textures.Add("tileset", Content.Load<Texture2D>("Textures\\tileset1"));
			GameServices.Textures.Add("ui", Content.Load<Texture2D>("Textures\\ui"));
			//GameServices.Textures.Add("ui", Content.Load<Texture2D>("tiles/18x18_ui"));

			//var path = Content.Load<ItemDefinition[]>("config/items");
			//var path = @"Content/config/items.json";
			//var options = new JsonSerializerOptions
			//{
			//	PropertyNameCaseInsensitive = true
			//};
			//var entityDefs = JsonSerializer.Deserialize<ItemDefinitionJson>(File.ReadAllText(path), options);

			_spriteBatch = new SpriteBatch(GraphicsDevice);


			// create UI here

			//var demoWindow = new Window(new Rectangle(20, 20, 200, 100), "Window 1");
			//demoWindow.BackColor = Color.Gray;
			//demoWindow.BorderStyle = new BorderStyle { Color = Color.Black, Thickness = 2 };
			//demoWindow.ZIndex = 10;

			var trackSelectionWindow = new TrackPlacementWindow(new Rectangle(600, 20, 600, 400), "Track Placement");
			trackSelectionWindow.BackColor = Color.Gray;
			trackSelectionWindow.BorderStyle = new BorderStyle { Color = Color.Black, Thickness = 2 };
			trackSelectionWindow.ZIndex = 10;
			GameServices.GridWorld.SetCurrentTool(trackSelectionWindow.tool);

			gamePanel = new Panel(new Rectangle(0, 0, 1600, 900));
			gamePanel.BackColor = Color.Yellow;
			gamePanel.ZIndex = 0;

			UIParent = new Panel(new Rectangle(0, 0, 1600, 900));
			UIParent.AddControl(trackSelectionWindow);
			//UIParent.AddControl(demoWindow);
			UIParent.AddControl(gamePanel);

			GameServices.UIManager.TopLevelControl = UIParent;

		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}

			GameServices.InputManager.Update(gameTime);
			GameServices.UIManager.Update(gameTime);

			GameServices.GridWorld.Update(gameTime);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.SteelBlue);

			GameServices.GridWorld.Draw(_spriteBatch);

			_spriteBatch.Begin();

			gamePanel.renderTarget = GameServices.GridWorld.RenderTarget;
			GameServices.UIManager.Draw(_spriteBatch);

			_spriteBatch.DrawString(GameServices.Fonts["Calibri"], GameServices.InputManager.CurrentMouse.Position.ToString(), new Vector2(20, 200), Color.White);

			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
