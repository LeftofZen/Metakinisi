using Graph;
using Metakinisi.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Gui;
using MonoGame.Extended.Gui.Controls;
using MonoGame.Extended.ViewportAdapters;
using MonoGame.ImGui;
using ImGuiNET;
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

		//public static readonly UIManager UIManager = new();
		public static ImGUIRenderer GuiRenderer;

		public static IGridWorld GridWorld;
	}

	public class MainGame : Game
	{
		private GraphicsDeviceManager _graphics;
		private SpriteBatch _spriteBatch;
		GuiSystem _guiSystem;

		public MainGame()
		{
			_graphics = new GraphicsDeviceManager(this);
			//_graphics.PreferMultiSampling = true;
			Content.RootDirectory = "Content";
			IsMouseVisible = true;

			Window.AllowUserResizing = true;
			//Window.ClientSizeChanged += WindowOnClientSizeChanged;
			Window.Title = "Metakinisi";
			//Window.IsBorderless = true;

			GameServices.Game = this;
			GameServices.GridWorld = new GridWorld(16, 16);
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

			GameServices.GuiRenderer = new ImGUIRenderer(this).Initialize().RebuildFontAtlas();

			base.Initialize();
			MaximiseWindow();

			GameServices.GridWorld.RenderTarget = new RenderTarget2D(
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
		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}

			GameServices.InputManager.Update(gameTime);

			GameServices.GridWorld.Update(gameTime);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.SteelBlue);

			GameServices.GridWorld.Draw(_spriteBatch);
			//gamePanel.renderTarget = GameServices.GridWorld.RenderTarget;

			_spriteBatch.Begin();

			_spriteBatch.DrawString(GameServices.Fonts["Calibri"], GameServices.InputManager.CurrentMouse.Position.ToString(), new Vector2(20, 200), Color.White);

			_spriteBatch.End();

			base.Draw(gameTime);

			DrawImgui(gameTime);
		}

		public void DrawImgui(GameTime gameTime)
		{
			GameServices.GuiRenderer.BeginLayout(gameTime);

			if (ImGui.CollapsingHeader("Test"))
			{

			}

			GameServices.GuiRenderer.EndLayout();
		}
	}

	public class DemoViewModel
	{
		public DemoViewModel(string name, object content)
		{
			Name = name;
			Content = content;
		}

		public string Name { get; }
		public object Content { get; }

		public override string ToString()
		{
			return Name;
		}
	}
}
