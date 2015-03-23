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
            ListView listView = new ListView();


            listView.ItemTemplate = new DataTemplate(typeof(interfaces.CenteredTextCell));
            listView.ItemTemplate.SetBinding(TextCell.TextProperty, "Title");
            listView.ItemTemplate.SetBinding(TextCell.DetailProperty, "Detail");
            //listView.RowHeight = 120;
            listView.HasUnevenRows = true;

            listView.ItemsSource = items;
            //listView.VerticalOptions = LayoutOptions.CenterAndExpand;
            //listView.HorizontalOptions = LayoutOptions.CenterAndExpand;
            //listView.BindingContext = items;
            
            
            Title = title;

            // Create a Scrollview to allow Scrolling on the Page
            Content = listView;
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
