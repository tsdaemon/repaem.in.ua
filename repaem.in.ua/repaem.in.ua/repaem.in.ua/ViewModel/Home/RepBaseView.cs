﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel.Home
{
    public class RepBaseView
    {
        public RepBaseFilter Filter { get; set; }

        public GoogleMap Map { get; set; }

        public List<RepBaseListItem> RepBases { get; set; }

        public RepBaseView(bool demo)
        {
            if(demo) 
            {
                Filter = new RepBaseFilter(demo);

                Map = new GoogleMap(demo);

                RepBases = new List<RepBaseListItem>(){ 
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
                    },
                    new RepBaseListItem() {
                        Address = "Металистов, 5",
                        Description = "Трарарарарарарарарарара",
                        Id = 14,
                        Name = "Волшебный пездюль",
                        ImageSrc = "images/big_898050.jpg",
                        Rating="2",
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
                    },
                    new RepBaseListItem() {
                        Address = "Металистов, 5",
                        Description = "Трарарарарарарарарарара",
                        Id = 14,
                        Name = "Волшебный пездюль",
                        ImageSrc = "images/big_898050.jpg",
                        Rating="2",
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

        public RepBaseView()
        {
            RepBases = new List<RepBaseListItem>();

            Map = new GoogleMap();

            Filter = new RepBaseFilter();
        }
    }
}