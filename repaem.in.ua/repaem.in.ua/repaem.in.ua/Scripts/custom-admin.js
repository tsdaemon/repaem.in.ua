$(document).ready(function () {
	$(".delete-item").click(function() {
		var url = $(this).data("url");
		var id = $(this).data("id");
		var self = this;

		$.ajax({
			url: url,
			type: 'DELETE',
			data: { id: id },
			statusCode: {
				200: function () {
					$(self).parents("tr").animate({ opacity: 'hide' }, "slow");
				}
			}
		});
	});
	
	//jquery ui для полей с датой
	$("input[type=date]").datepicker({ dateFormat: 'dd.mm.yy' });
	//что бы гребаный валидейт не лез со своими советами
	$("input[type=date]").removeAttr("data-val-date").removeAttr("data-val");
	//fucking jquery sets z-index in html, I don't whats the shit it was done so
	$(".ui-datepicker").css("zIndex", "3000");
	
	$("#RepBaseId").change(function () {
		loadRooms($(this).val());
	});
	
});

function reload(obj) {
	window.location.reload();
}

function loadRooms(id) {
	$.get("/Admin/Repetition/GetRooms", { id: id }, function (data) {
		$('#RoomId').empty();

		var length = data.length, element = null;
		for (var i = 0; i < length; i++) {
			element = data[i];
			$('#RoomId').append('<option value="' + element.Value + '">' + element.Text + '</option>');
		}
	});
}