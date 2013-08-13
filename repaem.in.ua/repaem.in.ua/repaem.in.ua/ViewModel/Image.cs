using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    public class Image
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string ThumbSrc { get; set; }

        public string Src { get; set; }

        public Image()
        {

        }
    }
}