$(function() {
	//табы на списке комнат
	$("#rooms-list").tabs({
		activate: function (event, ui) {
			$('.fullcalendar').fullCalendar('render');
		}
	});

	//для маленьких экранов
	if (window.screen.availWidth < 1280) {
		$("#page").css("width", "90em");
	}

	//рейтинг
	$('.rating').rating({
		fx: 'half',
		image: '/images/stars.png',
		url: '/comments/vote',
		click: function(responce) {
			$('<form action="/RepBase/Rate" method="GET">' +
				'<input type="hidden" name="id" value="' + this._data["vote-id"] + '">' +
				'<input type="hidden" name="rating" value="' + responce + '">' +
				'</form>').appendTo("body").submit();
		}
	});
	
	
	//jquery ui для полей с датой
	$("input[type=date]").datepicker({ dateFormat: 'dd.mm.yy' });
	//что бы гребаный валидейт не лез со своими советами
	$("input[type=date]").removeAttr("data-val-date").removeAttr("data-val");
	
	//что бы гребаный хром не лепил свой датапикер
	if (navigator.userAgent.indexOf('Chrome') != -1) {
		$("input[type=date]").attr('type', 'text');
	}

	$("select").autocomplete();
});

$(document).ready(function() {
	//клевая галерея
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

	$(".message").each(function() {
		var mess = this;
		$(this).children(".close").click(function() {
			$(mess).fadeOut();
		});
		$(this).fadeIn().delay(5000).fadeOut();
	});

	//for the cancel request link
	$("a[data-action='cancelrep']").click(function() {
	  var iid = $(this).data("id");
	    
		$.ajax({
			type: "POST",
			url: "/Repbase/Cancel/",
			data: {
			    id: iid
			}
		}).done(function(data) {
			//TODO: разобраться какого художника не работет коллбек!
		});
		$(this).parents(".repetition").removeClass("approoved").removeClass("ordered").removeClass("constant").addClass("cancelled");
		$(this).fadeOut();
	});

	//подгружаем после загрузки страницы - в фильтре города могли сохраниться значения
	$('#Filter_City_Value').load(function() {
		if ($(this).val() != 0) {
			$.get("/Home/GetDistincts", { id: $(this).val() }, function(data) {
				$('#Filter_Distinct_Value').empty();

				var length = data.length, element = null;
				for (var i = 0; i < length; i++) {
					element = data[i];
					$('#Filter_Distinct_Value').append('<option value="' + element.Value + '">' + element.Text + '</option>');
				}
			});
		}
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
						$('<form action="'+editUrl+'" method="GET">' +
								'<input type="hidden" name="id" value="' + calEvent.id + '">' +
								'</form>').appendTo("body").submit();
					}
				}
			});
		});
	});
});