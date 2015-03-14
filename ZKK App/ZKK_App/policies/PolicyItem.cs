using System;
using System.Collections.Generic;
using System.Text;

namespace ZKK_App.policies
{
    /// <summary>
    /// Just a small PoliciesItem-Class for being a Model in Model-View-Data for Policies
    /// </summary>
    public class PolicyItem
    {
        public PolicyItem() { }

        public string Title { get; set; }
        public string Text { get; set; }
    }
}
