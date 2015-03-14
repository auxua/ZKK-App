using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Xamarin.Forms;

namespace ZKK_App.aklist
{
    /// <summary>
    /// A class for managing the plans of the workshops
    /// they can be imported in the constructor from textfile and exported for AKlistItemCell-Views via methods
    /// Each conference can be loaded seperately in this Database. There will be no checks on overrides/import
    /// </summary>
    class AKlistDB
    {
        List<AKlistItem> database;

        /// <summary>
        /// A Database for the Policies of the conference
        /// 
        /// </summary>
        public AKlistDB(Conferences conf)
        {
            //import from file

            database = new List<AKlistItem>();

            string path ="";

            // Decide on Conference, what to import
            switch (conf)
            {
                case Conferences.ALL:
                    path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("aklist-zkk");
                    break;
                case Conferences.KIF:
                    path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("aklist-kif");
                    break;
                case Conferences.KOMA:
                    path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("aklist-koma");
                    break;
                case Conferences.ZAPF:
                    path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("aklist-zapf");
                    break;
            }

            StreamReader sr = new StreamReader(path);

            String line;

            String[] separators = new String[] { Settings.TextFileDelimiter };

            // Additionally, store the days with Workshops...
            Days = new List<string>();

            database = new List<AKlistItem>();
            //Add all Items to DB
            while ((line = sr.ReadLine()) != null)
            {
                string[] words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (words.Count() == 4)
                {
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
            //Close File, finished!
            sr.Close();


        }

        private List<string> Days;

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
