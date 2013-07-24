using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

namespace aspdev.repaem.ViewModel
{
    public class RepaemColor
    {
        string _value;
        public string Value { get { return _value; } set {
                switch (value)
                {
                    case "green":  _value = "#89C403";
                        break;
                    case "orange": _value = "#C43D03";
                        break;
                    case "purple": _value = "#C4032A";
                        break;
                    case "blue": _value = "#03C49D";
                        break;
                    case "blue2": _value = "#038AC4";
                        break;
                    case "yellow": _value = "#C5FC45";
                        break;
                    case "violet": _value = "#C4038A";
                        break;
                    default: _value = value;
                        break;
                }
            } 
        }
            
        /// <summary>
        /// Добавляем или отнимаем всех компонентов
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public string GetDarkerOrLighter(SByte b) 
        {
            string v = _value.Substring(1).Insert(0, "00");
            Color c = Color.FromArgb(Convert.ToInt32(v, 16));
            v = String.Format("#{0:X2}{1:X2}{2:X2}", ZeroOrGreater(c.R + b), ZeroOrGreater(c.G + b), ZeroOrGreater(c.B + b));
            return v;
        }

        private int ZeroOrGreater(int p)
        {
            if (p < 0)
                return 0;
            else
                return p;
        }

        public RepaemColor(string val)
        {
            Value = val;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}