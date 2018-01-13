var sData = [];
var sBuiType = '';
var sOrdeType = '';
var isMRT = false;
var vizLocation = d3plus.viz().container("#vizLocation");
var vizHistory = d3plus.viz().container("#vizHistory");
var vizTreeMap = d3plus.viz().container("#vizTreeMap");
//https://jsfiddle.net/q7Ss6/
var selectedCity = 0;
var selectedYear = 3;
$.get('home/CachedData', function (data) {
    console.log('start');
    drawTreeMap(data);
});
function SelectCity(City)
{
    if (City === 0) {
        isMRT = true;
    }
    else
    {
        isMRT = false;
    }
    $.get('home/CachedData?City=' + City, function (data) {
        console.log('Lets Go');
        drawTreeMap(data);
    });
}
function drawTreeMap(data) {
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
                        return (n.district === dp.district && n.buitype === dp.buitype);
                    });
                    sData = tmpData;
                    sBuiType = dp.buitype;
                    filterData('');
                    //http://stackoverflow.com/questions/25896553/yaxis-categories-on-scatter-plot
                    //http://jsfiddle.net/2Wr8v/1/
                }
            }
        })
        .draw();
}

function filterData(orderType, byLocation)
{
    var tmpData = [];
    var tmpLocation = $('#inputLocation').val();
    var condition1 = '';
    if (orderType === '' && sOrdeType === '') {
        sOrdeType = orderType;
        orderType = 'tprice';
    }
    else if (orderType === '' && sOrdeType !== '') {
        orderType = sOrdeType;
    }
    if (byLocation && tmpLocation !=='') {
        condition1 = " where location like '%" + tmpLocation + "%'";
    }
    else
    {
        $('#inputLocation').val('');
        condition1 = '';
    }
    console.log("select * from ? " + condition1 + " order by " + orderType + " desc");
    tmpData = alasql("select * from ? " + condition1 + " order by " + orderType + " desc", [sData]);
    console.log(tmpData);
    HCLocation(tmpData);
}

function HCLocation(filterData)
{
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
                return '總價:' + yTprice[this.points[0].point.x] + '萬, 房齡: '
                    + yAge[this.points[0].point.x] + ', <br> 坪數: '
                    + yLanda[this.points[0].point.x] + '<br>交易數量: '
                    + yCountNum[this.points[0].point.x] + '<br>地點: '
                    + filterData[this.points[0].point.x].location;
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
                    console.log(x[event.point.x]);
                    console.log(event.point.y);
                    var tempLocation = x[event.point.x];

                    GetGeo(tempLocation);
                    var url = 'Home/GetData2?location=' + tempLocation + '&buitype=' + sBuiType;
                    console.log(url);
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
                    
                    GetGeo(tempLocation);
                    var url = 'Home/GetData2?location=' + tempLocation + '&buitype=' + sBuiType;
                    console.log(url);
                    HCHistory(url, tempLocation);
                }
            }
        }]
    });
}

function HCHistory(url,location)
{
    $.get(url, function (data) {
        var x = []; 
        var yTprice = [];
        var yLanda = [];
        console.log('HCLocation');
        console.log(data);
        $.each(data, function (i, item) {
            x.push(item.sdate.substring(0, 10));
            yTprice.push(Math.round(item.tprice / 100) / 100);
            yLanda.push(item.landa);
           
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
                    format: '{value}萬元',
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
                    return '總價:' + Math.round(data[this.points[0].point.x].tprice / 100) / 100 + '萬, 房齡:' + data[this.points[0].point.x].houseage + ',<br>' + data[this.points[0].point.x].landa + '坪, ' + data[this.points[0].point.x].buildR + '房' + data[this.points[0].point.x].buildL + '廳, 層數:' + data[this.points[0].point.x].sbuild + ', <br>車位:' + (data[this.points[0].point.x].parktype === null ? 'N' : data[this.points[0].point.x].parktype) + ', 其他:' + (data[this.points[0].point.x].rmnote === null ? 'N' : data[this.points[0].point.x].rmnote);
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
                    }
                }
            }]
        });
    });
    
}



