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
    public static class SliderExtensions
    {
        public static HtmlString RangeSliderFor(this HtmlHelper html, Range range)
        {
            try
            {
                StringBuilder sr = new StringBuilder();

                string name = html.ViewData.ModelMetadata.PropertyName;
                
                //object min = (t.GetCustomAttributes(typeof(MinAttribute), false)[0] as MinAttribute).Min;
                //object max = (t.GetCustomAttributes(typeof(MaxAttribute), false)[0] as MaxAttribute).Max;

                object min = html.ViewData.ModelMetadata.AdditionalValues["Min"];
                object max = html.ViewData.ModelMetadata.AdditionalValues["Max"];

                string val = @"
<input type='text' id='slider-range-{0}-val1' class='slider-val1' readonly/>
<input type='text' id='slider-range-{0}-val2' class='slider-val2' readonly/>";
                sr.Append(String.Format(val, name));

                val = @"<div id='slider-range-%4%' class='slider'></div>
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
            }
        });
    });
    $('#slider-range-%4%-val1').val(%2%);
    $('#slider-range-%4%-val2').val(%3%);
</script>".Replace("%0%", min.ToString())
                              .Replace("%1%", max.ToString())
                              .Replace("%2%", range.Begin.ToString())
                              .Replace("%3%", range.End.ToString())
                              .Replace("%4%", name);

                sr.Append(val);

                return new HtmlString(sr.ToString());
            }
            catch
            {
                return new HtmlString("");
            }
        }
    }

}