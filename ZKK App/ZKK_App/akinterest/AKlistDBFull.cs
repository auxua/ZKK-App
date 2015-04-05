using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// The Items are stored in this database
        /// </summary>
        List<AKlistItem> database;
        //ObservableCollection<AKlistItem> database;

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
            //database = new ObservableCollection<AKlistItem>();

            string path ="";
            // A List of the sources of data
            List<String> Sources = new List<string>();

            // common Workshops
            path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("aklist-zkk");
            Sources.Add(path);
            // KIF-Workshops
            path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("aklist-kif");
            Sources.Add(path);
            // KoMa-Workshops
            path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("aklist-koma");
            Sources.Add(path);
            // ZaPf Workshops
            path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("aklist-zapf");
            Sources.Add(path);

            // List of all Days bein used
            Days = new List<string>();
            // Load the Likes of the user
            LikeManagement.LoadAKLikes();
            System.Collections.ObjectModel.ObservableCollection<String> likes = LikeManagement.GetList();

            String line;
            String[] separators = new String[] { Settings.TextFileDelimiter };

            // Add data from all sources
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
            // Sort the whole DB
            database.Sort();
        }

        /// <summary>
        /// Returns the List of the Days existing in the database
        /// </summary>
        public IEnumerable<string> GetDays()
        {
            return Days;
        }

        /// <summary>
        /// Returns all Items corresponding to the Day d
        /// </summary>
        public IEnumerable<AKlistItem> GetItems(string d)
        {
            return database.FindAll(item => item.Day == d);
        }

        /// <summary>
        /// Returns all Items corresponding to the Day d.
        /// Creates an Observable Collection for bindings
        /// </summary>
        public ObservableCollection<AKlistItem> GetObservableItems(string d)
        {
            return new ObservableCollection<AKlistItem>(this.GetItems(d));
        }
    }
}
