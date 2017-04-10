var _tData = [];
var isMRT = false;
var vizLocation = d3plus.viz()
            .container("#vizLocation");
var vizHistory = d3plus.viz()
            .container("#vizHistory");
var vizTreeMap = d3plus.viz()
    .container("#vizTreeMap");
//https://jsfiddle.net/q7Ss6/
$.get('home/CachedData', function (data) {
    console.log('start');
    drawTreeMap2(data);
});
function SelectCity(City)
{
    if (City == 0) {
        console.log("show mrt");
        isMRT = true;
    }
    else
    {
        isMRT = false;
    }
    console.log('home/CachedData?City=' + City);
    $.get('home/CachedData?City=' + City, function (data) {
        console.log('GotData!');
        drawTreeMap2(data);
        console.log(data);
    });
}
function drawTreeMap2(data) {
    vizTreeMap.data(data)
        .type("tree_map")
        .id(["district", "buitype"])
        .size("uprice")
        .title("不動產買賣! 請點擊區域 (uprice:每坪價格,  landa:坪數)")
        .color("uprice")
        .ui([
            {
                "method": "size",
                "value": ["uprice", "landa"]
            },
            {
                "method": "color",
                "value": ["uprice", "landa"]
            }
        ])
        .aggs({ "uprice": "mean" })
        .aggs({ "landa": "mean" })
        .mouse({
            "over": function (dp, tdiv) {
                if (dp.d3plus.depth === 1) {
                    var tmpData = $.grep(data, function (n, i) {
                        return (n.district === dp.district && n.buitype === dp.buitype)
                    });

                    _tData = tmpData;
                    //var tmpData = alasql("select district,location,AVG(uprice) as uprice, AVG(tprice) as tprice, AVG(houseage) as houseage, AVG(landa) as landa,COUNT(*) as countNum from ? group by district,location order by tprice desc", [tmpData]);

                    HCLocation(tmpData);

                    //http://stackoverflow.com/questions/25896553/yaxis-categories-on-scatter-plot
                    //http://jsfiddle.net/2Wr8v/1/
                }
            }
        })
        .draw();
}

function HCLocation(_data)
{
    var filterData = alasql("select * from ? order by tprice desc", [_data]);
    console.log('Chart Location');
    console.log(_data);
    var x = []; //Category
    var yTprice = [];
    var yLanda = [];
    var yAge = [];
    var yCountNum = [];
    $.each(filterData, function (i, item) {
        x.push(item.location);
        yTprice.push(Math.round(item.tprice / 100) / 100);
        yLanda.push(item.landa);
        yAge.push(item.houseage);
        yCountNum.push(item.countNum);
    });

    var yTpriceMin = Math.min(...yTprice);
    if (yTpriceMin > 10) {
        yTpriceMin = 10;
    }
    var chart = Highcharts.chart('vizHCLocation', {
        chart: {
            zoomType: 'x'
        },
        title: {
            text: '價格 vs 地段 (可縮放選擇詳細資料)'
        },
        subtitle: {
            text: 'TPRICE:總價(萬元)、LANDA:土地移轉總面積(坪)'
        }, xAxis: [{
            categories: x,
            crosshair: true
        }],
        yAxis: [{ // Primary yAxis for uprice
            labels: {
                format: '{value}°萬元',
                style: {
                    color: Highcharts.getOptions().colors[0]
                }
            },
            title: {
                text: '交易總價(萬元)',
                style: {
                    color: Highcharts.getOptions().colors[0]
                }
            },
            opposite: false,
            gridLineWidth: 2,
            min: yTpriceMin
        }, { // Secondary yAxis
            title: {
                text: '土地移轉總面積(坪)',
                style: {
                    color: Highcharts.getOptions().colors[1]
                }
            },
            labels: {
                format: '{value} 坪',
                style: {
                    color: Highcharts.getOptions().colors[2]
                }
            },
            opposite: true
        }], tooltip: {
            formatter: function () {
                return '總價:' + yTprice[this.points[0].point.x] + '萬, 房齡:' + yAge[this.points[0].point.x] + ', <br> 坪數: ' + yLanda[this.points[0].point.x] + '<br>2016交易數量: ' + yCountNum[this.points[0].point.x];
            },
            //pointFormat: '{series.name} : <b>{point.y}</b><br/>'+''+'<br/>',
            shared: true
        },
        legend: {
            layout: 'vertical',
            align: 'left',
            x: 80,
            verticalAlign: 'top',
            y: 55,
            floating: true,
            backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'
        },
        series: [{
            name: '總價',
            type: 'spline',
            yAxis: 0,
            data: yTprice,
            lineWidth: 6,
            marker: {
                lineWidth: 2,
                radius: 6
            },
            tooltip: {
                valueSuffix: ' 萬元'
            },
            events: {
                click: function (event) {
                    var tempLocation = x[event.point.x];
                    GetGeo(tempLocation);
                    var url = 'home/GetData2?location=' + tempLocation;
                    //d3History(url,dp);
                    HCHistory(url, tempLocation);
                }
            }
        }, {
            name: '面積',
            type: 'areaspline',
            yAxis: 1,
            data: yLanda,
            lineColor: Highcharts.getOptions().colors[2],
            marker: {
                symbol: 'circle',
                enabled: false,
                status: {
                    hover: {
                        linecolor: 'red',
                        linewidth: 2,
                        fillcolor: 'white',
                        radius: 20
                    }
                }
            },
            fillColor: {
                linearGradient: [0, 0, 0, 300],
                stops: [[0, Highcharts.getOptions().colors[2]], [1, Highcharts.Color(Highcharts.getOptions().colors[2]).setOpacity(0).get('rgba')]]
            },
            tooltip: {
                valueSuffix: ' 坪'
            },
            events: {
                click: function (event) {
                    console.log(x[event.point.x]);
                    console.log(event.point.y);
                    var tempLocation = x[event.point.x];

                    console.log(url);
                    GetGeo(tempLocation);
                    var url = 'Home/GetData2?location=' + tempLocation;
                    console.log(url);
                    //d3History(url,dp);
                    HCHistory(url, tempLocation);
                }
            }
        }]
    });


    
}

function HCHistory(url,location)
{
    $.get(url, function (data) {
        var x = []; //Category
        var yTprice = [];
        var yLanda = [];
        var yAge = [];
        var ySBuild = [];
        var yBuitype = [];
        var yBuildR = [];
        var yBuildL = [];
        var yParktype = [];
        var yRmnote = [];
        console.log('HCLocation');
        console.log(data);
        $.each(data, function (i, item) {
            x.push(item.sdate.substring(0, 10));
            yTprice.push(Math.round(item.tprice / 100) / 100);
            yLanda.push(item.landa);
            yAge.push(item.houseage);
            yBuitype.push(item.buitype);
            yBuildR.push(item.buildR);
            yBuildL.push(item.buildL);
            if (item.sbuild == null) {
                ySBuild.push('N');
            }
            else {
                ySBuild.push(item.sbuild);
            }
            if (item.parktype == null) {
                yParktype.push('N');
            }
            else {
                yParktype.push(item.parktype);
            }
            if (item.rmnote == null) {
                yRmnote.push('N');
            }
            else {
                yRmnote.push(item.rmnote);
            }
        });
        var yTpriceMin = Math.min(...yTprice);
        if (yTpriceMin > 10) {
            yTpriceMin = 10;
        }
        Highcharts.chart('vizHistory', {
            chart: {
                zoomType: 'xy'
            },
            title: {
                text: location+' 歷史交易紀錄'
            },
            subtitle: {
                text: 'TPRICE:交易總價(萬元)、LANDA:土地移轉總面積(坪)'
            },
            xAxis: [{
                categories: x,
                crosshair: true
            }],
            yAxis: [{ // Primary yAxis for uprice
                labels: {
                    format: '{value}°萬元',
                    style: {
                        color: Highcharts.getOptions().colors[0]
                    }
                },
                title: {
                    text: '交易總價(萬元)',
                    style: {
                        color: Highcharts.getOptions().colors[0]
                    }
                },
                opposite: false,
                gridLineWidth: 2,
                min: yTpriceMin
            }, { // Secondary yAxis
                title: {
                    text: '土地移轉總面積(坪)',
                    style: {
                        color: Highcharts.getOptions().colors[1]
                    }
                },
                labels: {
                    format: '{value} 坪',
                    style: {
                        color: Highcharts.getOptions().colors[2]
                    }
                },
                opposite: true
            }],
            tooltip: {
                formatter: function () {
                    return '總價:' + yTprice[this.points[0].point.x] + '萬, 房齡:' + yAge[this.points[0].point.x] + ',<br>'+yLanda[this.points[0].point.x]+'坪, ' + yBuildR[this.points[0].point.x] + '房' + yBuildL[this.points[0].point.x] + '廳, 層數:' + ySBuild[this.points[0].point.x] + ', <br>車位:' + yParktype[this.points[0].point.x] + ', 其他:' + yRmnote[this.points[0].point.x];
                },
                //pointFormat: '{series.name} : <b>{point.y}</b><br/>'+''+'<br/>',
                shared: true
            },
            legend: {
                layout: 'vertical',
                align: 'left',
                x: 80,
                verticalAlign: 'top',
                y: 55,
                floating: true,
                backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'
            },
            series: [{
                name: '總價',
                type: 'spline',
                yAxis: 0,
                data: yTprice,
                lineWidth: 6,
                marker: {
                    lineWidth: 2,
                    radius: 6
                },
                tooltip: {
                    valueSuffix: ' 萬元'
                },
                events: {
                    click: function (event) {
                        console.log(event.point.x);
                        console.log(event.point.y);
                    }
                }
            }, {
                name: '面積',
                type: 'areaspline',
                yAxis: 1,
                data: yLanda,
                lineColor: Highcharts.getOptions().colors[2],
                marker: {
                    symbol: 'circle',
                    enabled: false,
                    status: {
                        hover: {
                            linecolor: 'red',
                            linewidth: 2,
                            fillcolor: 'white',
                            radius: 20
                        }
                    }
                },
                fillColor: {
                    linearGradient: [0, 0, 0, 300],
                    stops: [[0, Highcharts.getOptions().colors[2]], [1, Highcharts.Color(Highcharts.getOptions().colors[2]).setOpacity(0).get('rgba')]]
                },
                tooltip: {
                    valueSuffix: ' 坪'
                },
                events: {
                    click: function (event) {
                        /*console.log(event.point.x);
                        console.log(event.point.y);*/
                    }
                }
            }]
        });
    });
    
}

function d3History(url)
{
    $.get(url, function (data2,dp) {
        console.log(data2);
        $.each(data2, function (i, item) {
            item.sdate = item.sdate.substring(0, 10);
        })
        vizHistory.data(data2)
        .type("bar")
        .id("sdate")
        .title("地址:" + dp.location + " 歷史交易單價(萬元/坪)")
        .x("sdate").y("uprice")
        .order({ "value": "sdate", "sort": "asc" })
        .color("uprice")
        .draw();
    });
}


function RealPrice(City)
{
    this.City = City
}
