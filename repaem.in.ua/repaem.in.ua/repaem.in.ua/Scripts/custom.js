$(function () {
    $("#rooms-list").tabs();
    $('.rating').rating({
        fx: 'half',
        image: '/images/stars.png',
        url: '/comments/vote',
        callback: function(responce){
           //TODO: BY AST: Отримати ід новго коммента, визвати даілог для редагування коммента
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
    
});