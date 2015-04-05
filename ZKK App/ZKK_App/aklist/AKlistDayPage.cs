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

        public ListView listview;
        
        public AKlistDayPage(ObservableCollection<AKlistItem> list, string title, Boolean isDestrucive = false)
        {
            ZKK_App.akinterest.LikeManagement.LoadAKLikes();

            // create a listview
            listview = new ListView();

            // TODO: Fix this for long texts (texts can overflow into next view!)
            //listview.RowHeight = 120;
            // Xamarin Forms 1.4 Update!
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
