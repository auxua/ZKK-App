using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Net;

using Xamarin.Forms;

namespace ZKK_App.interfaces
{
    public partial class ConfigPage : ContentPage
    {

        /// <summary>
        /// List of the Conference-Selection human-readable names
        /// </summary>
        Dictionary<String, Conferences> nameToConf = new Dictionary<string, Conferences>
        {
            { "ZaPF",Conferences.ZAPF },
            { "KIF",Conferences.KIF },
            { "KoMa",Conferences.KOMA },
            { "Alle!",Conferences.ALL }
        };

        /// <summary>
        /// A static refernce for the Label representing the progress of the Update
        /// Is visible from outside, but only the UI-Thread is allowed to change text!
        /// </summary>
        public static Label UpdateLabel;


        public ConfigPage()
        {
            //InitializeComponent();

            //
            // For now, deactivate Picker, because it is not used at the moment...
            //


            //Create a Picker for the Conference
            Picker picker = new Picker
            {
                Title = "Konferenz",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 150.0
            };
            //Add conference options to the picker
            foreach (string cName in nameToConf.Keys)
            {
                picker.Items.Add(cName);
            }

            //Not best, but working:
            //Convert the string enum-representation into the enum and convert it to the ord-int.
            int selected = (int)Enum.Parse(typeof(Conferences), (string)Application.Current.Properties[Settings.PropertyConferenceSelection], true);

            //The picker starts at 0, the enum at 1
            selected--;
            picker.SelectedIndex = selected;
            //If conference was changed, update the preferences
            picker.SelectedIndexChanged += (sender, args) =>
            {

                if (picker.SelectedIndex == -1)
                {
                    //Cancel
                }
                else
                {
                    string cName = picker.Items[picker.SelectedIndex];
                    //save somewhere...
                    //Conferences conf = nameToConf[cName];
                    Application.Current.Properties[Settings.PropertyConferenceSelection] = cName;
                }
            };

            //A Switch for the News-Source Switching
            Switch NewsSwitch = new Switch
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            //Current value of NewsOnline-property
            bool newsOnline = (Application.Current.Properties[Settings.PropertyNewsSource].Equals("Online"));

            //Initial Value in UI
            NewsSwitch.IsToggled = newsOnline;

            //Switch should change
            NewsSwitch.Toggled += (sender, args) =>
            {
                newsOnline = (!newsOnline);
                if (newsOnline)
                    Application.Current.Properties[Settings.PropertyNewsSource] = "Online";
                else
                    Application.Current.Properties[Settings.PropertyNewsSource] = "Offline";
            };

            //The label for the DatenStand
            Label DatenLabel = new Label
            {
                XAlign = TextAlignment.Center,
                FontAttributes = FontAttributes.Italic,
                Text = "Datenstand: " + (string)Application.Current.Properties[Settings.PropertyUpdateDate] + "\n "
            };

            //The Label for the Updates
            UpdateLabel = new Label
            {
                XAlign = TextAlignment.Center,
                FontAttributes = FontAttributes.Italic,
                Text = " " //No Text on start
            };

            //The Button, triggering an update
            Button UpdateButton = new Button
            {
                Text = "Updaten (Kann etwas dauern)"
            };

            //Connect the method for the Updater with the Button
            UpdateButton.Clicked += OnButtonClicked;

            //Create embedding Scrollview to allow scrolling on this page
            ScrollView v = new ScrollView();
            v.Orientation = ScrollOrientation.Vertical;

            /*image = new Image
            {
                Source = "Files/geier3d.png"
            };*/

            //Content = new StackLayout
            StackLayout stack = new StackLayout
            {
                Padding = new Thickness(0.3, 0.1, 0.2, 0),
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Center,
                Children = {
						/*new Label {
							XAlign = TextAlignment.Center,
							Text = "Welche Konferenz besuchst du? (Die Anzeige wird entsprechend angepasst)"
						},picker,*/
                        /*new Label {
                            XAlign = TextAlignment.Center,
                            Text = "Soll der Bereich News die News-Website direkt einbinden (Internet-Verbindung immer notwendig!), oder Offline bereitstellen?"
                        },NewsSwitch,*/
                        new Label {
                            Text = " " //Just a fake for making a bit spacing in here
                        },DatenLabel,UpdateButton,UpdateLabel

					}
            };

            v.Content = stack;
            Content = v;


            Title = "Update";
        }

        /// <summary>
        /// Triggers an update of data.
        /// The log of this will be written into the UpdateLabel
        /// </summary>
        void OnButtonClicked(object sender, EventArgs e)
        {
            //Teill user what will happen
            UpdateLabel.Text = "\n Starting Update...";

            //TODO: A file versioning or something to avoid duplicate downloads

            /*
             * How the Update works:
             * 
             * 1. Download the newest News-File for News-Section and Contents-File
             * 2. Get well-known text files as well from server, count number of Files to be loaded
             *      for security reasons, only the verified base Server is allowed for File-downloads!
             * 3. Finish main Thread, The Asynchronous Download-Threads call the downloadFinished method 
             *      and update the UpdateLabel by Invoking an method on the UIThread
             * 4. After all downloads are finished, the method invokes a call for Alert-Display on the UI Thread
             * 
             */


            // Step 1: Load news and contents
            //this.DownloadFile(Settings.AppContentsFileSource, "contents.txt", true); downloads++;
            //string path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("contents.txt");

            downloadFails = false;

            downloads = Settings.ContentFiles.Length + 1;
            this.DownloadFile(Settings.NewsFileSource, "news.txt");


            // Step 2: Well-known text files. (See Settings for details)

            foreach (string file in Settings.ContentFiles)
            {
                this.DownloadFile(Settings.AppFilesBaseUrl + file, file);
            }

            // Step 3: End main Thread and inform User
            UpdateLabel.Text += "\n Update gestartet";


        }


        /// <summary>
        /// A simple Wrapper for the Download of a File.
        /// </summary>
        /// <param name="source">The (remote) source adress of the File</param>
        /// <param name="target">The (local) target name of the file</param>
        /// <param name="binary">The flag indicating, whether the file is a base64-binary file (by default = false)</param>
        /// <returns></returns>
        private void DownloadFile(string source, string target)
        {

            //We will need a webclient for the download
            WebClient webClient = new WebClient();


            //Binary Files cannot be downloaded as strings - use async binary download
            HttpWebRequest lxRequest = (HttpWebRequest)WebRequest.Create(source);

            //returned values are returned as a stream, then read into a string
            String lsResponse = string.Empty;

            RequestState myRequestState = new RequestState(target);
            myRequestState.request = lxRequest;

            //Start the asynchronous request.
            IAsyncResult result =
              (IAsyncResult)lxRequest.BeginGetResponse(new AsyncCallback(RespCallback), myRequestState);

            //UpdateLabel.Text += "\n " + target + " heruntergeladen gestartet";

        }

        /// <summary>
        /// Number of ongoing Downloads
        /// </summary>
        public static int downloads = 0;
        
        /// <summary>
        /// Indicates, whether an Error occured on Downloading,
        /// </summary>
        static bool downloadFails = false;

        /// <summary>
        /// handler for finished downloads
        /// Automatically calls display-Alerts on UI Thread after all downloads are finished.
        /// </summary>
        /// <param name="failed">true, if the download failed</param>
        public void downloadFinished(bool failed)
        {
            // one download less is to await
            downloads--;

            // check fails in the process
            downloadFails |= failed;

            // All downloads finished?
            if (downloads == 0)
            {
                if (!downloadFails)
                {
                    // No fails - inform user and save the date!
                    Device.BeginInvokeOnMainThread(() => { DisplayAlert("Update", "Update erfolgreich fertigestellt", "Super!"); });
                    DateTime dt = DateTime.Now;
                    string datePatt = @"dd.MM.yyyy hh:mm:ss tt";
                    string nowDate = dt.ToString(datePatt);
                    Application.Current.Properties[Settings.PropertyUpdateDate] = nowDate;
                }
                else
                {
                    // Fails occured! Inform user
                    // TODO: Think about data integraty..
                    Device.BeginInvokeOnMainThread(() => { DisplayAlert("Update", "Update konnte nicht erfolgreich beendet werden", "Schade..."); });
                }
            }
        }


        /// <summary>
        /// The Callback for the Download-Action.
        /// Tries to get the Downoad Stream and save the file.
        /// Automatically invokes updates on UI.
        /// </summary>
        /// <param name="asynchronousResult"></param>
        private void RespCallback(IAsyncResult asynchronousResult)
        {

            //State of request is asynchronous.
            RequestState myRequestState = (RequestState)asynchronousResult.AsyncState;
            HttpWebRequest myHttpWebRequest = myRequestState.request;

            try
            {
                myRequestState.response = (HttpWebResponse)myHttpWebRequest.EndGetResponse(asynchronousResult);
            }
            catch (Exception web)
            {
                //Console.WriteLine("Error on Download ("+myRequestState.target+": " + web.Message);
                //fails = "Error on Download ("+myRequestState.target+": " + web.Message;
                //myRequestState.success = false;
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() => { ConfigPage.UpdateLabel.Text += "\n Failed: " + myRequestState.target+" with Message: "+web.Message; });
                this.downloadFinished(true);
                return;
            }

            //Read the response into a Stream object.
            Stream responseStream = myRequestState.response.GetResponseStream();

            //get path for file
            string path = DependencyService.Get<IPersonalStorage>().GetFullFilePath(myRequestState.target);

            //Read from Stream and Write to File
            BinaryReader br = new BinaryReader(responseStream);
            BinaryWriter bw;
            try
            {
                bw = new BinaryWriter(new FileStream(path, FileMode.Create));
            }
            catch (Exception web)
            {
                //something bad happened like No Access for the File
                /*
                 * This mainly occurs on the known issue of Forms 1.3,
                 * where the Imagerenderers of Image-Views sometimes store file handlers
                 * even after the containing page was disposed.
                 * This bug is tracked but for the moment, the simplest workaround is this.
                 * 
                 * In fact, users may be able to update Images only after restarting the app or after several tries.
                 * 
                 * This behaviour can be tolerated in here, because images *should* change seldomly.
                 */

                //tidy up and leave
                br.Close();
                //myRequestState.success = false;
                //return;
                Xamarin.Forms.Device.BeginInvokeOnMainThread(() => { ConfigPage.UpdateLabel.Text += "\n Failed: " + path+" with Message: "+web.Message; });
                this.downloadFinished(true);
                return;
            }

            long length = responseStream.Length;
            byte[] bytes = new byte[length];

            bytes = br.ReadBytes(bytes.Length);

            bw.Write(bytes);
            bw.Flush();
            bw.Close();

            //Set Attributes to Normal
            File.SetAttributes(path, FileAttributes.Normal);

            Xamarin.Forms.Device.BeginInvokeOnMainThread(() => { ConfigPage.UpdateLabel.Text += "\n finished: " + myRequestState.target; });

            this.downloadFinished(false);

            //Finished... Hopefully...
        }

        
        public class RequestState
        {
            // This class stores the State of the request.
            // it is more information avilable than needed, but its ok - the download is horrible...
            const int BUFFER_SIZE = 1024;
            public StringBuilder requestData;
            public byte[] BufferRead;
            public HttpWebRequest request;
            public HttpWebResponse response;
            public Stream streamResponse;
            public string target;
            /*public bool success
            {
                get { return success; }
                set
                {
                    success = value;
                    //Xamarin.Forms.Device.BeginInvokeOnMainThread(() => { ConfigPage.UpdateLabel.Text = "Download of "+target+"finished: "+success; });
                }
            }*/
            public RequestState(string t)
            {
                BufferRead = new byte[BUFFER_SIZE];
                requestData = new StringBuilder("");
                request = null;
                streamResponse = null;
                target = t;
                //success = true;
            }
        }
    }
}
