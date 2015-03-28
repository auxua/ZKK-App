using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Xamarin.Forms;
using ZKK_App.aklist;

namespace ZKK_App.akinterest
{
    /// <summary>
    /// A class for managing the plans of the workshops
    /// they can be imported in the constructor from textfile and exported for AKlistItemCell-Views via methods
    /// Each conference can be loaded seperately in this Database. There will be no checks on overrides/import
    /// </summary>
    class AKlistDBFull
    {
        /// <summary>
        /// The Items are stored inthis database
        /// </summary>
        List<AKlistItem> database;

        /// <summary>
        /// Stores the names of the days that are existing
        /// </summary>
        private List<string> Days;

        /// <summary>
        /// A Database for the Policies of the conference
        /// 
        /// </summary>
        public AKlistDBFull()
        {
            // import from file
            // In this case, import from every file
            database = new List<AKlistItem>();

            string path ="";
            List<String> Sources = new List<string>();

            path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("aklist-zkk");
            Sources.Add(path);
            path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("aklist-kif");
            Sources.Add(path);
            path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("aklist-koma");
            Sources.Add(path);
            path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("aklist-zapf");
            Sources.Add(path);

            Days = new List<string>();
            LikeManagement.LoadAKLikes();
            List<String> likes = Application.Current.Properties[Settings.PropertyLikesList] as List<String>;

            String line;
            String[] separators = new String[] { Settings.TextFileDelimiter };

            foreach (string pa in Sources)
            {
                StreamReader sr = new StreamReader(pa);

                //Add all Items to DB
                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    if (words.Count() == 4)
                    {
                        // If this was not a workshop being liked, do not add it!
                        if (!likes.Contains(words[2]))
                        {
                            continue;
                        }
   
                        AKlistItem a = new AKlistItem();
                        a.Day = words[0];
                        a.Room = words[3];
                        a.Time = words[1];
                        a.Title = words[2];
                        database.Add(a);

                        if (!Days.Contains(a.Day))
                            Days.Add(a.Day);
                    }
                }
                // Close File, finished!
                sr.Close();
            }

            // Apply a sorting to the days. For this case, we can simply use string-sorting
            Days.Sort();
        }


        public IEnumerable<string> GetDays()
        {
            return Days;
        }

        public IEnumerable<AKlistItem> GetItems(string d)
        {
            return database.FindAll(item => item.Day == d);
        }
    }
}
