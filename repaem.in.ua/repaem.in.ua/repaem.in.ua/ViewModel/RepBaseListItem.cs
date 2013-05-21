using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    /// <summary>
    /// ВьюМодел відображення бази у списку
    /// </summary>
    public class RepBaseListItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Rating { get; set; }

        public int RatingCount { get; set; }

        /// <summary>
        /// Посилання на лого бази
        /// </summary>
        public string ImageSrc { get; set; }

        public string Address { get; set; }
    }
}