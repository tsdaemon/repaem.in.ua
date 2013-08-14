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
		public GoogleMap()
		{
			ApiKey = "AIzaSyC58ukVIqnUhu8CWrPe4fGDFBeDh35WAMc";
			Coordinates = new List<RepbaseInfo>();
			CenterLat = "50.5";
			CenterLon = "30.5";
		}

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
	}

	public class RepbaseInfo
	{
		public int Id { get; set; }

		public float Lat { get; set; }

		public float Long { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public string ThumbSrc { get; set; }
	}
}