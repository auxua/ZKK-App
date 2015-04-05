using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Xamarin.Forms;

namespace ZKK_App
{
	public class App : Application
	{
		public App ()
		{
            // On First start, copy the data from Assets to Personal storage
            
            // Check Configuration and load default values on first start
            // Disabled, because not used at the moment
            /*if (!Application.Current.Properties.ContainsKey(Settings.PropertyConferenceSelection))
            {
                // Not defined conference!
                Application.Current.Properties[Settings.PropertyConferenceSelection] = Conferences.ALL.ToString();
                // Not defined the News-Source (Default: Online)
                Application.Current.Properties[Settings.PropertyNewsSource] = "Online";
            }*/

            // First satrt, copy assets if needed
            DependencyService.Get<IPersonalStorage>().CopyAssets();
            
            
            if (!Application.Current.Properties.ContainsKey(Settings.PropertyUpdateDate))
            {
                // Set property - "Datenstand" is the version/date of the data
                Application.Current.Properties[Settings.PropertyUpdateDate] = "Initialzustand";
            }
            MainPage = new RootPage();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
            // Handle when your app sleeps
            // Removed manual Persistency handling

		}


		protected override void OnResume ()
		{
			// Handle when your app resumes
            // Removed manual Persistency handling
		}
	}
}
