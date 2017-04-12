
var _lat = 0;
var _lng = 0;
function GetGeo(address) {
    //var address = $("#Text1").val();
    var url = 'https://maps.googleapis.com/maps/api/geocode/json?address=' + address + '&key=AIzaSyBlEnOEgknWMReRy_XAKq2ars1I0zhEuc8';
    $.getJSON(url, function (data) {
        console.log('GetJson');
        console.log(address);
        console.log(data);
        if (data.results.length > 0) {
            _lat = data.results[0].geometry.location.lat;
            _lng = data.results[0].geometry.location.lng;
            console.log('lat = ' + _lat + ',   lng = ' + _lng);
            GoMap(_lat, _lng, address);
        }
        else {
            console.log('no result');
        }
    });

}

function initMap() {
    var map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: 25.167937, lng: 121.442566 },
        scrollwheel: false,
        zoom: 15
    });

}

function GoMap(_lat, _lng, _address) {
    var map = new google.maps.Map(document.getElementById('map'), {
        center: { lat: _lat, lng: _lng },
        scrollwheel: true,
        zoom: 15

    });
    console.log(map);
    var myLatLng = { lat: _lat, lng: _lng };
    var marker = new google.maps.Marker({
        map: map,
        position: myLatLng,
        title: _address
    });

};



var geocoder;
function initialize() {
    geocoder = new google.maps.Geocoder();
    var myOptions = {
        zoom: 15,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    }
    map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
    codeAddress();
}
function codeAddress() {
    var address = ''
    if (geocoder) {
        geocoder.geocode({ 'address': address }, function (results, status) {
            if (status === google.maps.GeocoderStatus.OK) {
                map.setCenter(results[0].geometry.location);
                var marker = new google.maps.Marker({
                    map: map,
                    position: results[0].geometry.location
                });
            } else {
                alert("Geocode was not successful for the following reason: " + status);
            }
        });
    }
}
