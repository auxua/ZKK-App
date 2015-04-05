using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections.ObjectModel;

using Xamarin.Forms;

namespace ZKK_App.akinterest
{
    class LikeManagement
    {
        /// <summary>
        /// The actual List of Workshops the users likes
        /// </summary>
        //private static List<String> Likes = null;
        private static ObservableCollection<String> Likes = null;

        /// <summary>
        /// Saves all Likes to File
        /// </summary>
        public static void SaveAKLikes()
        {
            if (Likes == null)
            {
                return;
            }

            //List<String> list = Likes;
            ObservableCollection<String> list = Likes;
            string path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("aklikes");

            StreamWriter sw = new StreamWriter(path,false);

            // Store elements line per line
            foreach (string s in list)
            {
                sw.WriteLine(s);
            }
            sw.Close();
        }

        /// <summary>
        /// Loads all Likes from File
        /// </summary>
        public static void LoadAKLikes()
        {
            // If there is no List stored so far, then at least, create this list!
            if (Likes == null)
            {
                //Likes = new List<string>();
                Likes = new ObservableCollection<string>();
            }
            
            string path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("aklikes");

            if (!File.Exists(path)) return;

            StreamReader sr = new StreamReader(path);
            string s;

            while ((s = sr.ReadLine()) != null)
            {
                if (!Likes.Contains(s))
                    Likes.Add(s);
            }

            sr.Close();
        }

        /// <summary>
        /// Checks whether the Workshop is liked by the user
        /// </summary>
        public static bool CheckName(string name)
        {
            // prevent from null-pointer referencing - instead do an update!
            if (Likes == null)
            {
                LoadAKLikes();
            }
            // Check for the Entry
            return Likes.Contains(name);
        }

        /// <summary>
        /// Adds a Workshop to the list of liked ones
        /// </summary>
        public static void AddLike(string name)
        {
            // Check for valid database
            if (Likes == null)
            {
                LoadAKLikes();
            }
            // Add element
            if (!Likes.Contains(name))
            {
                Likes.Add(name);
            }
            // Store to File!
            SaveAKLikes();
        }

        /// <summary>
        /// Removes a workshop from List of liked ones
        /// </summary>
        public static void RemoveLike(string name)
        {
            // Check for valid database
            if (Likes == null)
            {
                LoadAKLikes();
            }
            // remove element
            if (Likes.Contains(name))
            {
                Likes.Remove(name);
            }
            // Store To File!
            SaveAKLikes();
        }

        /// <summary>
        /// Returns the actual list of likes.
        /// (May return a copy of it in future)
        /// </summary>
        public static ObservableCollection<String> GetList()
        {
            return Likes;
        }
    }
}
