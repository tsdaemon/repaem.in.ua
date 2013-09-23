using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace aspdev.repaem.Helpers
{
	public static partial class Extensions
	{
		public static HtmlString DeleteItemLink(this HtmlHelper html, string title, int id, string url)
		{
			string a = string.Format("<a class='delete-item'  href='javascript:void(0)' data-id='{0}' data-url='{1}'>{2}</a>", id, url, title);
			return new HtmlString(a);
		}
	}
}