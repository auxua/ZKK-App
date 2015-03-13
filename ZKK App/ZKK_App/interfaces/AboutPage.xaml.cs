using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ZKK_App.interfaces
{
	public partial class AboutPage : ContentPage
	{
		public AboutPage ()
		{
			//InitializeComponent ();

            StackLayout stack = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Label 
                    {
                        Text = "Die ZKK-App",
                        FontAttributes = FontAttributes.Bold,
                        FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label)),
                        XAlign = TextAlignment.Center
                    },
                    new Label 
                    {
                        Text = "Das ZKK-Logo wurde kreiert von Martin Bellgardt. \nDie App wurde entwickelt von Arno Schmetz. \nDer Quellcode ist OpenSource und zu finden unter https://github.com/auxua/ZKK_App",
                        FontAttributes = FontAttributes.None,
                        FontSize = Device.GetNamedSize(NamedSize.Medium,typeof(Label)),
                        XAlign = TextAlignment.Center
                    }
                }
            };

            Title = "Über diese App";

            Content = stack;
		}
	}
}
