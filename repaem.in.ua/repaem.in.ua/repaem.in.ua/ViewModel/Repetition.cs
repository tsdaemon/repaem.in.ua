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

        [Display(Name="Время")]
        public TimeRange Time { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата")]
        public DateTime Date { get; set; }

        //Когда этот класс используется в списке репетиций базы - Имя того, кто заказал или группы
        //Список репетиций музыканта - Имя базы
        public string Name { get; set; }

        public Status Status { get; set; }

        [Display(Name = "Стоимость")]
        public int Sum { get; set; }

        [Display(Name = "Комментарии")]
        public string Comment { get; set; }

        public Repetition()
        {
            Time = new TimeRange();
        }
    }

    public enum Status 
    {
        ordered, //только заказли
        approoved, //админы подтвердили
        constant, //постоянка
        cancelled //отменили
    }
}