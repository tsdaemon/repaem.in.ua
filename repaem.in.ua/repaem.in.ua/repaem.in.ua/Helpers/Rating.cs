using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace aspdev.repaem.Helpers
{
    public static partial class Extensions
    {
        public static HtmlString Rating(this HtmlHelper html, double val, int votes, int vote_id)
        {
	        var sb = new StringBuilder();
	        sb.Append("<div class='rating'>");
					sb.Append("<input type='hidden' name='val' value='" + val + "'/>");
					sb.Append("<input type='hidden' name='votes' value='" + votes + "'/>");
					sb.Append("<input type='hidden' name='vote-id' value='" + vote_id + "'/>");
	        sb.Append("</div>");
					if (votes > 0)
					{
						string text;
						if (votes == 1)
							text = votes + " голос";
						else if (votes > 1 && votes < 5)
							text = votes + " голоса";
						else
							text = votes + " голосов";
						sb.Append(html.ActionLink(text, "Comments", "Repbase", new { id = vote_id }, new { }));
					}
	        return new HtmlString(sb.ToString());
        }
    }
}