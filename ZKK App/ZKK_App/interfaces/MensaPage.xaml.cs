using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

using Xamarin.Forms;

namespace ZKK_App.interfaces
{
	public partial class MensaPage : ContentPage
	{
        Entry gerichtEntry;
        Entry commentEntry;
        Entry homeEntry;
        List<string> mensen;
        List<string> ratings;
        Picker mensaPicker;
        Picker ratingPicker;
        ActivityIndicator act;

		public MensaPage ()
		{
			//InitializeComponent ();

            mensen = new List<string> { "Mensa Academica", "Bistro", "Ahornstraße", "Mensa Vita" };
            ratings = new List<string> { "Viel besser", "Etwas besser", "Etwa gleich", "schlechter", "Viel schlechter" };

            Label titleLabel = new Label {
                //YAlign = TextAlignment.Center,
                XAlign = TextAlignment.Center,
                TextColor = Color.FromHex("00549F"),
                FontAttributes = FontAttributes.Bold,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                Text = "Mensabewertung"
            };

            Title = "Mensabewertung";

            Label subtitleLabel = new Label
            {
                //YAlign = TextAlignment.Center,
                XAlign = TextAlignment.Center,
                FontAttributes = FontAttributes.None,
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                Text = "Hier kannst du unsere Mensen bewerten im Vergleich zur Heimatmensa \n"
            };

            Label mensaLabel = new Label {
                //YAlign = TextAlignment.Center,
                XAlign = TextAlignment.Center,
                //TextColor = Color.FromHex("00549F"),
                FontAttributes = FontAttributes.None,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                Text = "\nBesuchte Mensa"
            };

            Label gerichtLabel = new Label
            {
                //YAlign = TextAlignment.Center,
                XAlign = TextAlignment.Center,
                //TextColor = Color.FromHex("00549F"),
                FontAttributes = FontAttributes.None,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
				Text = "\nGericht"
            };

            Label homeLabel = new Label
            {
                //YAlign = TextAlignment.Center,
                XAlign = TextAlignment.Center,
                //TextColor = Color.FromHex("00549F"),
                FontAttributes = FontAttributes.None,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
				Text = "\nHeimatuni"
            };

            Label ratingLabel = new Label
            {
                //YAlign = TextAlignment.Center,
                XAlign = TextAlignment.Center,
                //TextColor = Color.FromHex("00549F"),
                FontAttributes = FontAttributes.None,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
				Text = "\nIm Vergleich zu meiner Uni"
            };

            Label commentLabel = new Label
            {
                //YAlign = TextAlignment.Center,
                XAlign = TextAlignment.Center,
                //TextColor = Color.FromHex("00549F"),
                FontAttributes = FontAttributes.None,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
				Text = "\nKommentar"
            };

            mensaPicker = new Picker
            {
                Title = "Mensa-Auswahl",
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 250.0
            };
            foreach (string s in mensen)
            {
                mensaPicker.Items.Add(s);
            }
            mensaPicker.SelectedIndex = 0;


            ratingPicker = new Picker
            {
                Title = "Mensa-Bewertung",
                HorizontalOptions = LayoutOptions.Center,
                WidthRequest = 250.0
            };
            foreach (string s in ratings)
            {
                ratingPicker.Items.Add(s);
            }
            ratingPicker.SelectedIndex = 0;

			gerichtEntry = new Entry {
				HorizontalOptions = LayoutOptions.Center,
				WidthRequest = 300,
			};
			commentEntry = new Entry{
				HorizontalOptions = LayoutOptions.Center,
				WidthRequest = 300,
			};
			homeEntry = new Entry{
				HorizontalOptions = LayoutOptions.Center,
				WidthRequest = 300,
			};

            Button button = new Button
            {
                Text = "Absenden",
                HorizontalOptions = LayoutOptions.Center
            };

            

            act = new ActivityIndicator();
            act.IsRunning = false;
            act.IsVisible = false;

            button.Clicked += OnButtonClicked;

            gerichtEntry.Completed += (sender, e) =>
            {
                homeEntry.Focus();
            };

            commentEntry.Completed += async (sender, e) =>
            {
                button.Focus();
                //button.Command.Execute(null);
                await Task.Factory.StartNew(() => this.OnButtonClicked(sender, e));
                //await this.OnButtonClicked(sender, e);
            };

            homeEntry.Completed += (sender, e) =>
            {
                commentEntry.Focus();
            };

            StackLayout stack = new StackLayout
            {
                //Padding = new Thickness(0.3, 0.1, 0.2, 0),
                Padding = new Thickness(20.0, 10.0, 5.0, 5.0),
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Center,
                Children = { titleLabel, subtitleLabel, mensaLabel, mensaPicker, gerichtLabel, gerichtEntry,
                    homeLabel, homeEntry, ratingLabel, ratingPicker, commentLabel, commentEntry, act, button }
            };

            ScrollView scroll = new ScrollView();
            scroll.Content = stack;

            Content = scroll;

		}

        public async void OnButtonClicked(object sender, EventArgs e)
        {
            // Check data
            string gericht;
            string uni;
            string comment;

            // Try getting strings - can result in exception for unchanged entries (some platforms)
            try
            {
                if (homeEntry.Text.Length > 0)
                {
                    uni = homeEntry.Text;
                }
                else
                {
                    Device.BeginInvokeOnMainThread(() => { DisplayAlert("Fehler", "Bitte Heimatuni oder -ort angeben", "OK"); });
                    return;
                }
            }
            catch
            {
                Device.BeginInvokeOnMainThread(() => { DisplayAlert("Fehler", "Bitte Heimatuni oder -ort angeben", "OK"); });
                return;
            }


            try
            {
                if (commentEntry.Text.Length > 0)
                {
                    comment = commentEntry.Text;
                }
                else
                {
                    comment = "";
                }
            }
            catch
            {
                comment = "";
            }


            try
            {
                if (gerichtEntry.Text.Length > 0)
                {
                    gericht = gerichtEntry.Text;
                }
                else
                {
                    gericht = "";
                }
            }
            catch
            {
                gericht = "";
            }

            string mensa = mensen.ElementAt(mensaPicker.SelectedIndex);
            string rating = ratings.ElementAt(ratingPicker.SelectedIndex);

            Device.BeginInvokeOnMainThread(() => { act.IsRunning = true; act.IsVisible = true; });

            string answer = await SendMensaRatingAsync(mensa, gericht, uni, rating, comment);

            bool success = (answer == "Bewertung eingetragen");

            Device.BeginInvokeOnMainThread(() =>
            {
                act.IsRunning = false;
                act.IsVisible = false;
                if (success)
                {
                    DisplayAlert("Fertig", "Daten eingetragen - Vielen Dank!", "OK");
                }
                else
                {
                    DisplayAlert("Problem", "Ein Fehler ist aufgetreten: " + answer, "OK - Schade");
                }
            });
        }

        public async static Task<string> SendMensaRatingAsync(string mensa, string gericht, string uni, string rating, string comment)
        {
            var answer = await interfaces.MensaPage.RestCallAsync("radiomensa="+mensa+"&textessen="+gericht+"&texthome="+uni+"&radiorating="+rating+"&textcomment="+comment, "https://zkk.fsmpi.rwth-aachen.de/news/mensaapp");
            return answer;
        }

        /// <summary>
        /// A generic REST-Call to an endpoint using POST method
        /// 
        /// Uses a WebRequest for POST
        /// </summary>
        /// <typeparam name="T1">The Datatype to await for response</typeparam>
        /// <param name="input">the data as string (url-encdoed)</param>
        /// <param name="endpoint">The REST-Endpoint to call</param>
        /// <returns>The datatype that has been awaited for the call or default(T1) on error</returns>
        public async static Task<string> RestCallAsync(string input, string endpoint)
        {
            try
            {

                var http = (HttpWebRequest)WebRequest.Create(new Uri(endpoint));
                //http.Accept = "application/json";
                //http.ContentType = "application/json";
                http.ContentType = "application/x-www-form-urlencoded";
                http.Method = "POST";
                //string parsedContent = "{ \"client_id\": \"" + Config.ClientID + "\", \"scope\": \"l2p.rwth userinfo.rwth\" }";

                //ASCIIEncoding encoding = new ASCIIEncoding();
                Byte[] bytes = Encoding.UTF8.GetBytes(input);

                using (var stream = await Task.Factory.FromAsync<Stream>(http.BeginGetRequestStream,
                                                        http.EndGetRequestStream, null))
                {
                    // Write the bytes to the stream
                    await stream.WriteAsync(bytes, 0, bytes.Length);
                }

                using (var response = await Task.Factory.FromAsync<WebResponse>(http.BeginGetResponse,
                                    http.EndGetResponse, null))
                {
                    var stream = response.GetResponseStream();
                    var sr = new StreamReader(stream);
                    string content = sr.ReadToEnd();

                    //T1 answer = JsonConvert.DeserializeObject<T1>(content);
                    //T1 answer = default(T1);

                    //string content = response.Content.ReadAsStringAsync().Result;
                    //Console.WriteLine(content);
                    //String ans = content;
                    return content;

                }

               
            }
            catch (Exception ex)
            {
                var t = ex.Message;
                return ""+t;
            }
        }
	}
}
