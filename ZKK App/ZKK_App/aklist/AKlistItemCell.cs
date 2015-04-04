using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ZKK_App.aklist
{

    public class AKlistItemCell : ViewCell
    {

        Label likeLabel;
        Label titleLabel;

        Boolean DestructiveOnly;

        public AKlistItemCell() : this(false) { }

        public AKlistItemCell(Boolean onlyDestructive = false)
        {
            this.DestructiveOnly = onlyDestructive;
            
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

            titleLabel = new Label {
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

            Label temp = new Label
            {
                Text = " "
            };

            // Create Context-Menu
            if (!onlyDestructive)
            {
                var LikeAction = new MenuItem { Text = "Interessant" };
                LikeAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
                LikeAction.Clicked += OnLikeClicked;
                ContextActions.Add(LikeAction);
            }
            var UnlikeAction = new MenuItem { Text = "Uninteressant", IsDestructive = true };
            UnlikeAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
            UnlikeAction.Clicked += OnUnlikeClicked;
            // add context actions to the cell
            ContextActions.Add(UnlikeAction);

            var layout = new StackLayout
            {
                Padding = new Thickness(20, 0, 20, 0),
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Fill,
                Children = { dateLabel, titleLabel, detailLabel, temp, likeLabel, temp }
            };
            View = new Frame { Content = layout };
                
        }

        async void OnLikeClicked(object sender, EventArgs e)
        {
            likeLabel.Text = "Interesse bekundet\n";
            ((List<String>)Application.Current.Properties[Settings.PropertyLikesList]).Add(titleLabel.Text);
            await Application.Current.SavePropertiesAsync();
            ZKK_App.akinterest.LikeManagement.SaveAKLikes();
        }

        async void OnUnlikeClicked(object sender, EventArgs e)
        {
            likeLabel.Text = " \n";
            ((List<String>)Application.Current.Properties[Settings.PropertyLikesList]).Remove(titleLabel.Text);
            await Application.Current.SavePropertiesAsync();
            ZKK_App.akinterest.LikeManagement.SaveAKLikes();
            if (DestructiveOnly)
            {
                this.View.IsVisible = false;
                this.View.IsEnabled = false;
                this.IsEnabled = false;
                
            }
        }

        protected override void OnBindingContextChanged()
        {
            // Fixme : this is happening because the View.Parent is getting 
            // set after the Cell gets the binding context set on it. Then it is inheriting
            // the parents binding context.
            //View.BindingContext = BindingContext;
            base.OnBindingContextChanged();
            List<String> list = Application.Current.Properties[Settings.PropertyLikesList] as List<String>;
            try
            {
                if (list.Contains(titleLabel.Text))
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
                likeLabel.Text = " Konnte Daten nciht laden: " + e.Message + list.ToString();
            }
        }
    }

    public class AKListItemCellDestructiveOnly : AKlistItemCell
    {
        public AKListItemCellDestructiveOnly() : base(true) { }
    }
}
