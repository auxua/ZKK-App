using System;
using System.Collections.Generic;
using System.Text;

namespace ZKK_App.aks
{
    /// <summary>
    /// Just a small AKitem-Class for being a Model in Model-View-Data for AKs
    /// 
    /// Maybe add Links for the Wiki-pages later on...
    /// </summary>
    public class AKItem
    {
        public AKItem() { }

        public string Detail { get; set; }
        public string Title { get; set; }
        //public string Rooms { get; set; }
        //public string Comment { get; set; }
    }
}
