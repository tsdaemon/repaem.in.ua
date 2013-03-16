$('.rating').rating({
    fx: 'float',
    image: 'images/stars.png',
});

$("input[type=date]").datepicker().datepicker("option", "dateFormat", "dd.mm.yy");

$("select").autocomplete();