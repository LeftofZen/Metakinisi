using System;

namespace Metakinisi
{
	public static class Program
	{
		[STAThread]
		static void Main()
		{
			using var game = new MainGame();
			game.Run();
		}
	}
}
