var mapOptions = {
    center: new google.maps.LatLng($(".map-data").data("center-lat"), $(".map-data").data("center-lon")),
    zoom: 8,
    mapTypeId: google.maps.MapTypeId.ROADMAP
};
var map = new google.maps.Map(document.getElementById("map_canvas"),
    mapOptions);

var infowindow = new google.maps.InfoWindow({

});

$(".map-data .marker").each(function () {
    var myLatLng = new google.maps.LatLng($(this).data("lat"), $(this).data("long"));
    var marker = new google.maps.Marker({
        position: myLatLng,
        map: map,
        title: $(this).data("title").toString()
    });

    google.maps.event.addListener(marker, 'click', function () {
        infowindow.setContent($(this).innerHtml());
        infowindow.open(map, marker);
    });

});