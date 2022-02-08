using Metakinisi.UI.Controls;
using Microsoft.Xna.Framework;

namespace Metakinisi.UI
{
	public class VehicleManagementWindow : Window
	{
		ListBox<Vehicle> vehicleListBox;

		public VehicleManagementWindow(Rectangle bounds, string title) : base(bounds, title)
		{
			var squareButton = new Rectangle(4, titleBar.Height + 4, 32, 32);

			vehicleListBox = new ListBox<Vehicle>(new Rectangle(50, titleBar.Height, bounds.Width - 50, bounds.Height - titleBar.Height))
			{
				Items = GameServices.GridWorld.GameState.Vehicles,
				Font = GameServices.Fonts["Calibri"],
				ForeColor = Color.Black,
				BackColor = Color.Azure,
			};
			AddControl(vehicleListBox);

			var addVehicleButton = new Button(squareButton, "Add", () =>
			{
				GameServices.GridWorld.GameState.Vehicles.Add(new Vehicle(new Point3(10, 10, 0), 0.5f, $"vehicle{GameServices.GridWorld.GameState.Vehicles.Count}"));
			});
			AddControl(addVehicleButton);

			squareButton.Offset(0, 32);
			var deleteVehicleButton = new Button(squareButton, "Delete", () =>
			{
				if (vehicleListBox.SelectedElement is Vehicle v)
				{
					GameServices.GridWorld.GameState.Vehicles.Remove(v);
				}
			});
			AddControl(deleteVehicleButton);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			vehicleListBox.Items = GameServices.GridWorld.GameState.Vehicles;
		}
	}
}
