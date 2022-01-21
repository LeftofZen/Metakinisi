using Metakinisi.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Metakinisi.UI
{
	public abstract class Control : IDrawable, IUpdateable
	{
		public Control(Rectangle bounds)
		{
			RelativeBounds = bounds;
		}

		public Action OnMouseDown;
		public Action OnMouseUp;
		public Action OnClick;
		public Action OnDrag;
		public Action OnDragBegin;
		public Action OnDragEnd;

		public bool ShouldDisposeOnClose = false;
		public bool ShouldDispose = false;

		public bool IsHovering = false;
		public bool IsDragging => DragBeginPoint != Point.Zero;
		public bool IsPressed = false;

		public bool Visible { get; set; } = true;
		public bool Enabled { get; set; } = true;

		public bool DrawBackground { get; set; } = true;

		public string Name { get; set; } = "<unnamed>";

		public Color ForeColor { get; set; } = Color.DarkMagenta;
		public Color BackColor { get; set; } = Color.Magenta;

		public TilesetReference? BackgroundImage { get; set; } = null;

		public Rectangle RelativeBounds;
		public Rectangle AbsoluteBounds
			=> new Rectangle(AbsoluteLocation, RelativeBounds.Size);

		public BorderStyle BorderStyle;

		public int Width => AbsoluteBounds.Width;
		public int Height => AbsoluteBounds.Height;

		private List<Control> controls = new();
		public IReadOnlyCollection<Control> Controls => controls.AsReadOnly();

		public Control? Parent;

		public int ZIndex = 0;

		public bool ContainsMouse
			=> AbsoluteBounds.Contains(GameServices.InputManager.CurrentMouse.Position);

		public void AddControl(Control control)
		{
			control.Parent = this;
			controls.Add(control);
		}

		public bool RemoveControl(Control control)
		{
			control.Parent = null;
			return controls.Remove(control);
		}

		public Point RelativeLocation
		{
			get => RelativeBounds.Location;
			set
			{
				RelativeBounds.X = value.X;
				RelativeBounds.Y = value.Y;
			}
		}

		public Point GetAbsoluteLocation(Control? control)
		{
			if (control == null)
				return Point.Zero;

			var parentAbsolute = GetAbsoluteLocation(control.Parent);

			return new Point(
				control.RelativeLocation.X + parentAbsolute.X,
				control.RelativeLocation.Y + parentAbsolute.Y);
		}

		public Point AbsoluteLocation
			=> GetAbsoluteLocation(this);

		public virtual void Draw(SpriteBatch sb)
		{
			if (DrawBackground)
			{
				if (BackgroundImage == null)
				{
					sb.FillRectangle(AbsoluteBounds, BackColor);
				}
				else
				{
					var centre = new Vector2(BackgroundImage.Value.SourceRectangle.Width / 2f, BackgroundImage.Value.SourceRectangle.Height / 2f);
					var renderRect = AbsoluteBounds;
					renderRect.Offset(AbsoluteBounds.Width / 2f, AbsoluteBounds.Height / 2f);

					// image
					sb.Draw(
						GameServices.Textures[BackgroundImage.Value.TilesetName],
						renderRect,
						BackgroundImage.Value.SourceRectangle,
						Color.White,
						RotationHelpers.RotationAnglesForDrawing[BackgroundImage.Value.Rotation],
						centre,
						SpriteEffects.None,
						0f);

					// debug
					//sb.Draw(
					//	GameServices.Textures[BackgroundImage.Value.TilesetName],
					//	new Rectangle(0, 0, GameServices.Textures[BackgroundImage.Value.TilesetName].Width, GameServices.Textures[BackgroundImage.Value.TilesetName].Height),
					//	Color.White);

					//sb.DrawString(GameServices.Fonts["Calibri"], BackgroundImage.Value.SourceRectangle.ToString(), AbsoluteLocation.ToVector2(), Color.White);

					// debug
					//sb.DrawRectangle(AbsoluteBounds, Color.Blue, 3);
					//sb.DrawRectangle(BackgroundImage.Value.SourceRectangle, Color.Aqua, 3);
					//sb.DrawPoint(BackgroundImage.Value.SourceRectangle.Center.ToVector2(), Color.Yellow, 5);
				}
			}

			sb.DrawRectangle(AbsoluteBounds, BorderStyle.Color, BorderStyle.Thickness);

			foreach (var c in controls.Where(c => c.Visible).OrderBy(c => c.ZIndex))
			{
				c.Draw(sb);
			}

			// debug
			//sb.DrawString(GameServices.Fonts["Calibri"], RelativeLocation.ToString(), AbsoluteLocation.ToVector2(), Color.Purple);
			//sb.DrawString(GameServices.Fonts["Calibri"], AbsoluteLocation.ToString(), AbsoluteLocation.ToVector2() + new Vector2(0, 16), Color.Purple);
		}

		public Point DragBeginPoint;

		public void HandleInput(GameTime gameTime)
		{
			HandleInput();
			foreach (var v in Controls)
			{
				v.HandleInput();
			}
		}

		void DragBegin()
		{
			DragBeginPoint = GameServices.InputManager.CurrentMouse.Position;
			OnDragEnd?.Invoke();
		}

		void DragEnd()
		{
			DragBeginPoint = Point.Zero;
			OnDragEnd?.Invoke();
		}

		void Drag()
		{
			OnDrag?.Invoke();
		}

		public void HandleInput()
		{
			IsHovering = ContainsMouse;
			IsPressed = ContainsMouse && GameServices.InputManager.IsMouseButtonPressed(MouseButtons.LeftButton);

			var input = GameServices.InputManager;

			if (IsHovering && input.IsNewMousePress(MouseButtons.LeftButton))
			{
				OnMouseDown?.Invoke();
			}

			if (IsHovering && input.IsNewMouseRelease(MouseButtons.LeftButton))
			{
				OnMouseUp?.Invoke();
				OnClick?.Invoke();
			}

			if (IsHovering && input.IsNewMousePress(MouseButtons.LeftButton))
			{
				DragBegin();
			}

			if (input.IsMouseButtonPressed(MouseButtons.LeftButton) && DragBeginPoint != Point.Zero)
			{
				Drag();
			}

			if (input.IsNewMouseRelease(MouseButtons.LeftButton))
			{
				DragEnd();
			}
		}

		public virtual void Update(GameTime gameTime)
		{
			foreach (Control c in Controls.Where(c => c.Enabled))
			{
				c.Update(gameTime);
			}

			// remove controls we don't need
			controls = controls.Where(c => !c.ShouldDispose).ToList();
		}
	}
}
