using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.Areas.Admin.ViewModel
{
    public class UserInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int NewReps { get; set; }

        public bool NewBill { get; set; }
    }
}