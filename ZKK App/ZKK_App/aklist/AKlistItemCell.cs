using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ZKK_App.aklist
{
    public class AKlistItemCell : ViewCell
    {
        public AKlistItemCell()
        {
            Label dateLabel = new Label {
                YAlign = TextAlignment.Start,
                XAlign = TextAlignment.Start,
                FontSize = Device.GetNamedSize(NamedSize.Small,typeof(Label)),
#if __IOS__
            TextColor = Color.FromHex("262626")
#else            
            TextColor = Color.FromHex("DCDCDC")
#endif
            };
            dateLabel.SetBinding(Label.TextProperty,"Time");

            Label titleLabel = new Label {
                YAlign = TextAlignment.Center,
                XAlign = TextAlignment.Center,
                TextColor = Color.FromHex("00549F"),
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label))

            };
            titleLabel.SetBinding(Label.TextProperty,"Title");

            Label detailLabel = new Label
            {
                YAlign = TextAlignment.Center,
                XAlign = TextAlignment.Center,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
            };
            detailLabel.SetBinding(Label.TextProperty, "Room");

            var layout = new StackLayout
            {
                Padding = new Thickness(20, 0, 20, 0),
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Children = { dateLabel, titleLabel, detailLabel }
            };
            View = layout;
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
