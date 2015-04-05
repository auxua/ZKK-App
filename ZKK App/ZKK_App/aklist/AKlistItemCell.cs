using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace ZKK_App.aklist
{

    public class AKlistItemCell : ViewCell
    {
        /// <summary>
        /// This label is signaling the user whether the Workshop is liked or not
        /// </summary>
        Label likeLabel;

        /// <summary>
        /// The label containing the Workshop Title
        /// </summary>
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
                Padding = new Thickness(20, 10, 5, 5),
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.Fill,
                Children = { dateLabel, titleLabel, detailLabel, temp, likeLabel, temp }
            };
            //View = new Frame { Content = layout };
            View = layout;
        }

        /// <summary>
        /// User clicked the "liked"-button
        /// Add the Workshop to List of liked ones
        /// </summary>
        void OnLikeClicked(object sender, EventArgs e)
        {
            likeLabel.Text = "Interesse bekundet\n";
            akinterest.LikeManagement.AddLike(titleLabel.Text);
        }

        /// <summary>
        /// User clicked the "unlike" button
        /// remove from likes-List.
        /// In Destructive Mode remove this element from data source of the Page
        /// </summary>
        void OnUnlikeClicked(object sender, EventArgs e)
        {
            likeLabel.Text = " \n";
            // Remove from Like-List
            akinterest.LikeManagement.RemoveLike(titleLabel.Text);
            // For personal plan, remove element from ItemSource
            if (DestructiveOnly)
            {
                /*this.View.IsVisible = false;
                this.View.IsEnabled = false;
                this.IsEnabled = false;*/
                // Get the containing Page
                AKlistDayPage mpar = GetParentPage();
                // Get Item Source
                ObservableCollection<AKlistItem> items = mpar.listview.ItemsSource as ObservableCollection<AKlistItem>;
                AKlistItem item = null;
                // Find the corresponding element
                foreach (AKlistItem i in items)
                {
                    if (i.Title == titleLabel.Text)
                    {
                        item = i;
                        break;
                    }
                }
                // Not found? return (should never happen)
                if (item == null)
                {
                    return;
                }
                // Remove it from Source
                items.Remove(item);

            }
        }

        /// <summary>
        /// Traverses the Parent-Hierarchy finding the containing page
        /// </summary>
        private AKlistDayPage GetParentPage()
        {
            var mParent = this.Parent;

            while (mParent != null && mParent.GetType() != typeof(AKlistDayPage))
            {
                mParent = mParent.Parent;
            }

            if (mParent == null)
            {
                throw new Exception(
                    string.Format("FindParentPage: Parent {0} not found for element {1}", typeof(AKlistDayPage), this));
            }

            return (AKlistDayPage)(object)mParent;
        }

        protected override void OnBindingContextChanged()
        {
            // Fixme : this is happening because the View.Parent is getting 
            // set after the Cell gets the binding context set on it. Then it is inheriting
            // the parents binding context.
            //View.BindingContext = BindingContext;
            base.OnBindingContextChanged();
            CheckLike();
        }

        /// <summary>
        /// Updates/Refreshes the Likes and updates the likelabel
        /// </summary>
        private void CheckLike()
        {
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
                likeLabel.Text = " Konnte Daten nciht laden: " + e.Message;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.CheckLike();
        }

    }

    public class AKListItemCellDestructiveOnly : AKlistItemCell
    {
        public AKListItemCellDestructiveOnly() : base(true) { }
    }
}
