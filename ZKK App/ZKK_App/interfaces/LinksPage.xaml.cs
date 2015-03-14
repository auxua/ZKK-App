using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using ZKK_App.links;

namespace ZKK_App.interfaces
{
	public partial class LinksPage : ContentPage
	{
		public LinksPage ()
		{
			//InitializeComponent ();

            LinksDB DB = new LinksDB();

            ListView listView = new ListView();


            listView.ItemTemplate = new DataTemplate(typeof(TextCell));
            listView.ItemTemplate.SetBinding(TextCell.TextProperty, "Title");
            listView.ItemTemplate.SetBinding(TextCell.DetailProperty, "Link");

            listView.ItemsSource = DB.GetItems();
            listView.VerticalOptions = LayoutOptions.CenterAndExpand;
            listView.HorizontalOptions = LayoutOptions.CenterAndExpand;
            //listView.BindingContext = items;

            listView.ItemSelected += (sender, e) => openURL(((LinksItem)e.SelectedItem).Link);
            
            Title = "Links (Antippen zum Öffnen)";

            // Create a Scrollview to allow Scrolling on the Page
            Content = listView;
        }

        private void openURL(string det)
        {
            Uri uri = new Uri(det);
            Device.OpenUri(uri);
        }

        protected override void OnBindingContextChanged()
        {
            // Fixme : this is happening because the View.Parent is getting 
            // set after the Cell gets the binding context set on it. Then it is inheriting
            // the parents binding context.
            //View.BindingContext = BindingContext;
            base.OnBindingContextChanged();
		}
	}
}
