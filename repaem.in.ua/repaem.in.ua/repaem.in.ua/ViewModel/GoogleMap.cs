using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
	public class GoogleMap
	{
        public List<RepbaseInfo> Coordinates { get; set; }

        public string ApiKey { get; set; }

        public bool Sensor { get; set; }

        public string Center { get; set; }

        public GoogleMap() {
            ApiKey = "AIzaSyC58ukVIqnUhu8CWrPe4fGDFBeDh35WAMc";
        }
	}

    public class RepbaseInfo
    {
        public string Coordinates { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}