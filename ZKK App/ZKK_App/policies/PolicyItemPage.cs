using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ZKK_App.policies
{
    public class PolicyItemPage : ContentPage
    {
        public PolicyItemPage(PolicyItem p)
        {
            Label titleLabel = new Label {
                YAlign = TextAlignment.Center,
                XAlign = TextAlignment.Center,
                TextColor = Color.FromHex("00549F"),
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                Text = p.Title

            };
            //titleLabel.SetBinding(Label.TextProperty,"Title");
            
            Label commentLabel = new Label
            {
                YAlign = TextAlignment.Center,
                XAlign = TextAlignment.Start,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                Text = p.Text
            };
            //commentLabel.SetBinding(Label.TextProperty, "Comment");


            var layout = new StackLayout
            {
                Padding = new Thickness(20, 0, 20, 0),
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                Children = { titleLabel, commentLabel },
                VerticalOptions = LayoutOptions.Center
            };


            Title = p.Title;

            // Create a Scrollview to allow Scrolling on the Page
            ScrollView sc = new ScrollView();
            sc.Orientation = ScrollOrientation.Vertical;
            sc.Content = layout;

            Content = sc;
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
