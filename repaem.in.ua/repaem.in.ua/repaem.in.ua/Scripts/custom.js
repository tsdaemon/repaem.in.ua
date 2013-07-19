$(function () {
    $("#rooms-list").tabs();
    $('.rating').rating({
        fx: 'half',
        image: '/images/stars.png',
        url: '/comments/vote',
        click: function (responce) {
            window.location.href = '/RepBase/Rate/' + this._data["vote-id"] + '/' + responce;
        }
    });

    $("input[type=date]").datepicker({ dateFormat: 'dd.mm.yy' });

    $("select").autocomplete();
});

$(document).ready(function () {

    $(".fancybox").fancybox({
        helpers: {
            overlay: {
                css: {
                    'background': 'rgba(58, 42, 45, 0.95)'
                }
            }
        }
    });

    //fucking jquery sets z-index in html, I don't whats the shit it was done so
    $(".ui-datepicker").css("zIndex", "3000");

    $(".dialog").dialog({
        autoOpen: false,
        position: 'center',
        resizable: false,
        modal: true
    });

    $("#Message").each(function () {
        var mess = this.innerHTML;
        $(document).avgrund({
            width: 200,
            height: 100,
            showClose: true, // switch to 'true' for enabling close button 
            showCloseText: '', // type your text for close button
            closeByEscape: true, // enables closing popup by 'Esc'..
            closeByDocument: true,
            holderClass: '',
            overlayClass: '',
            enableStackAnimation: true,
            onBlurContainer: '',
            openOnEvent: false,
            template: mess
        });
    });

    //for the cancel request link
    $(".cancel-rep").click(function () {
        var iid = $(this).data("id");
        $.ajax({
            url: "/repbase/cancel/",
            data: { id: iid }
        });
        $(this).parents(".repetition").removeClass("approoved").removeClass("ordered").removeClass("constant").addClass("cancelled");
        $(this).fadeOut();
    });
    
    //подгружаем районы по выбору города
    $('#Filter_City_Value').change(function () {
        $.get("/Home/GetDistincts", { id: $(this).val() }, function (data) {
            $('#Filter_Distinct_Value').empty();

            var length = data.length, element = null;
            for (var i = 0; i < length; i++) {
                element = data[i];
                $('#Filter_Distinct_Value').append('<option value="' + element.Value + '">' + element.Text + '</option>');
            }
        });
    });

    //У айфрейма какой-то слишком маленький размер
    //$('#ii_block-widget-content').style("height", "");

    //var reformalOptions = {
    //    project_id: 114709,
    //    project_host: "repaem.reformal.ru",
    //    tab_orientation: "left",
    //    tab_indent: "50%",
    //    tab_bg_color: "#1c8502",
    //    tab_border_color: "#FFFFFF",
    //    tab_image_url: "http://tab.reformal.ru/T9GC0LfRi9Cy0Ysg0Lgg0L%252FRgNC10LTQu9C%252B0LbQtdC90LjRjw==/FFFFFF/4bfb34d91c8d7fb481972ca3c84aec38/left/0/tab.png",
    //    tab_border_width: 1
    //};

    //(function () {
    //    var script = document.createElement('script');
    //    script.type = 'text/javascript'; script.async = true;
    //    script.src = ('https:' == document.location.protocol ? 'https://' : 'http://') + 'media.reformal.ru/widgets/v3/reformal.js';
    //    document.getElementsByTagName('head')[0].appendChild(script);
    //})();
});