using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace ZKK_App.aklist
{
    /// <summary>
    /// A Page containing a list with the workshops of this Day
    /// </summary>
    class AKlistDayPage : ContentPage
    {
        /// <summary>
        /// The Listview on the page, containing the Workshops
        /// Public to outside to enable children alterning the item Source (for destructive actions)
        /// </summary>
        public ListView listview;
        
        /// <summary>
        /// Creates a page with the workshops of this day
        /// </summary>
        /// <param name="list">A list of the workshops for this day</param>
        /// <param name="title">The Page title (usually the Day)</param>
        /// <param name="isDestrucive">true, if only Workshops of intereset should be shown, enabling (only) destructive user actions</param>
        public AKlistDayPage(ObservableCollection<AKlistItem> list, string title, Boolean isDestrucive = false)
        {
            ZKK_App.akinterest.LikeManagement.LoadAKLikes();

            // create a listview
            listview = new ListView();

            // Xamarin Forms Update!
            listview.HasUnevenRows = true;
            // set the cell data template
            if (isDestrucive)
            {
                listview.ItemTemplate = new DataTemplate(typeof(AKListItemCellDestructiveOnly));
            }
            else
            {
                listview.ItemTemplate = new DataTemplate(typeof(AKlistItemCell));
            }

            // HACK: workaround issue #894 for now
            if (Device.OS == TargetPlatform.iOS)
            {
                listview.ItemsSource = new string[1] { "" };
                Icon = "cal.png";
            }
            // Different Context-Actions for iOS and WP/Android
            if (Device.OS == TargetPlatform.iOS)
            {
                listview.Header = "Einträge zur Seite wischen für weitere Optionen";
            }
            else
            {
                listview.Header = "Einträge gedrückt halten für weitere Optionen";
            }


            // Perform Data Binding
            listview.ItemsSource = list;

            Title = title;

            Content = listview;
        }
    }
}
