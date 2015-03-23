using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using ZKK_App.places;

namespace ZKK_App.interfaces
{
	public partial class PlacesPage : TabbedPage
	{
		public PlacesPage ()
		{
			//InitializeComponent ();

            
            PlacesDB DB = new PlacesDB();
            IEnumerable<PlaceItem> liste = DB.GetItems();

            foreach (PlaceItem item in liste)
            {
                PlaceItemPage p = new PlaceItemPage(item);
                Children.Add(p);
            }
            
            
            Title = "Orte";

            
		}
	}
}
