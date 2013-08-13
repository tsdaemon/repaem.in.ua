using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace aspdev.repaem.Helpers
{
	public static partial class Extensions
	{
		public static HtmlString RadioButtonListFor<TModel, TValue>(this HtmlHelper<TModel> html,
		                                                        Expression<Func<TModel, TValue>> expression, List<SelectListItem> list)
		{
			var sb = new StringBuilder();
			foreach (var item in list)
			{
				sb.Append(html.RadioButtonFor(expression, item.Value) + "<span>" + item.Text + "</span>");
			}
			return new HtmlString(sb.ToString());
		}
	}
}