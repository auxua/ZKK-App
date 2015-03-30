using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using ZKK_App.policies;

namespace ZKK_App.interfaces
{
	public partial class PolicyPage : TabbedPage
	{
		public PolicyPage ()
		{
			//InitializeComponent ();

            PolicyDB DB = new PolicyDB();
            IEnumerable<PolicyItem> liste = DB.GetItems();

            foreach (PolicyItem item in liste)
            {
                PolicyItemPage p = new PolicyItemPage(item);
                Children.Add(p);
            }


            Title = "Gemeinschaftsstandards";
		}
	}
}
