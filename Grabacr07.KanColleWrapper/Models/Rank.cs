using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grabacr07.KanColleWrapper.Models
{
	public static class Rank
	{
		public static string GetName(int rank)
		{
			switch (rank)
			{
				case 1:
					return "Marshal Admiral";
				case 2:
					return "Admiral";
				case 3:
					return "Vice-Admiral";
				case 4:
					return "Rear-Admiral";
				case 5:
					return "Captain";
				case 6:
					return "Commander";
				case 7:
					return "Novice Commander";
				case 8:
					return "Lt. Commander";
				case 9:
					return "Lieutenant";
				case 10:
				default:
					return "Novice Lieutenant";
			}
		}
	}
}
