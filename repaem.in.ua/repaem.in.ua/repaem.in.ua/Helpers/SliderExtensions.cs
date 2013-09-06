using System;
using System.Collections.Generic;
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
        public static HtmlString RangeSliderFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            try
            {
                var sr = new StringBuilder();
                var mt = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
                TValue val = expression.Compile().Invoke(html.ViewData.Model);
                var range = val as Range;

                string name = html.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression));
                string propertyname = mt.PropertyName;

                object min = mt.AdditionalValues["Min"];
                object max = mt.AdditionalValues["Max"];

                var tr1 = new TagBuilder("input");
                tr1.Attributes.Add("type", "hidden");
                tr1.Attributes.Add("id", String.Format("slider-range-{0}-val1", propertyname));
                tr1.Attributes.Add("name", name+".Begin");
                tr1.AddCssClass("slider-val1");
                tr1.Attributes.Add("readonly", "readonly");
                sr.Append(tr1.ToString());

                var tr2 = new TagBuilder("input");
                tr2.Attributes.Add("type", "hidden");
                tr2.Attributes.Add("id", String.Format("slider-range-{0}-val2", propertyname));
                tr2.Attributes.Add("name", name + ".End");
                tr2.AddCssClass("slider-val2");
                tr2.Attributes.Add("readonly", "readonly");
                sr.Append(tr2.ToString());

                var java = @"От <span id='slider-range-%4%-text1'>%2%</span> до <span id='slider-range-%4%-text2'>%3%</span>
<div id='slider-range-%4%' class='slider'></div>
<script> 
    $(function() {
        $('#slider-range-%4%').slider({
            range: true,
            min: %0%,
            max: %1%,
            values: [ %2%, %3% ],
            slide: function( event, ui ) {
                $('#slider-range-%4%-val1').val(ui.values[0]);
                $('#slider-range-%4%-val2').val(ui.values[1]);
                $('#slider-range-%4%-text1').text(ui.values[0]);
                $('#slider-range-%4%-text2').text(ui.values[1]);
            }
        });
    });
    $('#slider-range-%4%-val1').val(%2%);
    $('#slider-range-%4%-val2').val(%3%);
</script>".Replace("%0%", min.ToString())
                              .Replace("%1%", max.ToString())
                              .Replace("%2%", range.Begin.ToString())
                              .Replace("%3%", range.End.ToString())
                              .Replace("%4%", propertyname);

                sr.Append(java);

                return new HtmlString(sr.ToString());
            }
            catch
            {
                return new HtmlString("");
            }
        }

        public static string ImagePath(this UrlHelper url, string path)
        {
            if (path != null)
                return url.Content(path);
            else
                return url.Content("Images/help.png");
        }
    }

}