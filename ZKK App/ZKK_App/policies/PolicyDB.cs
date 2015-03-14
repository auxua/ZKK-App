using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Xamarin.Forms;

namespace ZKK_App.policies
{
    /// <summary>
    /// A class for managing the policy items
    /// they can be imported in the constructor from textfile and exported for PolicyItemCell-Views via methods
    /// </summary>
    class PolicyDB
    {
        List<PolicyItem> database;

        /// <summary>
        /// A Database for the Policies of the conference
        /// 
        /// </summary>
        public PolicyDB()
        {
            //import from file

            string path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("policies");

            StreamReader sr = new StreamReader(path);

            String line;

            String[] separators = new String[] { Settings.TextFileDelimiter };

            database = new List<PolicyItem>();
            //Add all Items to DB
            while ((line = sr.ReadLine()) != null)
            {
                string[] words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (words.Count() == 2)
                {
                    PolicyItem p = new PolicyItem();

                    p.Title = words[0];
                    p.Text = words[1].Replace(Settings.LineBreakEscape, " \n");
                    database.Add(p);
                }
            }
            //Close File, finished!
            sr.Close();
        }

        public IEnumerable<PolicyItem> GetItems()
        {
            return database;
        }
    }
}
