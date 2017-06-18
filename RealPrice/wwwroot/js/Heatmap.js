
var heatmapData = [
    { location: new google.maps.LatLng(25.156397, 121.4619005), weight: 2.5 },
    new google.maps.LatLng(25.1760334, 121.4502566),
    new google.maps.LatLng(25.1760334, 121.4402566),
    new google.maps.LatLng(25.1760334, 121.4302566),
    { location: new google.maps.LatLng(25.1747875, 121.4369468), weight: 2 }

];


$.get('../api/Heatmap', function (data) {
    console.log(data); heatmapData = [];
    for (var i = 0; i <data.length; i++)
    {
        //var _latLng = new google.maps.LatLng(data[i].lat, data[i].lon);
        var _weight =data[i].avg;

        //console.log(_latLng);
        var weightedLoc = { location: new google.maps.LatLng(data[i].lat, data[i].lon),weight:data[i].avg};
        heatmapData.push(weightedLoc);
        //console.log(weightedLoc);
    }
    console.log(heatmapData);
    var sanFrancisco1 = new google.maps.LatLng(37.785, -122.437);

    var sanFrancisco = new google.maps.LatLng(25.1747875, 121.4369468);

    map = new google.maps.Map(document.getElementById('map'), {
        center: sanFrancisco,
        zoom: 13,
        mapTypeId: 'satellite'
    });

    var heatmap = new google.maps.visualization.HeatmapLayer({
        data: heatmapData,
        dissipating: true,
        map: map,
        radius: 20,
        gradient: [
            'rgba(0, 255, 0, 0)',
            'rgba(0, 255, 50, 1)',
            'rgba(0, 191, 100, 1)',
            'rgba(0, 127, 200, 1)',
            'rgba(0, 63, 255, 1)',
            'rgba(0, 0, 200, 1)',
            'rgba(127, 0, 63, 1)',
            'rgba(191, 0, 31, 1)',
            'rgba(255, 0, 0, 1)'

        ]

    });
    heatmap.setMap(map);

});
