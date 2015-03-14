using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using ZKK_App.aks;

namespace ZKK_App.interfaces
{
	public partial class AKPage : TabbedPage
	{
		public AKPage ()
		{
			//InitializeComponent ();
            AKDB db;

            // Try to create the Database. On Failure, Inform the user and abort
            try
            {
                db = new AKDB();
            }
            catch (Exception)
            {
                DisplayAlert("Datenfehler", "die AKs konnten nciht aus den Dateien geladen werden. Bitte ein Update durchführen!","Ok, mach ich");
                return;
            }

            Children.Add(new AKItemPage(db.GetItems(Conferences.ZAPF), "ZaPF-AKs"));
            Children.Add(new AKItemPage(db.GetItems(Conferences.KIF), "KIF-AKs"));
            Children.Add(new AKItemPage(db.GetItems(Conferences.KOMA), "KoMa-AKs"));
            Children.Add(new AKItemPage(db.GetItems(Conferences.ALL), "gemeinsame AKs"));

            Title = "AK-Liste";
		}
	}
}
