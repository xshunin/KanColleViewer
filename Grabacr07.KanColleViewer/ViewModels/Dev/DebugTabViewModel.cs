using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleViewer.Model;

namespace Grabacr07.KanColleViewer.ViewModels.Dev
{
	public class DebugTabViewModel : TabItemViewModel
	{
		public DebugTabViewModel()
		{
			this.Name = "Debug";
		}


		public void Notify()
		{
			WindowsNotification.Notifier.Show("Test", "This is a test notification.", () => App.ViewModelRoot.Activate());
		}

	}
}
