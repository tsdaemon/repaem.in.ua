using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    public class RepBase
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display (Name="Город")]
        public string City { get; set; }

        [Display(Name = "Адрес")]
        public string Address { get; set; }

        public string Coordinates { get; set; }

        [Display(Name = "Контакты")]
        public string Contacts { get; set; }

        [Display(Name = "Описание базы")]
        public string Description { get; set; }

        [Display(Name = "Цена")]
        public int Price { get; set; }

        public List<Image> Images { get; set; }

        public GoogleMap Map { get; set; }

        [Display(Name = "Есть время")]
        public List<HaveTime> Free { get; set; }

        [UIHint("RepBaseRoomList")]
        public List<RepBaseRoom> Rooms { get; set; }

        public double Rating { get; set; }

        public RepBase()
        {
            Images = new List<Image>();
            Map = new GoogleMap();
            Free = new List<HaveTime>();
            Rooms = new List<RepBaseRoom>();
        }

        public RepBase(bool t):this()
        {
            Images.Add(new Image(true));
            Images.Add(new Image(true));
            Images.Add(new Image(true));

            Free.Add(new HaveTime() { Date = DateTime.Today.AddDays(1), RoomName = "Зал 1", TimeStart = 17, TimeStop = 20 });
            Free.Add(new HaveTime() { Date = DateTime.Today, RoomName = "Зал 2", TimeStart = 15, TimeStop = 16 });

            Rooms.Add(new RepBaseRoom(true));
            Rooms.Add(new RepBaseRoom(true));
            Rooms[1].Id = 2;
            Rooms.Add(new RepBaseRoom(true));
            Rooms[2].Id = 3;

            Id = 14;
            Name = "Костяная нога ПРОДАКШН";
            City = "Киев";
            Address = "Металистов, 5";
            Contacts = "0956956757 Vasya 0649898493 Kolya";
            Description = @"
ГИТАРНОЕ ОБОРУДОВАНИЕ:
-MESA BOOGIE SINGLE RECTIFIRE + кабинет 2х12' (celestion v30)
-CRATE BV120 H (USA) + кабинте 2х12' (celestion v30)

БАСОВЫЙ АППАРАТ:
AMPEG SVT 3 Pro (USA) + кабинет AMPEG 4х10'

УДАРНЫЕ :

-TAMA SUPERSTAR HYPER DRIVE
22x20
10x6/5 Remo Pinstripe coated
12x7 Remo Pinstripe coated
16x14 Remo Pinstripe coated
13x6 Evans power center
-СТУЛ Gibraltar

ТАРЕЛКИ:

-HI HAT
-CRASH
-CRASH
-RIDE";
            Rating = 4.5;
        }
    }
}