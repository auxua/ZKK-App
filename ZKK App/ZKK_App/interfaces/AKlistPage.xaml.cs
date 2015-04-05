using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using ZKK_App.aklist;

namespace ZKK_App.interfaces
{
    /// <summary>
    /// A Tabbed page for the conference. 
    /// The Tabs correspond to the days of the workshops
    /// </summary>
	public partial class AKlistPage : TabbedPage
	{
		public AKlistPage (Conferences conf)
		{
			//InitializeComponent ();

            AKlistDB db = new AKlistDB(conf);

            // Create pages and provide data
            foreach (string day in db.GetDays())
            {
                AKlistDayPage daypPage = new AKlistDayPage(db.GetObservableItems(day),day);
                Children.Add(daypPage);
            }


		}
	}

    #region To allow navigation using typeof(), create some subclasses for the conferences

    public class AKListPageZapf : AKlistPage
    {
        public AKListPageZapf() : base(Conferences.ZAPF) 
        {
            Title = "AK-Plan ZaPF";
        }
    }

    public class AKListPageKif : AKlistPage
    {
        public AKListPageKif() : base(Conferences.KIF) 
        {
            Title = "AK-Plan KIF";
        }
    }

    public class AKListPageKoma : AKlistPage
    {
        public AKListPageKoma() : base(Conferences.KOMA) 
        {
            Title = "AK-Plan KoMa";
        }
    }

    public class AKListPageZKK : AKlistPage
    {
        public AKListPageZKK() : base(Conferences.ALL) 
        {
            Title = "Plan gemeinsamer AKs";
        }
    }

    #endregion
}
