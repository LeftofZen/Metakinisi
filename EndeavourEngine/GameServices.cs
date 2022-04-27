using Endeavour.Input;
using Endeavour.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Endeavour.Services
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

		#region Services

		static readonly Dictionary<Type, object> _services = new();

		public static void AddService<T>(T service)
		{
			if (service is null)
				throw new ArgumentException("cannot add null service");

			if (_services.ContainsKey(typeof(T)))
				throw new ArgumentException("service already exists");

			_services.Add(typeof(T), service);
		}

		public static T GetService<T>() => (T)_services[typeof(T)];

		#endregion
	}
}