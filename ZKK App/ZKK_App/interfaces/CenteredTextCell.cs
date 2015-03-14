using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ZKK_App.interfaces
{
    class CenteredTextCell : ViewCell
    {
        public CenteredTextCell()
        {
            Label titleLabel = new Label
            {
                YAlign = TextAlignment.Center,
                XAlign = TextAlignment.Center,
                TextColor = Color.FromHex("00549F"),
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))

            };
            titleLabel.SetBinding(Label.TextProperty, "Title");

            Label detailLabel = new Label
            {
                YAlign = TextAlignment.Center,
                XAlign = TextAlignment.Center,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
            };
            detailLabel.SetBinding(Label.TextProperty, "Detail");

            var layout = new StackLayout
            {
                Padding = new Thickness(20, 0, 20, 0),
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Children = { titleLabel, detailLabel }
            };
            View = layout;
        }
    }
}
