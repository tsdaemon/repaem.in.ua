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
            Map = new GoogleMap();
            Map.Coordinates.AddRange(new RepbaseInfo[] {
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

            Filter = new RepBaseFilter();
            Filter.Date = DateTime.Today;

            NewBases = new List<RepBaseListItem>() { 
                new RepBaseListItem() { 
                    Address = "Металистов, 5",
                    Description = "Трарарарарарарарарарара",
                    Id = 14,
                    Name = "Волшебный пездюль",
                    ImageSrc = "images/big_898050.jpg",
                    Rating="3.5",
                    RatingCount=1
                },
                new RepBaseListItem() {
                    Address = "Металистов, 5",
                    Description = "Трарарарарарарарарарара",
                    Id = 14,
                    Name = "Волшебный пездюль",
                    ImageSrc = "images/big_898050.jpg",
                    Rating="2",
                    RatingCount=1
                }
            };
        }
    }
}