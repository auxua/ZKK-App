using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using Xamarin.Forms;

namespace ZKK_App.interfaces
{
    public partial class PlanPage : ContentPage
    {
        /// <summary>
        /// The Page for the Plan of the Conference.
        /// What's happening in here:
        ///     Create a base64 representation of the image because many Android-version can only deal with images up to 2048x2048px.
        ///     Create a basic webpage containing this image to get whole gesture-support the easy way
        ///     
        /// Possible Improvements: 
        ///     in HTML, make background black in Android/WP, white in iOS
        ///     Do something for optimzing device rotation?
        /// 
        /// </summary>
        public PlanPage()
        {
            //InitializeComponent ();

            var path = DependencyService.Get<IPersonalStorage>().GetFullFilePath("plan.jpg");

            // create base 64 encoded image from path
            // provide read access to the file
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            // Create a byte array of file stream length
            byte[] ImageData = new byte[fs.Length];
            // Read block of bytes from stream into the byte array
            fs.Read(ImageData, 0, System.Convert.ToInt32(fs.Length));
            // Close the File Stream
            fs.Close();
            String base64String = Convert.ToBase64String(ImageData);
            // embed in HTML
            string html = "<html><head><meta name=\"viewport\" content=\"width=device-width, initial-scale=0.20, maximum-scale=3.0 user-scalable=1\" /></head><body><img src=\"data:image/jpg;base64," + base64String + "\" style=\"width=100%\" /></body></html>";
            // Create webview content for the string
            HtmlWebViewSource htmlSource = new HtmlWebViewSource();
            htmlSource.Html = html;

            WebView wv = new WebView
            {
                Source = htmlSource,
            };

            

            Content = wv;
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            //Device.BeginInvokeOnMainThread(() => { DisplayAlert("Debug", "w: " + this.Width + " - h: " + this.Height, "OK"); });
            /*if (width < height)
            {
                image.Rotation = 90;
                //image.Rotation = 2;
            }
            else
            {
                image.Rotation = 0;
            }
            layout.HeightRequest = 2*height;
            layout.WidthRequest = 2*width;
            image.HeightRequest = 2*height;
            image.WidthRequest = 2*width;
            image.Aspect = Aspect.Fill;*/
        }

       

    }
}
