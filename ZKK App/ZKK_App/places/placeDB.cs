using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Xamarin.Forms;

namespace ZKK_App.places
{
    /// <summary>
    /// A class for managing the places items
    /// they can be imported in the constructor from textfile and exported for PlacesItemCell-Views via methods
    /// </summary>
    class PlacesDB
    {
        List<PlaceItem> database;

        public PlacesDB()
        {
            //import from file

            string path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("rooms");

            StreamReader sr = new StreamReader(path);

            String line;

            String[] separators = new String[] { Settings.TextFileDelimiter };

            database = new List<PlaceItem>();
            //Add all Items to DB
            while ((line = sr.ReadLine()) != null)
            {
                string[] words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (words.Count() == 4)
                {
                    PlaceItem p = new PlaceItem();
                    p.Comment = words[3];
                    p.Imagename = words[1];
                    p.Rooms = words[2];
                    p.Title = words[0];

                    database.Add(p);
                }
            }
            //Close File, finished!
            sr.Close();
        }

        public IEnumerable<PlaceItem> GetItems()
        {
            return database;
        }
    }
}
