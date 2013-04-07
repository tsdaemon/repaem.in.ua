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

});