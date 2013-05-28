using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.Areas.Admin.ViewModel
{
    /// <summary>
    /// Інформація про користувача
    /// </summary>
    public class UserInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Скільки нових непідтверджених репетицій
        /// </summary>
        public int NewReps { get; set; }

        /// <summary>
        /// Новий неоплаченний рахунок
        /// </summary>
        public bool UnpaidBill { get; set; }
    }
}