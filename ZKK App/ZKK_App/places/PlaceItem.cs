using System;
using System.Collections.Generic;
using System.Text;

namespace ZKK_App.places
{
    /// <summary>
    /// Just a small Placeitem-Class for being a Model in Model-View-Data for Places
    /// </summary>
    public class PlaceItem
    {
        public PlaceItem() { }

        public string Imagename { get; set; }
        public string Title { get; set; }
        public string Rooms { get; set; }
        public string Comment { get; set; }
    }
}
