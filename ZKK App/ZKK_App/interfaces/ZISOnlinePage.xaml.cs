using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ZKK_App.interfaces
{
	public partial class ZISOnlinePage : ContentPage
	{
		public ZISOnlinePage ()
		{
			//InitializeComponent ();

            WebView webview = new WebView();
            webview.Source = Settings.NewsWebPage;

            Content = webview;
		}
	}
}
