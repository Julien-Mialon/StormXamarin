using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Storm.Mvvm.Inject;
using TestApp.Business;

namespace TestApp.Tablet
{
	static class Bootstrap
	{
		public static void Initialize(Frame frame)
		{
			Dictionary<string, Type> views = new Dictionary<string, Type>
			{
				{Views.NAVIGATION_PAGE, typeof(NavigationPage)}
			};

			Dictionary<string, Type> dialogs = new Dictionary<string, Type>
			{
				{Dialogs.NAVIGATION_DIALOG, typeof(DialogPage)},
				{Dialogs.MAIN, typeof(MainDialog)}
			};

			var container = WindowsContainer.CreateInstance<Container>(frame, views, dialogs);
			ViewModelsLocator.Initialize(container);
		}
	}
}
