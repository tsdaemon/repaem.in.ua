$(document).ready(function () {
	var editMode = false;
	var marker = null;
	
	//устанавливаем режим редактирования по атрибуту
	$(".edit-mode").each(function () {
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
			draggable: editMode  //в режиме редактирования маркеры можно таскать
		});
		
		var self = this;
		//В режиме редактирования мне нафиг не нужно окно описания
		if (!editMode) {
			google.maps.event.addListener(marker, 'click', function() {
				infowindow.setContent($(self).html());
				infowindow.open(map, marker);
			});
		} else {
			google.maps.event.addListener(marker, 'dragend', setCoordinates);
		}
	});
	
	//обработчик первого клика, который должен добавить первый маркер...
	if (editMode) {
		google.maps.event.addListener(map, 'click', function (e) {
			//...если его еще нет
			if (marker == null) {
				//еще не знаю, будет ли это работать
				
				marker = new google.maps.Marker({
					position: e.latLng,
					map: map,
					draggable: true
				});
				
				google.maps.event.addListener(marker, 'dragend', setCoordinates);

				setCoordinates(e);
			}
		});
	}
	
	geocoder = new google.maps.Geocoder();
	
	function setCoordinates(e) {
		//в каждой модели, которая будет заниматься редактированием карты, должны быть такие инпуты для координат...
		var latLng = e.latLng;
		$("input[name='Lat']").val(latLng.lat());
		$("input[name='Long']").val(latLng.lng());
		//...и для адреса
		//геокодирование из OSM
		$.getJSON("http://nominatim.openstreetmap.org/reverse?json_callback=?", {
				format: "json",
				accept_language: "ru-RU",
				lat: latLng.lat(),
				lon: latLng.lng(),
				zoom: 18,
				addressdetails: 1
		}, function (data) {
			//если адрес найден, заполняем
				if (data.address) {
					var cityName;
					if (data.address.city) {
						cityName = data.address.city;
					} else if (data.address.village) {
						cityName = data.address.village;
					} else if (data.address.town) {
						cityName = data.address.town;
					} else {
						cityName = data.address.state;
					}
					var address = data.address.road;
					if (data.address.house_number) {
						address = address + ", " + data.address.house_number;
					}
					$("input[name='CityName']").val(cityName);
					$("#cityName").text(cityName);
					if (address) {
						$("input[name='Address']").val(address);
					} else {
						$("input[name='Address']").val('');
					}
				} else {
					$("input[name='CityName']").val('');
					$("#cityName").text('');
					$("input[name='Address']").val('');
				}
			});
	}
});

