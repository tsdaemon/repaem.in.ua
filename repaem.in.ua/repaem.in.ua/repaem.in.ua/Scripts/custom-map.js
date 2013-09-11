$(document).ready(function () {
	var editMode = false;
	var marker = null;
	
	//устанавливаем режим редактирования по атрибуту
	$(".edit-mode").first(function () {
		editMode = true;
	});

	//инициализация
	var mapOptions = {
		center: new google.maps.LatLng($(".map-data").data("center-lat"), $(".map-data").data("center-lon")),
		zoom: 6,
		mapTypeId: google.maps.MapTypeId.ROADMAP
	};
	var map = new google.maps.Map(document.getElementById("map_canvas"),
		mapOptions);
	var infowindow = new google.maps.InfoWindow({
	});
	
	//ищем маркеры
	$(".map-data .marker").each(function() {
		var myLatLng = new google.maps.LatLng($(this).data("lat"), $(this).data("long"));
		//все маркеры идут через глобальную переменную для дальнейших операций
		marker = new google.maps.Marker({
			position: myLatLng,
			map: map,
			title: $(this).data("title").toString(),
			draggable: editMode //в режиме редактирования маркеры можно таскать
		});
		
		var self = this;
		//В режиме редактирования мне нафиг не нужно окно описания
		if (!editMode) {
			google.maps.event.addListener(marker, 'click', function() {
				infowindow.setContent($(self).html());
				infowindow.open(map, marker);
			});
		}
	});
	
	//обработчик первого клика, который должен добавить первый маркер...
	if (editMode) {
		google.maps.event.addListener(map, 'click', function () {
			//...если его еще нет
			if (marker == null) {
				//еще не знаю, будет ли это работать
				var latLng = google.maps.MouseEvent.latLng;
				marker = new google.maps.Marker({
					position: latLng,
					map: map,
					draggable: true
				});
			}
		});
	}
	
	//осталось написать геокодирование и событие перетаскивания маркера
});