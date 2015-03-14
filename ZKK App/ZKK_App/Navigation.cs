using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Xamarin.Forms;
using ZKK_App.interfaces;

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
            menuPage.Menu.ItemSelected += (sender, e) => NavigateTo(e.SelectedItem as MenuItem);

            Master = menuPage;
            // Set default Detail page
            Detail = new NavigationPage(new WelcomePage());
        }

        /// <summary>
        /// Depending on the selected item, go to the corresponding page
        /// </summary>
        /// <param name="menu">the menuitem, that was selected</param>
        void NavigateTo(MenuItem menu)
        {
            Page displayPage = (Page)Activator.CreateInstance(menu.TargetType);

            Detail = new NavigationPage(displayPage);

            IsPresented = false;
        }
    }


    /// <summary>
    /// The MenuPage is the Master Page of the MasterDetailPage.
    /// It contains the menu and gets colored correctly
    /// </summary>
    public class MenuPage : ContentPage
    {
        public ListView Menu { get; set; }

        public MenuPage()
        {
            Icon = "menu.png";
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
    public class MenuItem
    {
        /// <summary>
        /// The Title that is shown
        /// </summary>
        public string Title { get; set; }

        //public string IconSource { get; set; }

        /// <summary>
        /// The page that should be opened when selecting this item
        /// </summary>
        public Type TargetType { get; set; }
    }

    /// <summary>
    /// The view (cell) used in the menu for the menu item.
    /// Inherits from ImageCell for eventually add an image easily later on.
    /// </summary>
    public class MenuImageCell : ImageCell
    {        
        //TODO: Find images for the Menu items, then they can be added
        public MenuImageCell()
        {
            //make color device-dependent - the default colors may be not suitable!
#if __IOS__
            this.TextColor = Color.FromHex("262626");
#else            
            this.TextColor = Color.FromHex("DCDCDC");
#endif
            
        }
    }

    /// <summary>
    /// The menu is created using a adapted Listview.
    /// By that most of the stuff is already done by default ;)
    /// </summary>
    public class MenuListView : ListView
    {
        public MenuListView()
        {
            List<MenuItem> data = new MenuListData();

            ItemsSource = data;
            VerticalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Color.Transparent;
            
            // Set Bindings
            var cell = new DataTemplate(typeof(MenuImageCell));
            cell.SetBinding(TextCell.TextProperty, "Title");
            // Image Binding possible in here..
            // cell.SetBinding(ImageCell.ImageSourceProperty, "IconSource");

            ItemTemplate = cell;
            // The first item is selected
            SelectedItem = data[0];
        }
    }

    /// <summary>
    /// Here, we provide the technical navigation.
    /// The Title and corresponding pages are added to the menu.
    /// </summary>
    public class MenuListData : List<MenuItem>
    {
        public MenuListData()
        {
            this.Add(new MenuItem()
            {
                Title = "Willkommen",
                //IconSource = "example.png",
                TargetType = typeof(interfaces.WelcomePage)
            });
            
            this.Add(new MenuItem()
            {
                Title = "Einstellungen/Update",
                TargetType = typeof(interfaces.ConfigPage)
            });

            this.Add(new MenuItem()
            {
                Title = "News",
                TargetType = typeof(interfaces.NewsPage)
            });

            this.Add(new MenuItem()
            {
                Title = "Orte",
                TargetType = typeof(interfaces.PlacesPage)
            });

            this.Add(new MenuItem()
            {
                Title = "Kasse",
                TargetType = typeof(interfaces.BillingPage)
            });

            this.Add(new MenuItem()
            {
                Title = "AK-Liste",
                TargetType = typeof(interfaces.AKPage)
            });

            this.Add(new MenuItem()
            {
                Title = "AK-Plan ZaPF",
                TargetType = typeof(interfaces.AKListPageZapf)
            });

            this.Add(new MenuItem()
            {
                Title = "AK-Plan KIF",
                TargetType = typeof(interfaces.AKListPageKif)
            });
            
            this.Add(new MenuItem()
            {
                Title = "AK-Plan KoMa",
                TargetType = typeof(interfaces.AKListPageKoma)
            });
            
            this.Add(new MenuItem()
            {
                Title = "Plan gemeinsame AKs",
                TargetType = typeof(interfaces.AKListPageZKK)
            });

            this.Add(new MenuItem()
            {
                Title = "Raumfinder",
                //TargetType = typeof(WelcomePage)
                TargetType = typeof(interfaces.RoomFinderPage)
            });

            this.Add(new MenuItem()
            {
                Title = "Satzungen und Policies",
                //TargetType = typeof(WelcomePage)
                TargetType = typeof(interfaces.PolicyPage)
            });

            this.Add(new MenuItem()
            {
                Title = "Link-Sammlung",
                //TargetType = typeof(WelcomePage)
                TargetType = typeof(interfaces.LinksPage)
            });

            this.Add(new MenuItem()
            {
                Title = "Über diese App",
                //TargetType = typeof(WelcomePage)
                TargetType = typeof(interfaces.AboutPage)
            });
        }
    }

    
}
