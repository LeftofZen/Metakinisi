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
		public static readonly Random Random = new(1);
		//public static readonly GameLogger Logger = new();
		//public static readonly Dictionary<string, IGameEntity> EntityDefinitions = new();

		public static Game Game;
		public static int GameWidth => Game.GraphicsDevice.Viewport.Width;
		public static int GameHeight => Game.GraphicsDevice.Viewport.Height;
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

		GridWorld world;
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		//private TiledMap _tiledMap;
		//private TiledMapRenderer _tiledMapRenderer;
		//private OrthographicCamera _camera;
		//private Vector2 _cameraPosition;
		//private GridWorld world;

		//Rectangle worldRenderRect;
		//Rectangle

		public MainGame()
		{
			_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
			Window.AllowUserResizing = true;
			Window.Title = "Metakinisi";
			//Window.IsBorderless = true;
			GameServices.Game = this;

			world = new GridWorld();
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

			//Form form = (Form)Control.FromHandle(Window.Handle);
			//form.WindowState = FormWindowState.Maximized;

			//MaximizeWindow();
		}

		protected override void LoadContent()
		{
			//_tiledMap = Content.Load<TiledMap>("tiles\\samplemap");
			//_tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);

			GameServices.Fonts.Add("Calibri", Content.Load<SpriteFont>("Calibri"));
			//GameServices.Textures.Add("ui", Content.Load<Texture2D>("tiles/18x18_ui"));

			//var path = Content.Load<ItemDefinition[]>("config/items");
			//var path = @"Content/config/items.json";
			//var options = new JsonSerializerOptions
			//{
			//	PropertyNameCaseInsensitive = true
			//};
			//var entityDefs = JsonSerializer.Deserialize<ItemDefinitionJson>(File.ReadAllText(path), options);

			_spriteBatch = new SpriteBatch(GraphicsDevice);
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}

			world.Update(gameTime);

			base.Update(gameTime);
		}


		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.SteelBlue);

			_spriteBatch.Begin(blendState: BlendState.AlphaBlend);

			world.Draw(_spriteBatch);

			_spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
