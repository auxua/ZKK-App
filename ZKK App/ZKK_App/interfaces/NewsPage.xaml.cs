using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

using Xamarin.Forms;
using ZKK_App.news;

namespace ZKK_App.interfaces
{
	public partial class NewsPage : ContentPage
	{
        ListView listview;
        
        public NewsPage ()
		{
			//InitializeComponent ();
   
            // create a listview
            listview = new ListView();

                
            // Xamarin Forms Update!
            listview.HasUnevenRows = true;
            //listview.RowHeight = 140;
            // set the cell data template
            listview.ItemTemplate = new DataTemplate(typeof(NewsItemCell));
                
            // HACK: workaround issue #894 for now
            if (Device.OS == TargetPlatform.iOS)
                listview.ItemsSource = new string[1] { "" };

            NewsDB db = new NewsDB();

            //var items = db.GetItems();
            //DisplayAlert("Test", "News: " + items., "OK");

            // Perform Data Binding
            listview.ItemsSource = db.GetItems();
            // Enable Pull-To-Refresh functionality
            listview.IsPullToRefreshEnabled = true;
            listview.Refreshing += listview_Refreshing;

            listview.Header = "Zum Aktualisieren der News, einfach runter ziehen";

            Title = "News";

            Content = listview;

		}

        /// <summary>
        /// Simple Event handler for refeshing the list view
        /// Will download the news.txt and re-bind the data source
        /// </summary>
        private void listview_Refreshing(object sender, EventArgs e)
        {
            this.DownloadFile(Settings.NewsFileSource, "news.txt");
        }

        /// <summary>
        /// A simple Wrapper for the Download of a File.
        /// </summary>
        /// <param name="source">The (remote) source adress of the File</param>
        /// <param name="target">The (local) target name of the file</param>
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

        }
        

        /// <summary>
        /// handler for finished download
        /// Refresh data and inform user if the update failed
        /// </summary>
        /// <param name="failed">true, if the download failed</param>
        public void downloadFinished(bool failed)
        {
            // Stop Refreshing
            Device.BeginInvokeOnMainThread(() => { listview.IsRefreshing = false; });
            //listview.IsRefreshing = false;
            if (failed)
            {
                // Downlaod Failed - Inform User
                Device.BeginInvokeOnMainThread(() => { DisplayAlert("Fehler", "Update der News konnte nciht durchgeführt werden", "Schade..."); });
            }
            else
            {
                // Download finished - fill listview again with the items
                NewsDB db = new NewsDB();
                // Bind new Source on Main Thread (otherwise, strange things may happen - espacially on KitKat)
                Device.BeginInvokeOnMainThread(() => { listview.ItemsSource = db.GetItems(); });
            }
            // Giving the listview the focus will trigger an view-update
            Device.BeginInvokeOnMainThread(() => { listview.Focus(); });
            //listview.Focus();
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
            catch
            {
                //Console.WriteLine("Error on Download ("+myRequestState.target+": " + web.Message);
                //fails = "Error on Download ("+myRequestState.target+": " + web.Message;
                //myRequestState.success = false;
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
            catch
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
                //Xamarin.Forms.Device.BeginInvokeOnMainThread(() => { ConfigPage.UpdateLabel.Text += "\n Failed: " + path + " with Message: " + web.Message; });
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
