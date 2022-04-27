using Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metakinisi
{
	public struct GameState
	{
		public Map Map;
		public Graph2D RailGraph = new();
		public List<Vehicle> Vehicles = new();

		public GameState(Map map)
		{
			Map = map;
			RailGraph = new();
			Vehicles = new();
		}

		public GameState(Map map, Graph2D railGraph, List<Vehicle> vehicles)
		{
			Map = map;
			RailGraph = railGraph;
			Vehicles = vehicles;
		}
	}
}
