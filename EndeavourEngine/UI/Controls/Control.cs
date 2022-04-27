using Endeavour.Input;
using Endeavour.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Endeavour.UI
{
	public enum DockStyle
	{
		None,
		Fill,
	}

	public struct Margin
	{
		public int Left;
		public int Right;
		public int Top;
		public int Bottom;

		public Margin() : this(0, 0, 0, 0)
		{ }

		public Margin(int all) : this(all, all, all, all)
		{ }

		public Margin(int left, int right, int top, int bottom)
		{
			Left = left;
			Right = right;
			Top = top;
			Bottom = bottom;
		}
	}

	public abstract class Control : Interfaces.IDrawable, Interfaces.IUpdateable
	{
		public Control(Rectangle bounds)
		{
			RelativeBounds = bounds;
		}

		public event EventHandler MouseDownEH;
		public event EventHandler MouseUpEH;
		public event EventHandler MouseClickEH;
		public event EventHandler DragEH;
		public event EventHandler DragBeginEH;
		public event EventHandler DragEndEH;

		public void OnMouseDown() => MouseDownEH?.Invoke(this, new EventArgs());
		public void OnMouseUp() => MouseUpEH?.Invoke(this, new EventArgs());
		public void OnMouseClick() => MouseClickEH?.Invoke(this, new EventArgs());
		public void OnDrag() => DragEH?.Invoke(this, new EventArgs());
		public void OnDragBegin()
		{
			DragBeginPoint = GameServices.InputManager.CurrentMouse.Position;
			DragBeginEH?.Invoke(this, new EventArgs());
		}
		public void OnDragEnd()
		{
			DragEndEH?.Invoke(this, new EventArgs());
			DragBeginPoint = Point.Zero;
		}

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

		public Window ParentWindow => GetParentWindow(this);

		public static Window GetParentWindow(Control current)
		{
			var parent = current.Parent;

			if (parent is null)
				return null;

			if (parent is Window window)
			{
				return window;
			}
			return GetParentWindow(parent);
		}

		public Image BackgroundImage { get; set; } = null;

		public Rectangle RelativeBounds;
		public Rectangle AbsoluteBounds
			=> new Rectangle(AbsoluteLocation, RelativeBounds.Size);

		public BorderStyle BorderStyle;

		public int Width => AbsoluteBounds.Width;
		public int Height => AbsoluteBounds.Height;

		public Margin Margin = new(3);

		private List<Control> controls = new();
		public IReadOnlyCollection<Control> Controls => controls.AsReadOnly();

		public Control? Parent;

		public int ZIndex
		{
			get => zIndex;
			set
			{
				if (!LockZIndex)
				{
					zIndex = value;
				}
			}
		}
		int zIndex = 0;

		public bool LockZIndex { get; set; } = false;

		public DockStyle DockStyle { get; set; } = DockStyle.None;

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
					BackgroundImage.Draw(sb, AbsoluteBounds);

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

			if (BorderStyle.Thickness > 0)
			{
				sb.DrawRectangle(AbsoluteBounds, BorderStyle.Color, BorderStyle.Thickness);
			}

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

		public virtual void HandleInput()
		{
			IsHovering = ContainsMouse;
			IsPressed = ContainsMouse && GameServices.InputManager.IsMouseButtonPressed(MouseButtons.LeftButton);

			var input = GameServices.InputManager;

			if (IsHovering && input.IsNewMousePress(MouseButtons.LeftButton))
			{
				OnMouseDown();
			}

			if (IsHovering && input.IsNewMouseRelease(MouseButtons.LeftButton))
			{
				OnMouseUp();
				OnMouseClick();
			}

			if (IsHovering && input.IsNewMousePress(MouseButtons.LeftButton))
			{
				OnDragBegin();
			}

			if (input.IsMouseButtonPressed(MouseButtons.LeftButton) && DragBeginPoint != Point.Zero)
			{
				OnDrag();
			}

			if (input.IsNewMouseRelease(MouseButtons.LeftButton))
			{
				OnDragEnd();
			}
		}

		public virtual void Update(GameTime gameTime)
		{
			if (DockStyle == DockStyle.Fill && Parent is not null)
			{
				var newWidth = Parent.Width - Parent.Margin.Left - Parent.Margin.Right;
				var newHeight = Parent.Height - Parent.Margin.Top - Parent.Margin.Bottom;
				RelativeBounds = new Rectangle(Parent.Margin.Left, Parent.Margin.Top, newWidth, newHeight);
			}

			foreach (var c in Controls.Where(c => c.Enabled))
			{
				c.Update(gameTime);
			}

			// remove controls we don't need
			controls = controls.Where(c => !c.ShouldDispose).ToList();
		}
	}
}
