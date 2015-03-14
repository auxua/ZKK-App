using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Xamarin.Forms;

namespace ZKK_App.links
{
    /// <summary>
    /// A class for managing the link items
    /// they can be imported in the constructor from textfile
    /// </summary>
    class LinksDB
    {
        //IEnumerable<NewsItem> database;
        List<LinksItem> database;

        public LinksDB()
        {
            //import from file

            string path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("links");

            StreamReader sr = new StreamReader(path);

            String line;

            String[] separators = new String[] { Settings.TextFileDelimiter };

            database = new List<LinksItem>();
            //Add all Items to DB
            while ((line = sr.ReadLine()) != null)
            {
                string[] words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (words.Count() == 2)
                {
                    LinksItem l = new LinksItem();

                    l.Title = words[0];
                    l.Link = words[1];

                    database.Add(l);
                }
            }
            //Close File, finished!
            sr.Close();
        }

        public IEnumerable<LinksItem> GetItems()
        {
            return database;
        }
    }
}
