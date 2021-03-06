﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xamarin.Forms;
using ZKK_App.interfaces;
using System.ComponentModel;

namespace ZKK_App
{
    public class RootPage : MasterDetailPage
    {
        /// <summary>
        /// Create the Root Page
        /// Define master and Detail as wenn as the default Detail page
        /// </summary>
        public RootPage()
        {
            var menuPage = new MenuPage();
            // The effective navigation gets assigned
            menuPage.Menu.ItemSelected += (sender, e) =>
                {
                    // Before Navigating, recolor the cell
                    if (menuPage.Menu.selected != null)
                    {
                        menuPage.Menu.selected.SetColors(true);
                    }

                    // Select new
                    menuPage.Menu.selected = (menuPage.Menu.SelectedItem as NavMenuItem);
                    menuPage.Menu.selected.SetColors(false);

                    NavigateTo(e.SelectedItem as NavMenuItem);
                };

            Master = menuPage;
            // Set default Detail page
            Detail = new NavigationPage(new WelcomePage());
        }

        /// <summary>
        /// Depending on the selected item, go to the corresponding page
        /// </summary>
        /// <param name="menu">the menuitem, that was selected</param>
        void NavigateTo(NavMenuItem menu)
        {

            try
            {
                Page displayPage = (Page)Activator.CreateInstance(menu.TargetType);
                Detail = new NavigationPage(displayPage);

                IsPresented = false;
            }
            catch (Exception e)
            {
                Device.BeginInvokeOnMainThread(() => { DisplayAlert("Fehler", "Die Seite konnte nciht geöffnet werden. Bitte ein Update durchführen. Sollte das Problem bestehen bleiben, bitte melden!\n (Info für Nerds/Debugger: "+e.Message, "Jawoll!"); });
            }
            
        }
    }


    /// <summary>
    /// The MenuPage is the Master Page of the MasterDetailPage.
    /// It contains the menu and gets colored correctly
    /// </summary>
    public class MenuPage : ContentPage
    {
        public MenuListView Menu { get; set; }
        
        public MenuPage()
        {
            if (Device.OS == TargetPlatform.WinPhone)
            {
                Icon = "Toolkit.Content/ApplicationBar.Select.png";
            }
            else
            {
                Icon = "menu.png";
            }
            Title = "Navigation"; // The Title property must be set.
            // Use RWTH-Blue for Background
            BackgroundColor = Color.FromHex("00549F");

            Menu = new MenuListView();

            var menuLabel = new ContentView
            {
                // Allow a bit padding to provide a better view
                Padding = new Thickness(10, 36, 0, 5),
                Content = new Label
                {
                    // This color looks good on the Blue background
                    TextColor = Color.FromHex("DCDCDC"),
                    Text = "Menü",
                    FontSize = Device.GetNamedSize(NamedSize.Large,typeof(Label)),
                    FontAttributes = FontAttributes.Bold
                }
            };

            // Create the Layout of the page
            var layout = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            layout.Children.Add(menuLabel);
            layout.Children.Add(Menu);

            Content = layout;
        }
    }

    /// <summary>
    /// The MenuItem represents the menuitems and stores the corresponding target page.
    /// It can be extended by an Icon, e.g.
    /// </summary>
    public class NavMenuItem : INotifyPropertyChanged
    {
        /// <summary>
        /// The Title that is shown
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The Icon besides the Menu item title
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// The page that should be opened when selecting this item
        /// </summary>
        public Type TargetType { get; set; }

        // below: Event(handling) for (de)selecting the cell.
        // This is used for creating consistency in color schemes between different platforms

        public event PropertyChangedEventHandler PropertyChanged;

        private Color _backgroundColor;

        public Color BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("BackgroundColor"));
                }
            }
        }

        public void SetColors(bool isSelected)
        {
            if ( isSelected )
            {
                // Use RWTH-Blue
                BackgroundColor = Color.FromHex("00549F");
            }
            else
            {
                // Use RWTH light-blue
                BackgroundColor = Color.FromHex("8EBAE5");
            }
        }
    }

    /// <summary>
    /// The view (cell) used in the menu for the menu item.
    /// Inherits from ViewCell the get access to the Layout
    /// (inherited from Imagecell in the past for possibility of images in some time)
    /// </summary>
    public class MenuImageCell : ViewCell
    {
     
        public MenuImageCell() : base()
        {
            Label Text = new Label
            {
                TextColor = Color.FromHex("DCDCDC"),
                XAlign = TextAlignment.Start,
                YAlign = TextAlignment.Center
            };
            Text.SetBinding(Label.TextProperty, "Title");

            Label pad = new Label
            {
                Text = " "
            };

            Image image = new Image
            {
                Source = "info.png",
                HeightRequest = 30,
            };
            image.SetBinding(Image.SourceProperty, "Icon");

            var layout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center,
                Children = { pad, image, pad, Text }
            };
            layout.SetBinding(Layout.BackgroundColorProperty, new Binding("BackgroundColor"));

            if (Device.OS == TargetPlatform.WinPhone)
                layout.HeightRequest = 80;

            View = layout;
        }
    }

    /// <summary>
    /// The menu is created using a adapted Listview.
    /// By that most of the stuff is already done by default ;)
    /// </summary>
    public class MenuListView : ListView
    {
        public NavMenuItem selected { get; set; }
        
        public MenuListView()
        {
            List<NavMenuItem> data = new MenuListData();

            ItemsSource = data;
            VerticalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Color.Transparent;
            
            // Set Bindings
            var cell = new DataTemplate(typeof(MenuImageCell));
            cell.SetBinding(TextCell.TextProperty, "Title");
            // Image Binding possible in here..
            // cell.SetBinding(ImageCell.ImageSourceProperty, "IconSource");

            ItemTemplate = cell;
            // The first item is selected - the lines 2 and 3 are only for the color-fix...
            //SelectedItem = data[0];
            data[0].SetColors(false);
            selected = data[0];
        }
    }

    /// <summary>
    /// Here, we provide the technical navigation.
    /// The Title and corresponding pages are added to the menu.
    /// </summary>
    public class MenuListData : List<NavMenuItem>
    {
        public MenuListData()
        {
            this.Add(new NavMenuItem()
            {
                Title = "Willkommen",
                Icon = "info.png",
                TargetType = typeof(interfaces.WelcomePage)
            });
            
            this.Add(new NavMenuItem()
            {
                //Title = "Einstellungen/Update",
                Title = "Update",
                Icon = "refresh.png",
                TargetType = typeof(interfaces.ConfigPage)
            });

            this.Add(new NavMenuItem()
            {
                Title = "News",
                Icon = "news.png",
                TargetType = typeof(interfaces.NewsPage)
            });

            this.Add(new NavMenuItem()
            {
                Title = "ZIS (Online)",
                Icon = "rss.png",
                TargetType = typeof(interfaces.ZISOnlinePage)
            });

            this.Add(new NavMenuItem()
            {
                Title = "Orte",
                Icon = "house.png",
                TargetType = typeof(interfaces.PlacesPage)
            });

            this.Add(new NavMenuItem()
            {
                Title = "Mensabewertung",
                Icon = "mensa.png",
                TargetType = typeof(interfaces.MensaPage)
            });

            // Billing is deactivated due to no API at the moment...

            /*this.Add(new MenuItem()
            {
                Title = "Kasse",
                TargetType = typeof(interfaces.BillingPage)
            });*/

            this.Add(new NavMenuItem()
            {
                Title = "Ablaufplan",
                Icon = "cal.png",
                TargetType = typeof(interfaces.PlanPage)
            });

            this.Add(new NavMenuItem()
            {
                Title = "AK-Liste",
                Icon = "list.png",
                TargetType = typeof(interfaces.AKPage)
            });

            this.Add(new NavMenuItem()
            {
                Title = "AK-Plan ZaPF",
                Icon = "cal.png",
                TargetType = typeof(interfaces.AKListPageZapf)
            });

            this.Add(new NavMenuItem()
            {
                Title = "AK-Plan KIF",
                Icon = "cal.png",
                TargetType = typeof(interfaces.AKListPageKif)
            });
            
            this.Add(new NavMenuItem()
            {
                Title = "AK-Plan KoMa",
                Icon = "cal.png",
                TargetType = typeof(interfaces.AKListPageKoma)
            });
            
            this.Add(new NavMenuItem()
            {
                Title = "Plan gemeinsame AKs",
                Icon = "cal.png",
                TargetType = typeof(interfaces.AKListPageZKK)
            });

            this.Add(new NavMenuItem()
            {
                Title = "Mein Stundenplan",
                Icon = "cal.png",
                TargetType = typeof(interfaces.AKLikesPage)
            });

            this.Add(new NavMenuItem()
            {
                Title = "Raumfinder",
                Icon = "pin.png",
                //TargetType = typeof(WelcomePage)
                TargetType = typeof(interfaces.RoomFinderPage)
            });

            this.Add(new NavMenuItem()
            {
                Title = "Raumliste",
                Icon = "pin.png",
                TargetType = typeof(interfaces.RoomListPage)
            });

            this.Add(new NavMenuItem()
            {
                Title = "Gemeinschaftsstandards",
                Icon = "thumbsup.png",
                //Title = "Satzungen und Policies",
                //TargetType = typeof(WelcomePage)
                TargetType = typeof(interfaces.PolicyPage)
            });

            this.Add(new NavMenuItem()
            {
                Title = "Link-Sammlung",
                Icon = "rss.png",
                //TargetType = typeof(WelcomePage)
                TargetType = typeof(interfaces.LinksPage)
            });

            this.Add(new NavMenuItem()
            {
                Title = "Über diese App",
                Icon = "info.png",
                //TargetType = typeof(WelcomePage)
                TargetType = typeof(interfaces.AboutPage)
            });
        }
    }

    
}
