using aspdev.repaem.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aspdev.repaem.Helpers
{
	public static partial class Extensions
	{
		public static string RepetitionStatus(this HtmlHelper html, Status status)
		{
			switch (status)
			{
				case Status.approoved:
					return "Подтвержена";
				case Status.cancelled:
					return "Отменена";
				case Status.constant:
					return "Постоянная";
				case Status.ordered:
					return "Заказана";
				default:
					return "";
			}
		}
	}
}