$(function () {
    $("#rooms-list").tabs();
    $('.rating').rating({
        fx: 'half',
        image: '/images/stars.png',
        url: '/comments/vote',
        click: function (responce) {
            $('<form action="/RepBase/Rate" method="GET">' +
                '<input type="hidden" name="id" value="' + this._data["vote-id"] + '">' +
                '<input type="hidden" name="rating" value="' + responce + '">' +
                '</form>').appendTo("body").submit();

            //window.location.href = '/RepBase/Rate/' + this._data["vote-id"] + '/' + responce;
        }
    });

    //jquery ui для полей с датой
    $("input[type=date]").datepicker({ dateFormat: 'dd.mm.yy' });
    //что бы гребаный валидейт не лез со своими советами
    $("input[type=date]").removeAttr("data-val-date").removeAttr("data-val");

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

    //подгружаем после загрузки страницы - в фильтре города могли сохраниться значения
    $('#Filter_City_Value').load(function () {
        if ($(this).val() != 0) {
            $.get("/Home/GetDistincts", { id: $(this).val() }, function (data) {
                $('#Filter_Distinct_Value').empty();

                var length = data.length, element = null;
                for (var i = 0; i < length; i++) {
                    element = data[i];
                    $('#Filter_Distinct_Value').append('<option value="' + element.Value + '">' + element.Text + '</option>');
                }
            });
        }
    });
});