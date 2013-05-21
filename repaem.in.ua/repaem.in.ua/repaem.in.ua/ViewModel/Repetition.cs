using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    /// <summary>
    /// Репетиція
    /// </summary>
    public class Repetition
    {
        public int Id { get; set; }

        public TimeRange Time { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        //Когда этот класс используется в списке репетиций базы - Имя того, кто заказал или группы
        //Список репетиций музыканта - Имя базы
        public string Name { get; set; }

        //TODO: TO KCH - внеси такое поле в базу, пусть будет интовое (по значениям перечисления)
        public Status Status { get; set; }
    }

    public enum Status 
    {
        ordered, //только заказли
        approoved, //админы подтвердили
        constant, //постоянка
        cancelled //отменили
    }
}