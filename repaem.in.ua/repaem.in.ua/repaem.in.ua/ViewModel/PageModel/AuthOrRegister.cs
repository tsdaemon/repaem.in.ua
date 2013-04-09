using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace aspdev.repaem.ViewModel.PageModel
{
    public class AuthOrRegister
    {
        public Auth Auth { get; set; }

        public Register Register { get; set; }

        public AuthOrRegister()
        {
            Auth = new Auth();
            Register = new Register();
        }
    }
}