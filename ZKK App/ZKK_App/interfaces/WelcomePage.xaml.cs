using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ZKK_App.interfaces
{
    public partial class WelcomePage : ContentPage
	{
		public WelcomePage ()
		{
			//InitializeComponent ();

            Label titleLabel = new Label
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Text = "Willkommen bei der ZKK-APP",
            };

            Label contentLabel = new Label
            {
                FontAttributes = FontAttributes.None,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.StartAndExpand
            };
            // Not nice, but we create the page by code because of a bug in Xamarin that comes up sometimes about InitalizeComponent
            contentLabel.Text = "Dies ist die ZKK-App.\n\n  In der Navigation sind die verschiedenen Bereiche zu finden.\n\n  Im Bereich Update kann ein Update aller Daten gemacht werden, die dann auch Offline verfügbar sind. Die News können auch seperat mittels Pull-To-Refresh aktualisiert werden. Aus technischen Gründen gibt es derzeit keine Beschreibungstexte bei den ZaPF-AKs.\n\n  Außerdem gibt es eine Liste der AKs der Konferenzen und im Laufe der Konferenz auch die entsprechenden AK-Pläne. Dort kann jeweils auch ein AK als interessant markiert werden, wodurch er im eigenen Stundenplan auftaucht.\n\n  Bei Problemen mit der App einfach bei Arno (auX) melden. Diese App übernimmt keinen Anspruch auf Vollständigkeit und Korrektheit. Im Zweifelsfall gilt, was die Orga sagt.\n\n            Viel Spaß mit der App bei der ZKK!";

            StackLayout stack = new StackLayout
            {
                Spacing = 50,
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Children = { titleLabel, contentLabel }
            };

            Title = "Willkommen";

            ScrollView scroll = new ScrollView
            {
                Orientation = ScrollOrientation.Vertical,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Content = stack
            };

            Padding = new Thickness(30, 20, 20, 0);

            Content = scroll;
		}
	}
}
