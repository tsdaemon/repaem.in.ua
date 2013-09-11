var link;

$(document).ready(function () {

	$(".delete-item").click(function () {
		$(this).parents("tr").animate({ opacity: 'hide' }, "slow");
		var itemid = $(this).attr("data-id");
		var path = $(this).attr("data-controler");

		$.post(path, { id: itemid });
	});

	$("a").click(function () {
		link = this;
	});
});

function deleteRow(obj) {
	
}

function reload(obj) {
	window.location.reload();
}