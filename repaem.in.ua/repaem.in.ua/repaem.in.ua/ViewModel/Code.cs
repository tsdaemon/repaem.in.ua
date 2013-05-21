using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    /// <summary>
    /// СМС-код
    /// </summary>
    public class Code
    {
        [Display(Name="Введите код, который вы получили в СМС")]
        public int Value { get; set; }
    }
}