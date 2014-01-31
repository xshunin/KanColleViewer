using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper.Models.Raw;

namespace Grabacr07.KanColleWrapper.Models
{
	/// <summary>
	/// 艦種を表します。
	/// </summary>
	public class ShipType : RawDataWrapper<kcsapi_stype>, IIdentifiable
	{
		public int Id
		{
			get { return this.RawData.api_id; }
		}

		public string Name
		{
			//get { return this.RawData.api_name; }
			get
			{
				// Translate IJN ship types to the USN ones.
				if (RawData.api_name == "水上機母艦") { return string.Format("AV"); }
				if (RawData.api_name == "戦艦") { return string.Format("BB"); }
				if (RawData.api_name == "航空戦艦") { return string.Format("BBV"); }
				if (RawData.api_name == "重巡洋艦") { return string.Format("CA"); }
				if (RawData.api_name == "航空巡洋艦") { return string.Format("CAV"); }
				if (RawData.api_name == "軽巡洋艦") { return string.Format("CL"); }
				if (RawData.api_name == "重雷装巡洋艦") { return string.Format("CLT"); }
				if (RawData.api_name == "正規空母") { return string.Format("CV"); }
				if (RawData.api_name == "軽空母") { return string.Format("CVL"); }
				if (RawData.api_name == "駆逐艦") { return string.Format("DD"); }
				if (RawData.api_name == "潜水艦") { return string.Format("SS"); }
				if (RawData.api_name == "潜水空母") { return string.Format("SSV"); }
				if (RawData.api_name == "装甲空母") { return string.Format("CVB"); }
				if (RawData.api_name == "揚陸艦") { return string.Format("LHA"); }
				if (RawData.api_name == "海防艦") { return string.Format("DE"); }
				if (RawData.api_name == "超弩級戦艦") { return string.Format("B"); }
				if (RawData.api_name == "補給艦") { return string.Format("AE"); }

				return this.RawData.api_name;
			}
		}

		public int SortNumber
		{
			get { return this.RawData.api_sortno; }
		}

		public ShipType(kcsapi_stype rawData) : base(rawData) { }

		public override string ToString()
		{
			return string.Format("ID = {0}, Name = \"{1}\"", this.Id, this.Name);
		}

		#region static members

		private static readonly ShipType dummy = new ShipType(new kcsapi_stype
		{
			api_id = 999,
			api_sortno = 999,
			api_name = "不審船",
		});

		public static ShipType Dummy
		{
			get { return dummy; }
		}

		#endregion
	}
}
