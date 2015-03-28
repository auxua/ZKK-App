using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ZKK_App.places
{
    public class PlaceItemPage : ContentPage
    {
        public PlaceItemPage(PlaceItem p)
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

            Label detailLabel = new Label
            {
                YAlign = TextAlignment.Center,
                XAlign = TextAlignment.Center,
                FontAttributes = Xamarin.Forms.FontAttributes.Italic,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                Text = p.Rooms
            };
            //detailLabel.SetBinding(Label.TextProperty, "Rooms");

            Label commentLabel = new Label
            {
                YAlign = TextAlignment.Center,
                XAlign = TextAlignment.Start,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                Text = p.Comment
            };
            //commentLabel.SetBinding(Label.TextProperty, "Comment");

            string path = DependencyService.Get<IPersonalStorage>().GetFullFilePath(p.Imagename);


            // TODO: Optimize Size of the image...
            Image image = new Image
            {
                MinimumHeightRequest = 200,
                MinimumWidthRequest = 200,
                VerticalOptions = LayoutOptions.Start,
                HeightRequest = 500,
                Source = ImageSource.FromFile(path)
            };
            //image.SetBinding(Image.SourceProperty, "Imagename");

            var layout = new StackLayout
            {
                Padding = new Thickness(20, 0, 20, 0),
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                Children = { titleLabel, image, detailLabel, commentLabel },
                VerticalOptions = LayoutOptions.Center
            };

            Title = p.Title;

            if (Device.OS == TargetPlatform.iOS)
            {
                Icon = "house.png";
            }

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
