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

        public HomeIndexModel(bool demo):base()
        {
            if (demo)
            {
                Map.Coordinates.AddRange(new RepbaseInfo[] {
                new RepbaseInfo() { 
                    Lat = 50.1111f,
                    Long = 30.23445f,
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

                Filter.DisplayTpe = RepBaseFilter.DisplayType.square;

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

        public HomeIndexModel()
        {
            Map = new GoogleMap();
            Filter = new RepBaseFilter();
        }
    }  
}