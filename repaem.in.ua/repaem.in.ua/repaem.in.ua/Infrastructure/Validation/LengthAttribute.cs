using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace aspdev.repaem.Infrastructure
{
	[AttributeUsage(AttributeTargets.Property)]
	public class LengthAttribute : ValidationAttribute
	{
		public int Min { get; set; }
		public int Max { get; set; }

		public override bool IsValid(object value)
		{
			if (value == null)
			{
				if (Min == 0)
					return true;
				else
					return false;
			}

			int length = value.ToString().Length;
			if (Min > 0 && length <= Min)
				return false;
			if (Max > 0 && length > Max)
				return false;

			return true;
		}

		public override bool RequiresValidationContext
		{
			get
			{
				return false;
			}
		}

		public override string FormatErrorMessage(string name)
		{
			if (Min > 0 && Max > 0)
				return String.Format("{0} должен быть больше {1} и меньше {2} символов", name, Min, Max);
			else if (Min > 0 && Max == 0) 
				return String.Format("{0} должен быть больше {1} символов", name, Min);
			else if (Min == 0 && Max > 0)
				return String.Format("{0} должен быть меньше {1} символов", name, Min);

			return null;
		}
	}
}