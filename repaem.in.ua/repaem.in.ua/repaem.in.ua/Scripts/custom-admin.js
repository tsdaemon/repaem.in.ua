

$(document).ready(function () {
	//табы на списке комнат
	
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
	
	//календарь
	$(document).ready(function () {
		$(".fullcalendar").each(function () {
			var self = this;
			var mm = $(this).data("manager-mode");
			var roomId = $(this).data("room-id");
			var addUrl = $(this).data("add-url");
			var editUrl = $(this).data("edit-url");

			$(this).fullCalendar({
				defaultView: 'agendaWeek',
				events: function (start, end, callback) {
					var events = $(self).children("input[data-type='event']").map(function () {
						var event = {};
						event.start = this.dataset.begin;
						event.end = this.dataset.end;
						event.className = this.dataset.status;
						event.title = this.dataset.name;
						event.allDay = false;
						event.id = this.dataset.id;
						return event;
					}).get();
					callback(events);
				},
				monthNames: ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль',
					'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'],
				monthNamesShort: ['Янв', 'Фев', 'Мар', 'Апр', 'Май', 'Июн', 'Июл',
					'Авг', 'Сен', 'Окт', 'Ноя', 'Дек'],
				dayNames: ['Воскресенье', 'Понедельник', 'Вторник', 'Среда',
					'Четверг', 'Пятница', 'Субота'],
				dayNamesShort: ['Вс', 'Пн', 'Вт', 'Ср',
					'Чт', 'Пт', 'Сб'],
				allDaySlot: false,
				firstHour: 8,
				firstDay: 1,
				slotMinutes: 60,
				defaultEventMinutes: 120,
				timeFormat: 'h:mm{ - h:mm}',
				buttonText: {
					prev: 'назад', // <
					next: 'вперед', // >
					prevYear: '&laquo;',  // <<
					nextYear: '&raquo;',  // >>
					today: 'сегодня',
					month: 'месяц',
					week: 'неделя',
					day: 'день'
				},
				axisFormat: "H:mm",
				dayClick: function (date, allDay, jsEvent, view) {
					if (mm) {
						$('<form action="' + addUrl + '" method="GET">' +
								'<input type="hidden" name="datetime" value="' + date.toJSON() + '">' +
								'<input type="hidden" name="roomid" value="' + roomId + '">' +
								'</form>').appendTo("body").submit();
					} else {
						if (!allDay) {
							var id = $('#Id').val();

							$('<form action="' + addUrl + '" method="GET">' +
								'<input type="hidden" name="id" value="' + id + '">' +
								'<input type="hidden" name="datetime" value="' + date.toJSON() + '">' +
								'<input type="hidden" name="roomid" value="' + roomId + '">' +
								'</form>').appendTo("body").submit();
						}
					}
				},
				eventClick: function (calEvent, jsEvent, view) {
					if (mm) {
						$('<form action="' + editUrl + '" method="GET">' +
								'<input type="hidden" name="id" value="' + calEvent.id + '">' +
								'</form>').appendTo("body").submit();
					}
				}
			});
		});
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

