using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Xamarin.Forms;

namespace ZKK_App.news
{
    /// <summary>
    /// A class for managing the news items
    /// they can be imported in the constructor from textfile and exported for NewsItemCell-Views via methods
    /// </summary>
    class NewsDB
    {
        //IEnumerable<NewsItem> database;
        List<NewsItem> database;

        public NewsDB()
        {
            //import from file

            string path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("news.txt");

            StreamReader sr = new StreamReader(path);

            String line;

            String[] separators = new String[] { Settings.TextFileDelimiter };

            database = new List<NewsItem>();
            //Add all Items to DB
            while ((line = sr.ReadLine()) != null)
            {
                string[] words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (words.Count() == 3)
                {
                    NewsItem n = new NewsItem();
                    n.Date = words[2];
                    n.Detail = (words[1]).Replace(Settings.LineBreakEscape,"\n");
                    //n.Detail = words[1];
                    n.Title = words[0];

                    database.Add(n);
                }
            }
            //Close File, finished!
            sr.Close();
        }

        public IEnumerable<NewsItem> GetItems()
        {
            return database;
        }
    }
}
