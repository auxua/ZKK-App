using System;
using System.Collections.Generic;
using System.Text;

namespace ZKK_App.news
{
    /// <summary>
    /// Just a small Newsitem-Class for being a Model in Model-View-Data for News
    /// </summary>
    public class NewsItem
    {
        public NewsItem() { }

        public string Date { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
    }
}
