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

        public GoogleMap() 
        {
            ApiKey = "AIzaSyC58ukVIqnUhu8CWrPe4fGDFBeDh35WAMc";
            Coordinates = new List<RepbaseInfo>();
            Center = "50.5, 30.5";
        }

        public GoogleMap(bool demo)
            : this()
        {
            Coordinates.AddRange(new RepbaseInfo[] {
                new RepbaseInfo() { 
                    Coordinates = "51.4556,30.32323", 
                    Description="kjhblkblkg", 
                    Title = "bfjdhfjdhfjhdf" 
                },
                new RepbaseInfo() {
                    Coordinates = "50.1111,30.23445",
                    Description = ";lkj;ljh",
                    Title = "232424"
                }
            });
        }
	}

    public class RepbaseInfo
    {
        public string Coordinates { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}