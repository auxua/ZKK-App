using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ZKK_App.interfaces
{
	public partial class BillingPage : ContentPage
	{
		public BillingPage ()
		{
			InitializeComponent ();

            StackLayout stack = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    new Label {
                        Text = "Warte auf API-Zugriff..."
                    }
                }
            };

            Title = "Kasse des Vertrauens";

            Content = stack;
		}
	}
}
