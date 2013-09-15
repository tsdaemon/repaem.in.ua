using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using aspdev.repaem.ViewModel;
using DataAnnotationsExtensions;

namespace aspdev.repaem.Helpers
{
  public static partial class Extensions
  {
    public static HtmlString HiddenDoubleFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
    {
      try
      {
        var sr = new StringBuilder();
        var mt = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
        TValue val = expression.Compile().Invoke(html.ViewData.Model);
				double value = Convert.ToDouble(val);
        string name = html.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));

        var tr1 = new TagBuilder("input");
        tr1.Attributes.Add("type", "hidden");
        tr1.Attributes.Add("id", name.Replace('.','_'));
				tr1.Attributes.Add("value", value.ToString("0.000000", CultureInfo.InvariantCulture));
        tr1.Attributes.Add("name", name);
        sr.Append(tr1.ToString());

        return new HtmlString(sr.ToString());
      }
      catch
      {
        return new HtmlString("");
      }
    }
  }
}