$(function() {
	//табы на списке комнат
	$("#rooms-list").tabs();
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
	$(".cancel-rep").click(function() {
	    var iid = $(this).data("id");
	    var one = $(this).data("one");
		$.ajax({
			type: "POST",
			url: "/Repbase/Cancel/",
			data: {
			    id: iid,
                one: one
			}
		}).done(function(data) {
			//TODO: разобраться какого художника не работет коллбек!
		});
		$(this).parents(".repetition").removeClass("approoved").removeClass("ordered").removeClass("constant").addClass("cancelled");
		$(this).fadeOut();
	});

	//подгружаем районы по выбору города
	$('#Filter_City_Value').change(function() {
		$.get("/Home/GetDistincts", { id: $(this).val() }, function(data) {
			$('#Filter_Distinct_Value').empty();

			var length = data.length, element = null;
			for (var i = 0; i < length; i++) {
				element = data[i];
				$('#Filter_Distinct_Value').append('<option value="' + element.Value + '">' + element.Text + '</option>');
			}
		});
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
});