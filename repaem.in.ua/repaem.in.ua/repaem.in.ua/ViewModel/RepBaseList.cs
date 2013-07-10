using aspdev.repaem.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aspdev.repaem.ViewModel.Home
{
    /// <summary>
    /// ВьюМодел списку репбаз
    /// </summary>
    public class RepBaseList
    {
        //TODO: BY(AST) разобраться как и добавить в эту вьюмодел постраничный просмотр 

        IDatabase db = DependencyResolver.Current.GetService<IDatabase>();

        /// <summary>
        /// Фільтр, по якому вибрані бази
        /// </summary>
        public RepBaseFilter Filter { get; set; }

        public GoogleMap Map { get; set; }

        public List<RepBaseListItem> RepBases { get; set; }

        public RepBaseList()
        {
            RepBases = new List<RepBaseListItem>();

            Map = new GoogleMap();

            Filter = new RepBaseFilter() { DisplayTpe = RepBaseFilter.DisplayType.inline };
        }

        internal void LoadAllBases()
        {
            RepBases = db.GetAllBases();
            Map.Coordinates = db.GetAllBasesCoordinates();
        }

        internal void LoadBases(RepBaseFilter f)
        {
            Filter = f;
            RepBases = db.GetBasesByFilter(f);
            Map.Coordinates = db.GetBasesCoordinatesByList(RepBases);
        }
    }
}