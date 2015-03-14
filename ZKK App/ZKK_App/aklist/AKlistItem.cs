using System;
using System.Collections.Generic;
using System.Text;

namespace ZKK_App.aklist
{
    /// <summary>
    /// Just a small AKlistItem-Class for being a Model in Model-View-Data for the workshopplan
    /// </summary>
    public class AKlistItem
    {
        public AKlistItem() { }

        public string Title { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public string Room { get; set; }
    }
}
