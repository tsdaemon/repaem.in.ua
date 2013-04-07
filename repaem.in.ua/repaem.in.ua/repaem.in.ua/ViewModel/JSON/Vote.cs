using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel
{
    public class Vote
    {
        /// <summary>
        /// Текущее значение рейтинга
        /// </summary>
        public double val { get; set; }
        /// <summary>
        /// Ид записи, которой выставляют рейтинг
        /// </summary>
        public int vote_id { get; set; }
        /// <summary>
        /// Текущий голос
        /// </summary>
        public double score { get; set; }
    }
}