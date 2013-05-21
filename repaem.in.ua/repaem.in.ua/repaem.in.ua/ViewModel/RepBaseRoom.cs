using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    /// <summary>
    /// ВьюМодел для комнаты репетиционной базы
    /// </summary>
    public class RepBaseRoom
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Фотографії кімнати
        /// </summary>
        public List<Image> Images { get; set; }

        /// <summary>
        /// Календар репетицій
        /// </summary>
        public Calendar Calendar { get; set; }

        //используется либо это, если есть сложная цена
        public List<ComplexPrice> Prices { get; set; }

        //либо это
        public float? Price { get; set; }

        public RepBaseRoom()
        {
            Images = new List<Image>();
            Calendar = new Calendar();
            Prices = new List<ComplexPrice>();
        }

        public RepBaseRoom(bool t)
            : this()
        {
            Random r = new Random(3);
            Calendar = new Calendar(true);
            Id = r.Next();
            Calendar.RoomId = Id;
            Name = "Зал 1";
            Description = @"В наличии 2 стойки (журавли) + клемп с плечем (журавль) + хайхет стойка

КАРДАН Tama Iron Cobra (20грн\репа)

Все ударные постоянно отстраиваются
барабанным тюнером

ЛИНИЯ :

-Микшер YAMAHA MG 16\4 - 12 микрофонных канала + процессор эффектов
-Портал PEAVEY PV 215 700W
-Усилитель OUTLINE HD5000 (ITALY) 2x700
-Микрофоны SHURE SM 58, SENNHEISER e 845

А ТАКЖЕ:

- Кабели.
- Гитарные стойки
- Клавишная стойка.
- Блоки питания.
- Удлинители
- Метроном.
- Наушники для ударника.

------------ BRIGHT ROOM -----------

ГИТАРНОЕ ОБОРУДОВАНИЕ:
-PEAVEY 6505 (USA) (2X12)
-CRATE BV 120 H (USA) + кабинет 4х12'

БАСОВЫЙ АППАРАТ:
SWR 350X (USA) + AMPEG 2X10'

УДАРНЫЕ :

-MAPEX MERIDIAN MAPLE
22x18
12x9 Remo Pinstripe clear
13x10 Remo Pinstripe clear
16x16 Remo Pinstripe clear
14x6 Evans EC Revers Dot
-СТУЛ ВИНТОВОЙ

ТАРЕЛКИ:

-HI HAT
-CRASH
-CRASH
-RIDE

В наличии 2 стойки (журавли) + клемп с плечем (журавль) + хайхет стойка

КАРДАН Tama Iron Cobra (20грн\репа)

Все ударные постоянно отстраиваются
барабанным тюнером

ЛИНИЯ:

-Микшер SOUNDCRAFT FX8 - 8 микрофонных канала + процессор Lexicon
-Колонки BEYMA (SPAIN) 2x700
-Усилитель PARK AUDIO VX900
-Микрофоны SHURE SM 58 - 2 шт.

А ТАКЖЕ:

- Кабели.
- Гитарные стойки
- Клавишная стойка.
- Блоки питания.
- Удлинители
- Метроном.
- Наушники для ударника. ";
            Images.Add(new Image(true));
            Images.Add(new Image(true));
            Images.Add(new Image(true));

            Prices.Add(new ComplexPrice() { Id = 1, Price = 35, EndTime = 17, StartTime = 12 });
            Prices.Add(new ComplexPrice() { Id = 2, Price = 20, EndTime = 12, StartTime = 9 });
            Prices.Add(new ComplexPrice() { Id = 3, Price = 50, EndTime = 8, StartTime = 20 });
        }
    }
}