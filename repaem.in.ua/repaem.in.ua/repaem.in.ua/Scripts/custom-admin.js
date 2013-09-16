$(document).ready(function () {
	$(".delete-item").click(function() {
		var url = $(this).data("url");
		var id = $(this).data("id");
		var self = this;

		$.ajax({
			url: url,
			type: 'DELETE',
			data: { id: id }
		}).done(function() {
			$(self).parents("tr").animate({ opacity: 'hide' }, "slow");
		});
	});
});

function reload(obj) {
	window.location.reload();
}