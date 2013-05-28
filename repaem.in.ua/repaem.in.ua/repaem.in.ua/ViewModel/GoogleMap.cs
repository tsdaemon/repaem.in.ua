using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    /// <summary>
    /// Гугл мапа
    /// </summary>
	public class GoogleMap
	{
        /// <summary>
        /// Список координат, які будуть відмічені
        /// </summary>
        public List<RepbaseInfo> Coordinates { get; set; }

        public string ApiKey { get; set; }

        public bool Sensor { get; set; }

        /// <summary>
        /// Центр довгота
        /// </summary>
        public string CenterLat { get; set; }

        /// <summary>
        /// Центер широта
        /// </summary>
        public string CenterLon { get; set; }

        public bool EditMode { get; set; }

        public GoogleMap() 
        {
            ApiKey = "AIzaSyC58ukVIqnUhu8CWrPe4fGDFBeDh35WAMc";
            Coordinates = new List<RepbaseInfo>();
            CenterLat = "50.5";
            CenterLon = "30.5";
        }

        public GoogleMap(bool demo)
            : this()
        {
            Coordinates.AddRange(new RepbaseInfo[] {
                new RepbaseInfo() { 
                    Lat = 51.4556f,
                    Long = 30.32323f, 
                    Description="kjhblkblkg", 
                    Title = "bfjdhfjdhfjhdf" 
                },
                new RepbaseInfo() {
                    Lat = 50.1111f,
                    Long = 30.23445f,
                    Description = ";lkj;ljh",
                    Title = "232424"
                }
            });
        }
	}

    public class RepbaseInfo
    {
        public int Id { get; set; }

        public float Lat { get; set; }

        public float Long { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}