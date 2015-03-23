using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ZKK_App.aklist
{
    /// <summary>
    /// A Page containing a list with the workshops of this Day
    /// </summary>
    class AKlistDayPage : ContentPage
    {

        
        public AKlistDayPage(IEnumerable<AKlistItem> list, string title)
        {
            
            // create a listview
            ListView listview = new ListView();

            // TODO: Fix this for long texts (texts can overflow into next view!)
            //listview.RowHeight = 120;
            // Xamarin Forms 1.4 Update!
            listview.HasUnevenRows = true;
            // set the cell data template
            listview.ItemTemplate = new DataTemplate(typeof(AKlistItemCell));

            // HACK: workaround issue #894 for now
            if (Device.OS == TargetPlatform.iOS)
                listview.ItemsSource = new string[1] { "" };


            // Perform Data Binding
            listview.ItemsSource = list;

            Title = title;

            Content = listview;
        }
    }
}
