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

            // Create pages and provide data
            foreach (string day in db.GetDays())
            {
                AKlistDayPage daypPage = new AKlistDayPage(db.GetItems(day), day);
                Children.Add(daypPage);
            }
		}
	}
}
