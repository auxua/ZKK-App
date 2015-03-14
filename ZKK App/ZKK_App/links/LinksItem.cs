using System;
using System.Collections.Generic;
using System.Text;

namespace ZKK_App.links
{
    /// <summary>
    /// Just a small LinkItem-Class for being a Model in Model-View-Data for Links
    /// </summary>
    public class LinksItem
    {
        public LinksItem() { }

        public string Title { get; set; }
        public string Link { get; set; }
    }
}
