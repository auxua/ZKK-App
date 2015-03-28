using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace ZKK_App.aks
{
    public class AKItemPage : ContentPage
    {
        public AKItemPage(IEnumerable<AKItem> items, string title)
        {
            ZKK_App.akinterest.LikeManagement.LoadAKLikes();
            
            string header;

            if (Device.OS == TargetPlatform.iOS)
            {
                header = "Einträge zur Seite wischen für weitere Optionen";
                Icon = "list.png";
            }
            else
            {
                header = "Einträge gedrückt halten für weitere Optionen";
            }
            
            // create a listview
            ListView listview = new ListView();
            // Xamarin Forms 1.4 Update!
            listview.HasUnevenRows = true;
            listview.Header = header;

            // TODO: Fix this for long texts (texts can overflow into next view!)
            //listview.RowHeight = 180;
            // set the cell data template
            listview.ItemTemplate = new DataTemplate(typeof(AKItemCell));
            //listview.ItemTemplate.SetBinding(AKItemCell.AKNameProperty, "Title");
            //listview.ItemTemplate.SetBinding(AKItemCell.AKDetailProperty, "Detail");


            // HACK: workaround issue #894 for now
            if (Device.OS == TargetPlatform.iOS)
                listview.ItemsSource = new string[1] { "" };


            // Perform Data Binding
            listview.ItemsSource = items;
            

            Title = title;

            Content = listview;

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
