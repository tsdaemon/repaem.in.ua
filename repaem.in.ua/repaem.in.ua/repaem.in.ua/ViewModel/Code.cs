using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    public class Code
    {
        [Display(Name="Введите код, который вы получили в СМС")]
        public int Value { get; set; }
    }
}