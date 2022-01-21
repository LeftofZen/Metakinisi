using Graph;
using Metakinisi.Input;
using Metakinisi.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.Gui;
using MonoGame.Extended.Gui.Controls;
using MonoGame.Extended.ViewportAdapters;
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

			// create UI here

			var trackPlacementWindow = new TrackPlacementWindow(new Rectangle(600, 20, 600, 400), "Track Placement");
			trackPlacementWindow.BackColor = Color.Gray;
			trackPlacementWindow.BorderStyle = new BorderStyle { Color = Color.Black, Thickness = 2 };
			trackPlacementWindow.ZIndex = 10;
			GameServices.GridWorld.SetCurrentTool(trackPlacementWindow.tool);

			var vehicleManagementWindow = new VehicleManagementWindow(new Rectangle(800, 30, 800, 600), "Vehicle Management");
			vehicleManagementWindow.ZIndex = 20;

			var gameWindow = new Window(new Rectangle(100, 100, 1600, 900), "Main Game");
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

			//// Monogame.Extended.Gui
			//var viewportAdapter = new DefaultViewportAdapter(GraphicsDevice);
			//var guiRenderer = new GuiSpriteBatchRenderer(GraphicsDevice, () => Matrix.Identity);
			//var font = Content.Load<BitmapFont>("Fonts\\Sensation");
			//BitmapFont.UseKernings = false;
			//Skin.CreateDefault(font);

			//var controlTest = new DemoViewModel("Basic Controls",
			//	new StackPanel
			//	{
			//		Margin = 5,
			//		Orientation = Orientation.Vertical,
			//		Items =
			//		{
			//			new MonoGame.Extended.Gui.Controls.Label("Buttons") { Margin = 5 },
			//			new StackPanel
			//			{
			//				Orientation = Orientation.Horizontal,
			//				Spacing = 5,
			//				Items =
			//				{
			//					new MonoGame.Extended.Gui.Controls.Button { Content = "Enabled" },
			//					new MonoGame.Extended.Gui.Controls.Button { Content = "Disabled", IsEnabled = false },
			//					new ToggleButton { Content = "ToggleButton" }
			//				}
			//			},

			//			new MonoGame.Extended.Gui.Controls.Label("TextBox") { Margin = 5 },
			//			new TextBox {Text = "TextBox" },

			//			new MonoGame.Extended.Gui.Controls.Label("CheckBox") { Margin = 5 },
			//			new CheckBox {Content = "Check me please!"},

			//			new MonoGame.Extended.Gui.Controls.Label("ListBox") { Margin = 5 },
			//			new ListBox {Items = {"ListBoxItem1", "ListBoxItem2", "ListBoxItem3"}, SelectedIndex = 0},

			//			new MonoGame.Extended.Gui.Controls.Label("ProgressBar") { Margin = 5 },
			//			new ProgressBar {Progress = 0.5f, Width = 100},

			//			new MonoGame.Extended.Gui.Controls.Label("ComboBox") { Margin = 5 },
			//			new ComboBox {Items = {"ComboBoxItemA", "ComboBoxItemB", "ComboBoxItemC"}, SelectedIndex = 0, HorizontalAlignment = HorizontalAlignment.Left}
			//		}
			//	});

			//var stackTest = new DemoViewModel("Stack Panels",
			//		new StackPanel
			//		{
			//			Items =
			//			{
			//				new MonoGame.Extended.Gui.Controls.Button { Content = "Press Me", HorizontalAlignment = HorizontalAlignment.Left, VerticalAlignment = VerticalAlignment.Top },
			//				new MonoGame.Extended.Gui.Controls.Button { Content = "Press Me", HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Bottom  },
			//				new MonoGame.Extended.Gui.Controls.Button { Content = "Press Me", HorizontalAlignment = HorizontalAlignment.Centre, VerticalAlignment = VerticalAlignment.Centre  },
			//				new MonoGame.Extended.Gui.Controls.Button { Content = "Press Me", HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch },
			//			}
			//		});

			//var dockTest = new DemoViewModel("Dock Panels",
			//	new DockPanel
			//	{
			//		Items =
			//		{
			//			new MonoGame.Extended.Gui.Controls.Button { Content = "Dock.Top", AttachedProperties = { { DockPanel.DockProperty, Dock.Top } } },
			//			new MonoGame.Extended.Gui.Controls.Button { Content = "Dock.Bottom", AttachedProperties = { { DockPanel.DockProperty, Dock.Bottom } } },
			//			new MonoGame.Extended.Gui.Controls.Button { Content = "Dock.Left", AttachedProperties = { { DockPanel.DockProperty, Dock.Left } } },
			//			new MonoGame.Extended.Gui.Controls.Button { Content = "Dock.Right", AttachedProperties = { { DockPanel.DockProperty, Dock.Right } } },
			//			new MonoGame.Extended.Gui.Controls.Button { Content = "Fill" }
			//		}
			//	});

			//var demoScreen = new Screen
			//{
			//	Content = new DockPanel
			//	{
			//		LastChildFill = true,
			//		Items =
			//		{
			//			new ListBox
			//			{
			//				Name = "DemoList",
			//				AttachedProperties = { { DockPanel.DockProperty, Dock.Left} },
			//				ItemPadding = new Thickness(5),
			//				VerticalAlignment = VerticalAlignment.Stretch,
			//				HorizontalAlignment = HorizontalAlignment.Left,
			//				SelectedIndex = 0,
			//				Items = { controlTest, stackTest, dockTest }
			//			},
			//			new ContentControl
			//			{
			//				Name = "Content",
			//				BackgroundColor = new Color(30, 30, 30)
			//			}
			//		}
			//	}
			//};

			//_guiSystem = new GuiSystem(viewportAdapter, guiRenderer) { ActiveScreen = demoScreen };

			//var demoList = demoScreen.FindControl<ListBox>("DemoList");
			//var demoContent = demoScreen.FindControl<ContentControl>("Content");

			//demoList.SelectedIndexChanged += (sender, args) => demoContent.Content = (demoList.SelectedItem as DemoViewModel)?.Content;
			//demoContent.Content = (demoList.SelectedItem as DemoViewModel)?.Content;

		}

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}

			GameServices.InputManager.Update(gameTime);
			GameServices.UIManager.Update(gameTime);
			//_guiSystem.Update(gameTime);

			GameServices.GridWorld.Update(gameTime);

			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.SteelBlue);

			GameServices.GridWorld.Draw(_spriteBatch);
			gamePanel.renderTarget = GameServices.GridWorld.RenderTarget;

			_spriteBatch.Begin();

			GameServices.UIManager.Draw(_spriteBatch);

			_spriteBatch.DrawString(GameServices.Fonts["Calibri"], GameServices.InputManager.CurrentMouse.Position.ToString(), new Vector2(20, 200), Color.White);

			_spriteBatch.End();

			//_guiSystem.Draw(gameTime);

			base.Draw(gameTime);
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
