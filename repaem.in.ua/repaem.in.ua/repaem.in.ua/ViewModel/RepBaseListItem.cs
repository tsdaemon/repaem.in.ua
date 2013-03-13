using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    public class RepBaseListItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Rating { get; set; }

        public int RatingCount { get; set; }

        public string ImageSrc { get; set; }

        public string Address { get; set; }
    }
}