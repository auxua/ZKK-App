using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ZKK_App.aks
{
    public class AKItemCell : ViewCell
    {
        // create some properties for the Data Binding
        // Not used for now - There is a bug in WindowsPhone which results in very small rows...
        public static readonly BindableProperty AKNameProperty = BindableProperty.Create<AKItemCell, string> (w => w.AKName, default(string));

        public string AKName
        {
            get { return (string)GetValue(AKNameProperty); }
            set { SetValue(AKNameProperty, value); }
        }

        public static readonly BindableProperty AKDetailProperty = BindableProperty.Create<AKItemCell, string>(w => w.AKDetail, default(string));

        public string AKDetail
        {
            get { return (string)GetValue(AKDetailProperty); }
            set { SetValue(AKDetailProperty, value); }
        }

        public Boolean active = false;
        
        public AKItemCell()
        {
            titleLabel = new Label
            {
                YAlign = TextAlignment.Center,
                XAlign = TextAlignment.Center,
                TextColor = Color.FromHex("00549F"),
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))

            };
            titleLabel.SetBinding(Label.TextProperty, "Title");
            //titleLabel.SetBinding(Label.TextProperty, AKName);
            //titleLabel.SetBinding(Label.TextProperty, "AKName");
            //titleLabel.BindingContext = this;

            Label detailLabel = new Label
            {
                YAlign = TextAlignment.Center,
                XAlign = TextAlignment.Start,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
            };
            detailLabel.SetBinding(Label.TextProperty, "Detail");
            //detailLabel.SetBinding(Label.TextProperty, "AKDetail");
            //detailLabel.BindingContext = this;

            string path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("like.png");

            // Just a temporary testing solution
            Label temp = new Label
            {
                Text = "  "
            };

            likeLabel = new Label
            {
                YAlign = TextAlignment.Center,
                XAlign = TextAlignment.Center,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                FontAttributes = FontAttributes.Italic,
                TextColor = Color.Silver,
                Text = " \n"
            };

            /*Image image = new Image
            {
                MinimumHeightRequest = 30,
                MinimumWidthRequest = 30,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                HeightRequest = 50,
                WidthRequest = 50,
                Source = ImageSource.FromFile(path)
            };*/
            /*
            var layout = new StackLayout
            {
                Padding = new Thickness(20, 0, 20, 0),
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Children = { titleLabel }
            };*/
            
            // Create Context-Menu
            var LikeAction = new MenuItem { Text = "Interessant" };
            LikeAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            LikeAction.Clicked += OnLikeClicked;
            var UnlikeAction = new MenuItem { Text = "Uninteressant" };
            UnlikeAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            UnlikeAction.Clicked += OnUnlikeClicked;
            // add context actions to the cell
            ContextActions.Add(LikeAction);
            ContextActions.Add(UnlikeAction);

            var outerlayout = new StackLayout
            {
                Padding = new Thickness(20, 10, 5, 5),
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children = { titleLabel, detailLabel, temp, likeLabel, temp }
            };
            //View = new Frame { Content = outerlayout };
            View = outerlayout;
            //View = layout;
        }

        Label titleLabel;
        Label likeLabel;

        void OnLikeClicked(object sender, EventArgs e)
        {
            likeLabel.Text = "Interesse bekundet\n";
            akinterest.LikeManagement.AddLike(titleLabel.Text);
        }

        void OnUnlikeClicked(object sender, EventArgs e)
        {
            likeLabel.Text = " \n";
            akinterest.LikeManagement.RemoveLike(titleLabel.Text);
        }

        protected override void OnBindingContextChanged()
        {
            // Fixme : this is happening because the View.Parent is getting 
            // set after the Cell gets the binding context set on it. Then it is inheriting
            // the parents binding context.
            //View.BindingContext = BindingContext;
            base.OnBindingContextChanged();
            try
            {
                if (akinterest.LikeManagement.CheckName(titleLabel.Text))
                {
                    likeLabel.Text = "Interesse bekundet\n";
                }
                else
                {
                    likeLabel.Text = " \n";
                }
            } 
            catch (Exception e)
            {
                likeLabel.Text = " Konnte Daten nciht laden: "+e.Message;
            }
        }
    }
}
