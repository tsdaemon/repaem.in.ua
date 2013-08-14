var mapOptions = {
	center: new google.maps.LatLng($(".map-data").data("center-lat"), $(".map-data").data("center-lon")),
	zoom: 6,
	mapTypeId: google.maps.MapTypeId.ROADMAP
};
var map = new google.maps.Map(document.getElementById("map_canvas"),
	mapOptions);

var infowindow = new google.maps.InfoWindow({
	
});

$(".map-data .marker").each(function() {
	var myLatLng = new google.maps.LatLng($(this).data("lat"), $(this).data("long"));
	var marker = new google.maps.Marker({
		position: myLatLng,
		map: map,
		title: $(this).data("title").toString()
	});
	var self = this;

	google.maps.event.addListener(marker, 'click', function() {
		infowindow.setContent($(self).html());
		infowindow.open(map, marker);
	});

});

$(".edit-mode").first(function() {
	var coordinates = $("input[name=coordinates]").val();

	if (coordinates != null) {
		var lat = coordinates.split(";")[0];
		var lng = coordinates.split(";")[1];

		var myLatLng = new google.maps.LatLng(parseFloat(lat), parseFloat(lng));
		var marker = new google.maps.Marker({
			position: myLatLng,
			map: map,
			draggable: true,
			animation: google.maps.Animation.DROP,
		});
	}

	google.maps.event.addListener(marker, 'dragend', function() {
		$("input[name=coordinates]").val(marker.getPosition())
	});
});