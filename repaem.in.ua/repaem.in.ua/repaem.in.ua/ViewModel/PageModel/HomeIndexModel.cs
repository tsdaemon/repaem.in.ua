using aspdev.repaem.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    public class HomeIndexModel
    {
        public GoogleMap Map { get; set; }

        public RepBaseFilter Filter { get; set; }

        public List<RepBaseListItem> NewBases { get; set; }

        public HomeIndexModel()
        {
            var db = new Database();
            NewBases = db.GetNewBases().ToList();

            Map = new GoogleMap();
            Map.Coordinates = db.GetAllBasesCoordinates();

            Filter = new RepBaseFilter();
        }
    }  
}