using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using ZKK_App.akinterest;
using ZKK_App.aklist;

namespace ZKK_App.interfaces
{
	public partial class AKLikesPage : TabbedPage
	{
		public AKLikesPage ()
		{
			//InitializeComponent ();

            AKlistDBFull db = new AKlistDBFull();
            Title = "Stundenplan";
            // Create pages and provide data
            var liste = db.GetDays();
            foreach (string day in db.GetDays())
            {
                AKlistDayPage daypPage = new AKlistDayPage(db.GetItems(day), day);
                Children.Add(daypPage);
            }

            if (liste.Count() < 1)
            {
                Device.BeginInvokeOnMainThread(() => { DisplayAlert("Keine AKs", "Keine AKs als interessant markiert. Einfach in der AK-Liste die entsprechenden AKs markieren. (iOS: zur Seite Wischen, WP/Android: Gedrückt halten)", "Danke!"); });
            }
		}
	}
}
