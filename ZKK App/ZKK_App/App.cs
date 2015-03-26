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
            if (!Application.Current.Properties.ContainsKey(Settings.PropertyConferenceSelection))
            {
                // Not defined conference!
                Application.Current.Properties[Settings.PropertyConferenceSelection] = Conferences.ALL.ToString();
                // Not defined the News-Source (Default: Online)
                Application.Current.Properties[Settings.PropertyNewsSource] = "Online";
            }
            if (!Application.Current.Properties.ContainsKey(Settings.PropertyUpdateDate))
            {
                // First satrt, copy assets if needed
                DependencyService.Get<IPersonalStorage>().CopyAssets();
                // Set property - "Datenstand" is the version/date of the data
                Application.Current.Properties[Settings.PropertyUpdateDate] = "Initialzustand";
            }
            if (!Application.Current.Properties.ContainsKey(Settings.PropertyLikesList))
            {
                // Init the List of workshops, the user wants to visit
                Application.Current.Properties[Settings.PropertyLikesList] = new List<String>();
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
            // Save Settings by  writing to the config file
            String path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("config.txt");
            StreamWriter sw = new StreamWriter(path,false);
            // Save all significant Properties as strings

            // First, the Conference Setting
            sw.WriteLine(Settings.PropertyConferenceSelection + Settings.TextFileDelimiter + Application.Current.Properties[Settings.PropertyConferenceSelection]);
            // Now, Datenstand
            sw.WriteLine(Settings.PropertyUpdateDate + Settings.TextFileDelimiter + Application.Current.Properties[Settings.PropertyUpdateDate]);
            // Now, NewsSource
            sw.WriteLine(Settings.PropertyNewsSource + Settings.TextFileDelimiter + Application.Current.Properties[Settings.PropertyNewsSource]);

            // Tidy Up!
            sw.Flush();
            sw.Close();
		}


		protected override void OnResume ()
		{
			// Handle when your app resumes
            // Load Settings by reading the config file
            String path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("config.txt");
            StreamReader sr = new StreamReader(path);
            // Save all significant Properties as strings

            String line;
            String[] separators = new String[] { Settings.TextFileDelimiter };

            while ((line = sr.ReadLine()) != null)
            {
                string[] words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (words.Count() == 2)
                {
                    Application.Current.Properties[words[0]] = words[1];
                }
            }

            sr.Close();
		}
	}
}
