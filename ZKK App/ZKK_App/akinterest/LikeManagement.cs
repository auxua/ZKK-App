using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using Xamarin.Forms;

namespace ZKK_App.akinterest
{
    class LikeManagement
    {

        public static void SaveAKLikes()
        {
            
            List<String> list = Application.Current.Properties[Settings.PropertyLikesList] as List<String>;
            string path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("aklikes");

            StreamWriter sw = new StreamWriter(path,false);

            // Store elements line per line
            foreach (string s in list)
            {
                sw.WriteLine(s);
            }
            sw.Close();
        }

        public static void LoadAKLikes()
        {
            // If there is no List stored so far, then at least, create this list!
            Application.Current.Properties[Settings.PropertyLikesList] = new List<String>();
            
            
            List<String> list = Application.Current.Properties[Settings.PropertyLikesList] as List<String>;
            string path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("aklikes");

            if (!File.Exists(path)) return;

            StreamReader sr = new StreamReader(path);
            string s;

            while ((s = sr.ReadLine()) != null)
            {
                list.Add(s);
            }

            sr.Close();
        }
    }
}
