using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using ZKK_App.news;

namespace ZKK_App.interfaces
{
	public partial class NewsPage : ContentPage
	{
		public NewsPage ()
		{
			InitializeComponent ();
            //Use the Online-Version?
            bool Online = Application.Current.Properties[Settings.PropertyNewsSource].Equals("Online");

            if (Online)
            {
                WebView webView = new WebView
                {
                    Source = new UrlWebViewSource
                    {
                        Url = Settings.NewsWebPage,
                    },
                    VerticalOptions = LayoutOptions.FillAndExpand
                };
                
                Content = webView;

                /*string path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("logo.png");

                //This block was for testing purpose only!

                Content = new Image
                {
                    Source = ImageSource.FromFile(path)
                };*/

            } 
            else
            {
                
                // create a listview
                ListView listview = new ListView();
                
                // TODO: Fix this for long texts (texts can overflow into next view!)
                listview.RowHeight = 140;
                // set the cell data template
                listview.ItemTemplate = new DataTemplate(typeof(NewsItemCell));
                
                // HACK: workaround issue #894 for now
                if (Device.OS == TargetPlatform.iOS)
                    listview.ItemsSource = new string[1] { "" };

                NewsDB db = new NewsDB();

                // Perform Data Binding
                listview.ItemsSource = db.GetItems();

                Content = listview;

            }

            Title = "News";
		}
	}
}
