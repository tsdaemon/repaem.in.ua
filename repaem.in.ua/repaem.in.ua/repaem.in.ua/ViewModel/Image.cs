﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    //TODO: TO (Kostya) Зробити в базі таблицю для зображень. Ід, опис, шлях до нього, шлях до маленької копії
    //TODO: TO AST а смисл цього класу, якщо з моделі бази буде клас, який за це відповідає?
    //TODO: TO KCH Класс для роботи с базою - це інший клас. Цей клас для передачі данних у вьюху
    //TODO: TO KCH RTFM паттерн MVVM для розуміння ситуації.
    //в принципі, звідси його моджна буде потім видалити, якщо в базі буде такий самий клас. Коли я малював вьюхи мені потрібен був якийсь класс
    public class Image
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public string ThumbSrc { get; set; }

        public string Src { get; set; }

        public Image()
        {

        }

        public Image(bool t)
            : this()
        {
            Id = 1;
            Description = "11111";
            Src = "/Images/help.png";
            ThumbSrc = "/Images/help.png";
        }
    }
}