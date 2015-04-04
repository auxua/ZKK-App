using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using Xamarin.Forms;

namespace ZKK_App.interfaces
{
	public partial class AboutPage : ContentPage
	{
		public AboutPage ()
		{
			//InitializeComponent ();

            string debugText = "";
#if DEBUG
            debugText = " DEBUG ";
#endif

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
                        Text = "Version: "+Assembly.GetExecutingAssembly().GetName().Version+debugText+"\n"
                    },
                    new Label 
                    {
                        Text = "\n Das ZKK-Logo wurde kreiert von Martin Bellgardt. (lizenziert unter CC-BY-SA) \n \nDie App wurde entwickelt von Arno Schmetz. \n \nDer Quellcode ist OpenSource und zu finden unter https://github.com/auxua/ZKK-App",
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
