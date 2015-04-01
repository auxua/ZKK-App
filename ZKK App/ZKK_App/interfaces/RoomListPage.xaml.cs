using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

using Xamarin.Forms;

namespace ZKK_App.interfaces
{
	public partial class RoomListPage : ContentPage
	{
        List<RoomItem> rooms;

        public class RoomItem
        {
            public string RoomName { get; set; }
            public string RoomDetail { get; set; }
        }
        
        public RoomListPage ()
		{
			//InitializeComponent ();

            rooms = new List<RoomItem>();

            // The Dictionary should not be created in UI Thread
            Thread t = new Thread(createRoomsDB);
            // We do not need to wait for the Thread to finish, because it will be faster than the user/Page creation
            t.Start();

            // Create the Layout
            Title = "Raumliste";

            ListView listView = new ListView();

            listView.ItemTemplate = new DataTemplate(typeof(TextCell));
            listView.ItemTemplate.SetBinding(TextCell.TextProperty, "RoomName");
            listView.ItemTemplate.SetBinding(TextCell.DetailProperty, "RoomDetail");
            //t.Join();
            listView.ItemsSource = rooms;
            
            // Create a Scrollview to allow Scrolling on the Page
            Content = listView;
            
		}

        /// <summary>
        /// Creates the Database of rooms for performing efficient queries
        /// </summary>
        private void createRoomsDB()
        {
            string path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("roomfinder");

            StreamReader sr = new StreamReader(path);

            String line;

            String[] separators = new String[] { Settings.TextFileDelimiter };

            //Add all Items to DB
            while ((line = sr.ReadLine()) != null)
            {
                string[] words = line.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                if (words.Count() == 2)
                {
                    //rooms[words[0].ToLower()] = words[1];
                    RoomItem ri = new RoomItem();
                    ri.RoomName = words[0];
                    ri.RoomDetail = words[1];
                    rooms.Add(ri);
                }
            }
            //Close File, finished!
            sr.Close();
            //Console.WriteLine("Finished Database!");
        }
	}
}
