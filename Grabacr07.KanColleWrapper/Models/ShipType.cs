using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
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
			get
			{
				try
				{
					XDocument XML = XDocument.Load("Translations\\ShipTypes.xml");
					var Translations = XML.Descendants("Type");
					var FoundTranslation = Translations.Where(b => b.Element("JP-Name").Value.Equals(RawData.api_name));

					foreach (XElement el in FoundTranslation)
						return el.Element("TR-Name").Value;

					// Translation not found! Stick it onto the XML file for future translations.
                    XML.Root.Add(new XElement("Type",
							new XElement("JP-Name", RawData.api_name),
							new XElement("TR-Name", RawData.api_name)
						));
                    
					XML.Save("Translations\\ShipTypes.xml");
				}
				catch { }

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
