using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using Xamarin.Forms;
using System.IO;

namespace ZKK_App.interfaces
{
	public partial class RoomFinderPage : ContentPage
	{

        Entry entry;
        Label answer;
        Dictionary<string, string> rooms;
        public RoomFinderPage ()
		{
			InitializeComponent ();

            
            rooms = new Dictionary<string,string>();

            // The Dictionary should not be created in UI Thread
            Thread t = new Thread(createRoomsDB);
            // We do not need to wait for the Thread to finish, because it will be faster than the user/Page creation
            t.Start();

            // Create an Entry-View to allow users entering Room Names
            entry = new Entry
            {
                Placeholder = "Raumname zum Suchen",
                HorizontalOptions = LayoutOptions.Center
            };

            // The Label which will show the answer to the question
            answer = new Label {
                XAlign = TextAlignment.Center,
                HorizontalOptions = LayoutOptions.Center
            };

            // Create a Button that allows users to ask the question
            Button button = new Button
            {
                Text = "Wo ist der Raum?",
                HorizontalOptions = LayoutOptions.Center
            };

            // Tell the Button what to do on action
            button.Clicked += onButtonClicked;

            
            entry.Completed += (sender, e) =>
                {
                    button.Focus();
                    this.onButtonClicked(sender, e);
                };

            // Create the Layout
            Title = "Raumfinder";
            StackLayout stack = new StackLayout
            {
                Children =
                {
                    entry, button, answer
                },
                VerticalOptions = LayoutOptions.Center
            };

            ScrollView scroll = new ScrollView
            {
                Orientation = ScrollOrientation.Vertical,
                Content = stack
            };


            Content = scroll;
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
                    rooms[words[0].ToLower()] =  words[1];
                }
            }
            //Close File, finished!
            sr.Close();
            //Console.WriteLine("Finished Database!");
        }

        /// <summary>
        /// On Clicking the Button, a query in the Database is raised.
        /// The answer will be shown on the answer-Label by the UI Thread
        /// </summary>
        void onButtonClicked(object sender, EventArgs e)
        {
            try
            {
                string a = "";

                if (rooms.ContainsKey(entry.Text.ToLower()))
                    a = rooms[entry.Text.ToLower()];
                else
                    a = "Raum nicht gefunden. Bitte auf korrekte Schreibweise achten...";

                Device.BeginInvokeOnMainThread(() => { answer.Text = a; });
            }
            catch
            {
                Device.BeginInvokeOnMainThread(() => { answer.Text = "Bitte einen Raumnamen eingeben!"; });
            }
        }
	}
}
