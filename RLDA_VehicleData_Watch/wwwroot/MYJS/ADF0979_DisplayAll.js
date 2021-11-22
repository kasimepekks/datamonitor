function getNowFormatDate() {
    var date = new Date();
    var seperator1 = "-";
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    var strDate = date.getDate();
    if (month >= 1 && month <= 9) {
        month = "0" + month;
    }
    if (strDate >= 0 && strDate <= 9) {
        strDate = "0" + strDate;
    }
    var currentdate = year + seperator1 + month + seperator1 + strDate;
    return currentdate;
}

var fileinfo = [];
var map;
var lushu;

var polyline;
layui.use(['element', 'layer', 'table', 'form'], function () {
    var $ = layui.jquery;
    layer = layui.layer;

    $.ajax({

        type: "POST",
        //请求的媒体类型
        dataType: 'text',//这里改为json就不会传回success需要的数据了
        //请求地址
        url: urlfilewatcher,
        data: {
            _vehicleID: "ADF0979"
        },
       
    });




    var hideorshow = false;
    $(".third-class").on('click', function () {
        //$(".layui-nav-third-child").hide();
        if (!hideorshow) {
            $(this).next().show();
            hideorshow = true;
        }
        else {
            $(this).next().hide();
            hideorshow = false;
        }
    });

    var form = layui.form;
    //var table = layui.table;
    element = layui.element;
    var n = 0;
    var id = "";

    $(".layui-table-box").hide();
    form.on('switch(tabledisplay)', function (data) {

        if (data.elem.checked) {
            $(".layui-table-box").show();

        }
        else {
            $(".layui-table-box").hide()
        };

    });
   
   

    //拿到一个csv文件中的所有数据

    if (navigator.onLine) {


        map = new BMap.Map("allmap");

        var startpoint = new BMap.Point(121.472644, 31.231706);
        map.centerAndZoom(startpoint, 17);
    }

    var connection = new signalR.HubConnectionBuilder().withUrl("/MyHub").build();
    //connection.serverTimeoutInMilliseconds = 240000;
    //connection.keepAliveIntervalInMilliseconds = 120000;
    //if (navigator.onLine) {
       

    //}
    connection.on("SpeedtoDistance", function (_vehicleID, distance, speed, brake, Lat, Lon, zerotime) {
        var number = speed.length;
        //判断服务器传过来的是哪辆车就显示哪辆车的信息，因为每辆车的数据源不一样
        if (_vehicleID == "ADF0979") {
            $("#distance").text(distance.toFixed(2));
            var myChart = echarts.init(document.getElementById('speedchart'));
            //var DisChart = echarts.init(document.getElementById('dischart'));
            var BrakeChart = echarts.init(document.getElementById('brakechart'));
            var speedoption, disoption, brakeoption;
            speedoption = {

                series: [{
                    type: 'gauge',
                    //radius: '55%',
                    min: 0,
                    max: 240,
                    axisLine: {
                        lineStyle: {
                            width: 15,
                            color: [
                                [0.3, '#67e0e3'],
                                [0.7, '#37a2da'],
                                [1, '#fd666d']
                            ]
                        }
                    },
                    pointer: {
                        itemStyle: {
                            color: 'auto'
                        }
                    },
                    axisTick: {
                        distance: -40,
                        length: 8,
                        lineStyle: {
                            color: '#fff',
                            width: 2
                        }
                    },
                    //center: ['100%', '50%'], 

                    splitLine: {
                        distance: -30,
                        length: 30,
                        lineStyle: {
                            color: '#fff',
                            width: 4
                        }
                    },
                    axisLabel: {
                        color: 'auto',
                        distance: 40,
                        fontSize: 10
                    },
                    detail: {
                        valueAnimation: true,
                        fontSize: 15,
                        formatter: '{value} km/h',
                        color: 'auto'
                    },
                    data: [{
                        value: speed[0].toFixed(0)
                    }]
                }]

            };

            brakeoption = {
                series: [{
                    type: 'gauge',
                    //radius: '55%',
                    min: 0,
                    max: 100,
                    axisLine: {
                        lineStyle: {
                            width: 15,
                            color: [
                                [0.3, '#67e0e3'],
                                [0.7, '#37a2da'],
                                [1, '#fd666d']
                            ]
                        }
                    },
                    pointer: {
                        itemStyle: {
                            color: 'auto'
                        }
                    },
                    axisTick: {
                        distance: -30,
                        length: 8,
                        lineStyle: {
                            color: '#fff',
                            width: 2
                        }
                    },
                    //center: ['100%', '50%'], 
                    splitLine: {
                        distance: -30,
                        length: 30,
                        lineStyle: {
                            color: '#fff',
                            width: 4
                        }
                    },
                    axisLabel: {
                        color: 'auto',
                        distance: 40,
                        fontSize: 10
                    },
                    detail: {
                        valueAnimation: true,
                        fontSize: 15,
                        formatter: 'Brake {value} %',
                        color: 'auto'
                    },
                    data: [{
                        value: 0
                    }]
                }]
            }

            setInterval(function () {


                if (zerotime < number) {
                    speedoption.series[0].data[0].value = speed[zerotime].toFixed(0);
                    brakeoption.series[0].data[0].value = brake[zerotime].toFixed(0);
                    //trackMap.push(new BMap.Point(Lon[zerotime], Lat[zerotime]));
                    zerotime++;
                    myChart.setOption(speedoption, true);
                    BrakeChart.setOption(brakeoption, true);


                    //if (trackMap.length >1) {
                    //    map.addOverlay(new BMap.Polyline([trackMap[trackMap.length - 2], trackMap[trackMap.length - 1]], { strokeColor: "blue", strokeWeight: 3, strokeOpacity: 1, strokeStyle: "dashed" }));
                    //}

                    ////carMarker.setPosition(trackMap[trackMap.length - 1]);//setPosition:设置标注的地理坐标
                    //marker = new BMap.Marker(trackMap[trackMap.length - 1], { icon: carIcon });
                    //map.addOverlay(marker);

                    //map.panTo(trackMap[trackMap.length - 1]);//将地图的中心点更改为给定的点。

                }
                else {
                    speedoption.series[0].data[0].value = speed[number - 1].toFixed(0);
                    brakeoption.series[0].data[0].value = brake[number - 1].toFixed(0);
                    myChart.setOption(speedoption, true);
                    BrakeChart.setOption(brakeoption, true);

                }
            }, 1000);
            if (navigator.onLine) {

                //map = new BMap.Map("allmap");

                //var startpoint = new BMap.Point(121.472644, 31.231706);
                //map.centerAndZoom(startpoint, 17);
                map.enableScrollWheelZoom();
                var myIcon = new BMap.Icon('/Pictures/car.png',
                    new BMap.Size(52, 26), {
                    anchor: new BMap.Size(27, 13)
                });
                map.clearOverlays(polyline);
                var allPoint = [];
                var testPoint = [];
                for (var i = 0; i < Lat.length; i++) {
                    allPoint.push(new BMap.Point(Lon[i], Lat[i]));

                }
                //console.log(allPoint);
                callback = function (xyResult) {
                    //console.log(xyResult);
                    //if (xyResult.error != 0) { return; }//出错就直接返回;
                    for (var i = 0; i < xyResult.length; i++) {
                        testPoint.push(new BMap.Point(xyResult[i]["x"], xyResult[i]["y"]));

                    }

                    //console.log(testPoint);
                    polyline = new BMap.Polyline(testPoint, {
                        strokeColor: "blue",
                        strokeWeight: 3,
                        strokeOpacity: 0.7
                    });
                    map.addOverlay(polyline);
                    //console.log(lushu);
                    if (lushu) {
                        lushu.stop();
                    }
                    lushu = new BMapLib.LuShu(map, testPoint, {
                        defaultContent: "ADF0979",
                        autoView: true, //是否开启自动视野调整，如果开启那么路书在运动过程中会根据视野自动调整
                        icon: myIcon,
                        enableRotation: true, //是否设置marker随着道路的走向进行旋转
                        speed: 20,
                        landmarkPois: []
                    });


                    lushu.start();


                    //var marker = new BMap.Marker(point);
                    //map.addOverlay(marker);

                }

                BMap.Convertor.transMore(allPoint, 0, callback);


            }
        }
       
    });

   

    connection.on("ReloadDataWFT", function (_vehicleID, name, timedomainresult, statisticresult) {
        //console.log(timedomainresult);
        if (_vehicleID == "ADF0979") {

            if (timedomainresult.length > 0) {
                id = name;               //var array = result["data"];
                var time = [];
                var Dis_Dmp_LF = [];
                var Dis_Dmp_RF = [];
                var Dis_Dmp_LR = [];
                var Dis_Dmp_RR = [];
                var WFT_Fx_LF = [];
                var WFT_Fx_RF = [];
                var WFT_Fx_LR = [];
                var WFT_Fx_RR = [];
                var WFT_Fy_LF = [];
                var WFT_Fy_RF = [];
                var WFT_Fy_LR = [];
                var WFT_Fy_RR = [];
                var WFT_Fz_LF = [];
                var WFT_Fz_RF = [];
                var WFT_Fz_LR = [];
                var WFT_Fz_RR = [];
                var WFT_Mx_LF = [];
                var WFT_Mx_RF = [];
                var WFT_Mx_LR = [];
                var WFT_Mx_RR = [];
                var WFT_My_LF = [];
                var WFT_My_RF = [];
                var WFT_My_LR = [];
                var WFT_My_RR = [];
                var WFT_Mz_LF = [];
                var WFT_Mz_RF = [];
                var WFT_Mz_LR = [];
                var WFT_Mz_RR = [];
                for (var i in timedomainresult) {
                    if (timedomainresult[i]["disDmpLF"] > 100 || timedomainresult[i]["disDmpRF"] > 100 || timedomainresult[i]["disDmpLR"] > 100 || timedomainresult[i]["disDmpRR"] > 100) {
                        if (n < 10000) {
                            n++;
                        }

                    }
                    time.push(timedomainresult[i]["time"]);
                    Dis_Dmp_LF.push([timedomainresult[i]["time"], timedomainresult[i]["disDmpLF"]]);
                    Dis_Dmp_RF.push([timedomainresult[i]["time"], timedomainresult[i]["disDmpRF"]]);
                    Dis_Dmp_LR.push([timedomainresult[i]["time"], timedomainresult[i]["disDmpLR"]]);
                    Dis_Dmp_RR.push([timedomainresult[i]["time"], timedomainresult[i]["disDmpRR"]]);
                    WFT_Fx_LF.push([timedomainresult[i]["time"], timedomainresult[i]["wftFxLF"]]);
                    WFT_Fx_RF.push([timedomainresult[i]["time"], timedomainresult[i]["wftFxRF"]]);
                    WFT_Fx_LR.push([timedomainresult[i]["time"], timedomainresult[i]["wftFxLR"]]);
                    WFT_Fx_RR.push([timedomainresult[i]["time"], timedomainresult[i]["wftFxRR"]]);
                    WFT_Fy_LF.push([timedomainresult[i]["time"], timedomainresult[i]["wftFyLF"]]);
                    WFT_Fy_RF.push([timedomainresult[i]["time"], timedomainresult[i]["wftFyRF"]]);
                    WFT_Fy_LR.push([timedomainresult[i]["time"], timedomainresult[i]["wftFyLR"]]);
                    WFT_Fy_RR.push([timedomainresult[i]["time"], timedomainresult[i]["wftFyRR"]]);
                    WFT_Fz_LF.push([timedomainresult[i]["time"], timedomainresult[i]["wftFzLF"]]);
                    WFT_Fz_RF.push([timedomainresult[i]["time"], timedomainresult[i]["wftFzRF"]]);
                    WFT_Fz_LR.push([timedomainresult[i]["time"], timedomainresult[i]["wftFzLR"]]);
                    WFT_Fz_RR.push([timedomainresult[i]["time"], timedomainresult[i]["wftFzRR"]]);
                    WFT_Mx_LF.push([timedomainresult[i]["time"], timedomainresult[i]["wftMxLF"]]);
                    WFT_Mx_RF.push([timedomainresult[i]["time"], timedomainresult[i]["wftMxRF"]]);
                    WFT_Mx_LR.push([timedomainresult[i]["time"], timedomainresult[i]["wftMxLR"]]);
                    WFT_Mx_RR.push([timedomainresult[i]["time"], timedomainresult[i]["wftMxRR"]]);
                    WFT_My_LF.push([timedomainresult[i]["time"], timedomainresult[i]["wftMyLF"]]);
                    WFT_My_RF.push([timedomainresult[i]["time"], timedomainresult[i]["wftMyRF"]]);
                    WFT_My_LR.push([timedomainresult[i]["time"], timedomainresult[i]["wftMyLR"]]);
                    WFT_My_RR.push([timedomainresult[i]["time"], timedomainresult[i]["wftMyRR"]]);
                    WFT_Mz_LF.push([timedomainresult[i]["time"], timedomainresult[i]["wftMzLF"]]);
                    WFT_Mz_RF.push([timedomainresult[i]["time"], timedomainresult[i]["wftMzRF"]]);
                    WFT_Mz_LR.push([timedomainresult[i]["time"], timedomainresult[i]["wftMzLR"]]);
                    WFT_Mz_RR.push([timedomainresult[i]["time"], timedomainresult[i]["wftMzRR"]]);
                }
                if (n > 0) {
                    $("#warninginfo").text(n);
                    fileinfo.push(id);
                }
                Highcharts.chart('WFTFX', {
                    chart: {
                        zoomType: 'x'
                    },
                    exporting: { enabled: false },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    boost: {
                        useGPUTranslations: true
                    },
                    title: {
                        text: "轮心力X"
                    },
                    subtitle: {
                        text: id
                    },
                    tooltip: {
                        valueDecimals: 2
                    },
                    yAxis: {
                        title: {
                            text: 'N'
                        }
                    },
                    series: [{

                        name: 'LF',
                        data: WFT_Fx_LF

                    },
                    {
                        name: 'RF',
                        data: WFT_Fx_RF

                    },
                    {
                        name: 'LR',
                        data: WFT_Fx_LR

                    },
                    {
                        name: 'RR',
                        data: WFT_Fx_RR

                    }


                    ]




                });
                Highcharts.chart('WFTFY', {
                    chart: {
                        zoomType: 'x'
                    },
                    exporting: { enabled: false },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    boost: {
                        useGPUTranslations: true
                    },
                    title: {
                        text: "轮心力Y"
                    },
                    subtitle: {
                        text: id
                    },
                    tooltip: {
                        valueDecimals: 2
                    },
                    yAxis: {
                        title: {
                            text: 'N'
                        }
                    },
                    series: [{

                        name: 'LF',
                        data: WFT_Fy_LF

                    },
                    {
                        name: 'RF',
                        data: WFT_Fy_RF

                    },
                    {
                        name: 'LR',
                        data: WFT_Fy_LR

                    },
                    {
                        name: 'RR',
                        data: WFT_Fy_RR
                    }

                    ]

                });
                Highcharts.chart('WFTFZ', {
                    chart: {
                        zoomType: 'x'
                    },
                    exporting: { enabled: false },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    boost: {
                        useGPUTranslations: true
                    },
                    title: {
                        text: "轮心力Z"
                    },
                    subtitle: {
                        text: id
                    },
                    tooltip: {
                        valueDecimals: 2
                    },
                    yAxis: {
                        title: {
                            text: 'N'
                        }
                    },
                    series: [{

                        name: 'LF',
                        data: WFT_Fz_LF

                    },
                    {
                        name: 'RF',
                        data: WFT_Fz_RF

                    },
                    {
                        name: 'LR',
                        data: WFT_Fz_LR

                    },
                    {
                        name: 'RR',
                        data: WFT_Fz_RR
                    }

                    ]

                });
                Highcharts.chart('DisDMP', {
                    chart: {
                        zoomType: 'x'
                    },
                    exporting: { enabled: false },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    boost: {
                        useGPUTranslations: true
                    },
                    title: {
                        text: "减震器位移"
                    },
                    subtitle: {
                        text: id
                    },
                    tooltip: {
                        valueDecimals: 2
                    },
                    yAxis: {
                        title: {
                            text: 'mm'
                        }
                    },
                    series: [{

                        name: 'LF',
                        data: Dis_Dmp_LF

                    },
                    {
                        name: 'RF',
                        data: Dis_Dmp_RF

                    },
                    {
                        name: 'LR',
                        data: Dis_Dmp_LR

                    },
                    {
                        name: 'RR',
                        data: Dis_Dmp_RR
                    }

                    ]

                });

            }
            if (statisticresult.length > 0) {
                var filename = name;
                var WFTLFmax = [];
                var WFTRFmax = [];
                var WFTLRmax = [];
                var WFTRRmax = [];
                var WFTLFmin = [];
                var WFTRFmin = [];
                var WFTLRmin = [];
                var WFTRRmin = [];
                var DMPLFmin = [];
                var DMPRFmin = [];
                var DMPLRmin = [];
                var DMPRRmin = [];
                var DMPLFmax = [];
                var DMPRFmax = [];
                var DMPLRmax = [];
                var DMPRRmax = [];

                for (var i in statisticresult) {

                    if (statisticresult[i]["chantitle"] == "WFT_Fx_LF" || statisticresult[i]['chantitle'] == "WFT_Fy_LF" || statisticresult[i]['chantitle'] == "WFT_Fz_LF") {
                        WFTLFmax.push(statisticresult[i]["max"]);
                        WFTLFmin.push(statisticresult[i]["min"]);

                    }
                    if (statisticresult[i]["chantitle"] == "WFT_Fx_RF" || statisticresult[i]['chantitle'] == "WFT_Fy_RF" || statisticresult[i]['chantitle'] == "WFT_Fz_RF") {
                        WFTRFmax.push(statisticresult[i]["max"]);
                        WFTRFmin.push(statisticresult[i]["min"]);

                    }
                    if (statisticresult[i]["chantitle"] == "WFT_Fx_LR" || statisticresult[i]['chantitle'] == "WFT_Fy_LR" || statisticresult[i]['chantitle'] == "WFT_Fz_LR") {
                        WFTLRmax.push(statisticresult[i]["max"]);
                        WFTLRmin.push(statisticresult[i]["min"]);
                    }
                    if (statisticresult[i]["chantitle"] == "WFT_Fx_RR" || statisticresult[i]['chantitle'] == "WFT_Fy_RR" || statisticresult[i]['chantitle'] == "WFT_Fz_RR") {
                        WFTRRmax.push(statisticresult[i]["max"]);
                        WFTRRmin.push(statisticresult[i]["min"]);

                    }

                    if (statisticresult[i]["chantitle"] == "Dis_Dmp_LF") {
                        DMPLFmax.push(statisticresult[i]["max"]);
                        DMPLFmin.push(statisticresult[i]["min"])
                    }
                    if (statisticresult[i]["chantitle"] == "Dis_Dmp_RF") {
                        DMPRFmax.push(statisticresult[i]["max"]);
                        DMPRFmin.push(statisticresult[i]["min"])
                    }
                    if (statisticresult[i]["chantitle"] == "Dis_Dmp_LR") {
                        DMPLRmax.push(statisticresult[i]["max"]);
                        DMPLRmin.push(statisticresult[i]["min"])
                    }
                    if (statisticresult[i]["chantitle"] == "Dis_Dmp_RR") {
                        DMPRRmax.push(statisticresult[i]["max"]);
                        DMPRRmin.push(statisticresult[i]["min"])
                    }

                }
                //speed = Math.round(Speed[0]);



                Highcharts.chart('WFTLFRFMAX', {
                    chart: {

                        type: 'column',
                        zoomType: 'x'
                    },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    title: {
                        text: '最大值',

                    },
                    subtitle: {
                        text: filename,

                    },

                    xAxis: {
                        categories: [
                            'X', 'Y', 'Z'
                        ],

                    },
                    yAxis: {

                        title: {
                            text: 'N'
                        }
                    },
                    tooltip: {
                        shared: true,
                        pointFormat: '<span style="color:{series.color}">{series.name}: <b>{point.y:,.1f}</b><br/>'
                    },
                    plotOptions: {
                        column: {
                            borderWidth: 0
                        }
                    },
                    series: [{
                        name: 'LF',
                        data: eval(WFTLFmax)
                    }, {
                        name: 'RF',
                        data: eval(WFTRFmax)

                    }]


                });

                Highcharts.chart('WFTLRRRMAX', {
                    chart: {

                        type: 'column',
                        zoomType: 'x'
                    },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    title: {
                        text: '最大值',

                    },
                    subtitle: {
                        text: filename,

                    },

                    xAxis: {
                        categories: [
                            'X', 'Y', 'Z'
                        ],

                    },
                    yAxis: {

                        title: {
                            text: 'N'
                        }
                    },
                    tooltip: {
                        shared: true,
                        pointFormat: '<span style="color:{series.color}">{series.name}: <b>{point.y:,.1f}</b><br/>'
                    },
                    plotOptions: {
                        column: {
                            borderWidth: 0
                        }
                    },
                    series: [{
                        name: 'LR',
                        data: eval(WFTLRmax)
                    }, {
                        name: 'RR',
                        data: eval(WFTRRmax)

                    }]
                });

                Highcharts.chart('WFTLFRFMIN', {
                    chart: {

                        type: 'column',
                        zoomType: 'x'
                    },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    title: {
                        text: '最小值',

                    },
                    subtitle: {
                        text: filename,

                    },

                    xAxis: {
                        categories: [
                            'X', 'Y', 'Z'
                        ],

                    },
                    yAxis: {

                        title: {
                            text: 'N'
                        }
                    },
                    tooltip: {
                        shared: true,
                        pointFormat: '<span style="color:{series.color}">{series.name}: <b>{point.y:,.1f}</b><br/>'
                    },
                    plotOptions: {
                        column: {
                            borderWidth: 0
                        }
                    },
                    series: [{
                        name: 'LF',
                        data: eval(WFTLFmin)
                    }, {
                        name: 'RF',
                        data: eval(WFTRFmin)

                    }]


                });

                Highcharts.chart('WFTLRRRMIN', {
                    chart: {

                        type: 'column',
                        zoomType: 'x'
                    },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    title: {
                        text: '最小值',

                    },
                    subtitle: {
                        text: filename,

                    },

                    xAxis: {
                        categories: [
                            'X', 'Y', 'Z'
                        ],

                    },
                    yAxis: {

                        title: {
                            text: 'N'
                        }
                    },
                    tooltip: {
                        shared: true,
                        pointFormat: '<span style="color:{series.color}">{series.name}: <b>{point.y:,.1f}</b><br/>'
                    },
                    plotOptions: {
                        column: {
                            borderWidth: 0
                        }
                    },
                    series: [{
                        name: 'LR',
                        data: eval(WFTLRmin)
                    }, {
                        name: 'RR',
                        data: eval(WFTRRmin)

                    }]
                });

                Highcharts.chart('DMPMAX', {
                    chart: {

                        type: 'column',
                        zoomType: 'x'
                    },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    title: {
                        text: '减震器位移最大值',

                    },
                    subtitle: {
                        text: filename,

                    },

                    xAxis: {
                        categories: [
                            '位移'
                        ],

                    },
                    yAxis: {

                        title: {
                            text: 'mm'
                        }
                    },
                    tooltip: {
                        shared: true,
                        pointFormat: '<span style="color:{series.color}">{series.name}: <b>{point.y:,.1f}</b><br/>'
                    },
                    plotOptions: {
                        column: {
                            borderWidth: 0
                        }
                    },
                    series: [{
                        name: 'LF',
                        data: eval(DMPLFmax)
                    }, {
                        name: 'RF',
                        data: eval(DMPRFmax)

                    }
                        , {
                        name: 'LR',
                        data: eval(DMPLRmax)

                    }
                        , {
                        name: 'RR',
                        data: eval(DMPRRmax)

                    }

                    ]
                });
                Highcharts.chart('DMPMIN', {
                    chart: {

                        type: 'column',
                        zoomType: 'x'
                    },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    title: {
                        text: '减震器位移最小值',

                    },
                    subtitle: {
                        text: filename,

                    },

                    xAxis: {
                        categories: [
                            '位移'
                        ],

                    },
                    yAxis: {

                        title: {
                            text: 'mm'
                        }
                    },
                    tooltip: {
                        shared: true,
                        pointFormat: '<span style="color:{series.color}">{series.name}: <b>{point.y:,.1f}</b><br/>'
                    },
                    plotOptions: {
                        column: {
                            borderWidth: 0
                        }
                    },
                    series: [{
                        name: 'LF',
                        data: eval(DMPLFmin)
                    }, {
                        name: 'RF',
                        data: eval(DMPRFmin)

                    }
                        , {
                        name: 'LR',
                        data: eval(DMPLRmin)

                    }
                        , {
                        name: 'RR',
                        data: eval(DMPRRmin)

                    }

                    ]
                });


            }

        }
        
    });

    connection.on("ReloadDataACC", function (_vehicleID,name,acctimedomainresult, accstatisticresult) {
        if (_vehicleID == "ADF0979") {
            console.log(acctimedomainresult);
            if (acctimedomainresult.length > 0) {
                id = name;               //var array = acctimedomainresult["data"];
                var time = [];
                var speed = [];
                var speedsingle = [];

                var dataAcc_X_Whl_LF = [];
                var dataAcc_X_Whl_RF = [];
                var dataAcc_X_Whl_LR = [];
                var dataAcc_X_Whl_RR = [];

                var dataAcc_Y_Whl_LF = [];
                var dataAcc_Y_Whl_RF = [];
                var dataAcc_Y_Whl_LR = [];
                var dataAcc_Y_Whl_RR = [];

                var dataAcc_Z_Whl_LF = [];
                var dataAcc_Z_Whl_RF = [];
                var dataAcc_Z_Whl_LR = [];
                var dataAcc_Z_Whl_RR = [];

                var dataAcc_X_FM = [];
                var dataAcc_X_RM = [];
                var dataAcc_Y_FM = [];
                var dataAcc_Y_RM = [];
                var dataAcc_Z_FM = [];
                var dataAcc_Z_RM = [];



                for (var i in acctimedomainresult) {
                    if (acctimedomainresult[i]["accXWhlLF"] > 100 || acctimedomainresult[i]["accXWhlRF"] > 100 || acctimedomainresult[i]["accXWhlLR"] > 100 || acctimedomainresult[i]["accXWhlRR"] > 100) {
                        if (n < 10000) {
                            n++;
                        }

                    }
                    time.push(acctimedomainresult[i]["time"]);
                    speedsingle.push(acctimedomainresult[i]["speed"]);

                    speed.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["speed"]]);


                    dataAcc_X_Whl_LF.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accXWhlLF"]]);
                    dataAcc_X_Whl_RF.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accXWhlRF"]]);
                    dataAcc_X_Whl_LR.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accXWhlLR"]]);
                    dataAcc_X_Whl_RR.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accXWhlRR"]]);
                    dataAcc_Y_Whl_LF.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accYWhlLF"]]);
                    dataAcc_Y_Whl_RF.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accYWhlRF"]]);
                    dataAcc_Y_Whl_LR.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accYWhlLR"]]);
                    dataAcc_Y_Whl_RR.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accYWhlRR"]]);
                    dataAcc_Z_Whl_LF.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accZWhlLF"]]);
                    dataAcc_Z_Whl_RF.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accZWhlRF"]]);
                    dataAcc_Z_Whl_LR.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accZWhlLR"]]);
                    dataAcc_Z_Whl_RR.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accZWhlRR"]]);

                    dataAcc_X_FM.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accXFM"]]);
                    dataAcc_X_RM.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accXRM"]]);

                    dataAcc_Y_FM.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accYFM"]]);
                    dataAcc_Y_RM.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accYRM"]]);

                    dataAcc_Z_FM.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accZFM"]]);
                    dataAcc_Z_RM.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accZRM"]]);
                }
                if (n > 0) {
                    $("#warninginfo").text(n);
                    fileinfo.push(id);
                }
                speedout = speedsingle;
                firstspeed = speedsingle[0];

                Highcharts.chart('WheelAccXcontainer', {
                    chart: {
                        zoomType: 'x'
                    },
                    exporting: { enabled: false },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    boost: {
                        useGPUTranslations: true
                    },
                    title: {
                        text: "轮心加速度X"
                    },
                    subtitle: {
                        text: id
                    },
                    tooltip: {
                        valueDecimals: 2
                    },
                    yAxis: {
                        title: {
                            text: 'g'
                        }
                    },
                    series: [{

                        name: 'LF',
                        data: dataAcc_X_Whl_LF

                    },
                    {
                        name: 'RF',
                        data: dataAcc_X_Whl_RF

                    },
                    {
                        name: 'LR',
                        data: dataAcc_X_Whl_LR

                    },
                    {
                        name: 'RR',
                        data: dataAcc_X_Whl_RR

                    }


                    ]




                });

                Highcharts.chart('WheelAccYcontainer', {
                    chart: {
                        zoomType: 'x'
                    },
                    exporting: { enabled: false },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    boost: {
                        useGPUTranslations: true
                    },
                    title: {
                        text: "轮心加速度Y"
                    },
                    subtitle: {
                        text: id
                    },
                    tooltip: {
                        valueDecimals: 2
                    },
                    yAxis: {
                        title: {
                            text: 'g'
                        }
                    },
                    series: [
                        {
                            name: 'LF',
                            data: dataAcc_Y_Whl_LF

                        },
                        {
                            name: 'RF',
                            data: dataAcc_Y_Whl_RF

                        },
                        {
                            name: 'LR',
                            data: dataAcc_Y_Whl_LR

                        },
                        {
                            name: 'RR',
                            data: dataAcc_Y_Whl_RR

                        }]



                });



                Highcharts.chart('WheelAccZcontainer', {
                    chart: {
                        zoomType: 'x'
                    },
                    exporting: { enabled: false },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    boost: {
                        useGPUTranslations: true
                    },
                    title: {
                        text: "轮心加速度Z"
                    },
                    subtitle: {
                        text: id
                    },
                    tooltip: {
                        valueDecimals: 2
                    },
                    yAxis: {
                        title: {
                            text: 'g'
                        }
                    },
                    series: [
                        {
                            name: 'LF',
                            data: dataAcc_Z_Whl_LF

                        },
                        {
                            name: 'RF',
                            data: dataAcc_Z_Whl_RF

                        },
                        {
                            name: 'LR',
                            data: dataAcc_Z_Whl_LR

                        },
                        {
                            name: 'RR',
                            data: dataAcc_Z_Whl_RR

                        }]



                });

                Highcharts.chart('WheelRMFMcontainer', {
                    chart: {
                        zoomType: 'x'
                    },
                    exporting: { enabled: false },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    boost: {
                        useGPUTranslations: true
                    },
                    title: {
                        text: "车身加速度XYZ"
                    },
                    subtitle: {
                        text: id
                    },
                    tooltip: {
                        valueDecimals: 2
                    },
                    yAxis: {
                        title: {
                            text: 'g'
                        }
                    },
                    series: [{

                        name: 'X_FM',
                        data: dataAcc_X_FM

                    },
                    {
                        name: 'X_RM',
                        data: dataAcc_X_RM

                    },
                    {
                        name: 'Y_FM',
                        data: dataAcc_Y_FM

                    },
                    {
                        name: 'Y_RM',
                        data: dataAcc_Y_RM

                    },
                    {
                        name: 'Z_FM',
                        data: dataAcc_Z_FM

                    },
                    {
                        name: 'Z_RM',
                        data: dataAcc_Z_RM

                    }
                    ]




                });


            };

            if (accstatisticresult.length > 0) {
                var filename = name;
                var ACCFMmax = [];
                var ACCRMmax = [];
                var ACCFMmin = [];
                var ACCRMmin = [];
                var ACCFMrms = [];
                var ACCRMrms = [];
                var ACCWHLLFmax = [];
                var ACCWHLRFmax = [];
                var ACCWHLLRmax = [];
                var ACCWHLRRmax = [];
                var ACCWHLLFmin = [];
                var ACCWHLRFmin = [];
                var ACCWHLLRmin = [];
                var ACCWHLRRmin = [];
                var ACCWHLLFrms = [];
                var ACCWHLRFrms = [];
                var ACCWHLLRrms = [];
                var ACCWHLRRrms = [];
                var Speed = [];


                for (var i in accstatisticresult) {

                    if (accstatisticresult[i]['chantitle'] == "Acc_X_FM") {
                        ACCFMmax.push(accstatisticresult[i]["max"]);
                        ACCFMmin.push(accstatisticresult[i]["min"]);
                        ACCFMrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "Acc_X_RM") {
                        ACCRMmax.push(accstatisticresult[i]["max"]);
                        ACCRMmin.push(accstatisticresult[i]["min"]);
                        ACCRMrms.push(accstatisticresult[i]["rms"])
                    }

                    if (accstatisticresult[i]['chantitle'] == "Acc_X_Whl_LF") {
                        ACCWHLLFmax.push(accstatisticresult[i]["max"]);
                        ACCWHLLFmin.push(accstatisticresult[i]["min"]);
                        ACCWHLLFrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "Acc_X_Whl_RF") {
                        ACCWHLRFmax.push(accstatisticresult[i]["max"]);
                        ACCWHLRFmin.push(accstatisticresult[i]["min"]);
                        ACCWHLRFrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "Acc_X_Whl_LR") {
                        ACCWHLLRmax.push(accstatisticresult[i]["max"]);
                        ACCWHLLRmin.push(accstatisticresult[i]["min"]);
                        ACCWHLLRrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "Acc_X_Whl_RR") {
                        ACCWHLRRmax.push(accstatisticresult[i]["max"]);
                        ACCWHLRRmin.push(accstatisticresult[i]["min"]);
                        ACCWHLRRrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]["chantitle"] == "Speed") {
                        Speed.push(accstatisticresult[i]["rms"]);
                    }
                }

                for (var i in accstatisticresult) {

                    if (accstatisticresult[i]['chantitle'] == "Acc_Y_FM") {
                        ACCFMmax.push(accstatisticresult[i]["max"]);
                        ACCFMmin.push(accstatisticresult[i]["min"]);
                        ACCFMrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "Acc_Y_RM") {
                        ACCRMmax.push(accstatisticresult[i]["max"]);
                        ACCRMmin.push(accstatisticresult[i]["min"]);
                        ACCRMrms.push(accstatisticresult[i]["rms"])
                    }

                    if (accstatisticresult[i]['chantitle'] == "Acc_Y_Whl_LF") {
                        ACCWHLLFmax.push(accstatisticresult[i]["max"]);
                        ACCWHLLFmin.push(accstatisticresult[i]["min"]);
                        ACCWHLLFrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "Acc_Y_Whl_RF") {
                        ACCWHLRFmax.push(accstatisticresult[i]["max"]);
                        ACCWHLRFmin.push(accstatisticresult[i]["min"]);
                        ACCWHLRFrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "Acc_Y_Whl_LR") {
                        ACCWHLLRmax.push(accstatisticresult[i]["max"]);
                        ACCWHLLRmin.push(accstatisticresult[i]["min"]);
                        ACCWHLLRrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "Acc_Y_Whl_RR") {
                        ACCWHLRRmax.push(accstatisticresult[i]["max"]);
                        ACCWHLRRmin.push(accstatisticresult[i]["min"]);
                        ACCWHLRRrms.push(accstatisticresult[i]["rms"])
                    }

                }
                for (var i in accstatisticresult) {

                    if (accstatisticresult[i]['chantitle'] == "Acc_Z_FM") {
                        ACCFMmax.push(accstatisticresult[i]["max"]);
                        ACCFMmin.push(accstatisticresult[i]["min"]);
                        ACCFMrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "Acc_Z_RM") {
                        ACCRMmax.push(accstatisticresult[i]["max"]);
                        ACCRMmin.push(accstatisticresult[i]["min"]);
                        ACCRMrms.push(accstatisticresult[i]["rms"])
                    }

                    if (accstatisticresult[i]['chantitle'] == "Acc_Z_Whl_LF") {
                        ACCWHLLFmax.push(accstatisticresult[i]["max"]);
                        ACCWHLLFmin.push(accstatisticresult[i]["min"]);
                        ACCWHLLFrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "Acc_Z_Whl_RF") {
                        ACCWHLRFmax.push(accstatisticresult[i]["max"]);
                        ACCWHLRFmin.push(accstatisticresult[i]["min"]);
                        ACCWHLRFrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "Acc_Z_Whl_LR") {
                        ACCWHLLRmax.push(accstatisticresult[i]["max"]);
                        ACCWHLLRmin.push(accstatisticresult[i]["min"]);
                        ACCWHLLRrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "Acc_Z_Whl_RR") {
                        ACCWHLRRmax.push(accstatisticresult[i]["max"]);
                        ACCWHLRRmin.push(accstatisticresult[i]["min"]);
                        ACCWHLRRrms.push(accstatisticresult[i]["rms"])
                    }

                }
                Highcharts.chart('ACCFMRMMAX', {
                    chart: {

                        type: 'column',
                        zoomType: 'x'
                    },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    title: {
                        text: '最大值',

                    },
                    subtitle: {
                        text: filename,

                    },

                    xAxis: {
                        categories: [
                            'X', 'Y', 'Z'
                        ],

                    },
                    yAxis: {

                        title: {
                            text: '加速度(g)'
                        }
                    },
                    tooltip: {
                        shared: true,
                        pointFormat: '<span style="color:{series.color}">{series.name}: <b>{point.y:,.1f}</b><br/>'
                    },
                    plotOptions: {
                        column: {
                            borderWidth: 0
                        }
                    },
                    series: [{
                        name: 'FM',
                        data: eval(ACCFMmax)
                    }, {
                        name: 'RM',
                        data: eval(ACCRMmax)

                    }]


                });

                Highcharts.chart('ACCFMRMMIN', {
                    chart: {

                        type: 'column',
                        zoomType: 'x'
                    },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    title: {
                        text: '最小值',

                    },
                    subtitle: {
                        text: filename,

                    },

                    xAxis: {
                        categories: [
                            'X', 'Y', 'Z'
                        ],

                    },
                    yAxis: {

                        title: {
                            text: '加速度(g)'
                        }
                    },
                    tooltip: {
                        shared: true,
                        pointFormat: '<span style="color:{series.color}">{series.name}: <b>{point.y:,.1f}</b><br/>'
                    },
                    plotOptions: {
                        column: {
                            borderWidth: 0
                        }
                    },
                    series: [{
                        name: 'FM',
                        data: eval(ACCFMmin)
                    }, {
                        name: 'RM',
                        data: eval(ACCRMmin)

                    }]
                });

                Highcharts.chart('ACCFMRMRMS', {
                    chart: {

                        type: 'column',
                        zoomType: 'x'
                    },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    title: {
                        text: 'RMS',

                    },
                    subtitle: {
                        text: filename,

                    },

                    xAxis: {
                        categories: [
                            'X', 'Y', 'Z'
                        ],

                    },
                    yAxis: {

                        title: {
                            text: '加速度(g)'
                        }
                    },
                    tooltip: {
                        shared: true,
                        pointFormat: '<span style="color:{series.color}">{series.name}: <b>{point.y:,.1f}</b><br/>'
                    },
                    plotOptions: {
                        column: {
                            borderWidth: 0
                        }
                    },
                    series: [{
                        name: 'FM',
                        data: eval(ACCFMrms)
                    }, {
                        name: 'RM',
                        data: eval(ACCRMrms)

                    }]
                });

                Highcharts.chart('ACCFWHLMAX', {
                    chart: {

                        type: 'column',
                        zoomType: 'x'
                    },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    title: {
                        text: '最大值',

                    },
                    subtitle: {
                        text: filename,

                    },

                    xAxis: {
                        categories: [
                            'X', 'Y', 'Z'
                        ],

                    },
                    yAxis: {

                        title: {
                            text: '加速度(g)'
                        }
                    },
                    tooltip: {
                        shared: true,
                        pointFormat: '<span style="color:{series.color}">{series.name}: <b>{point.y:,.1f}</b><br/>'
                    },
                    plotOptions: {
                        column: {
                            borderWidth: 0
                        }
                    },
                    series: [{
                        name: 'LF',
                        data: eval(ACCWHLLFmax)
                    }, {
                        name: 'RF',
                        data: eval(ACCWHLRFmax)

                    }, {
                        name: 'LR',
                        data: eval(ACCWHLLRmax)
                    }, {
                        name: 'RR',
                        data: eval(ACCWHLRRmax)
                    }]
                });

                Highcharts.chart('ACCFWHLMIN', {
                    chart: {

                        type: 'column',
                        zoomType: 'x'
                    },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    title: {
                        text: '最小值',

                    },
                    subtitle: {
                        text: filename,

                    },

                    xAxis: {
                        categories: [
                            'X', 'Y', 'Z'
                        ],

                    },
                    yAxis: {

                        title: {
                            text: '加速度(g)'
                        }
                    },
                    tooltip: {
                        shared: true,
                        pointFormat: '<span style="color:{series.color}">{series.name}: <b>{point.y:,.1f}</b><br/>'
                    },
                    plotOptions: {
                        column: {
                            borderWidth: 0
                        }
                    },
                    series: [{
                        name: 'LF',
                        data: eval(ACCWHLLFmin)
                    }, {
                        name: 'RF',
                        data: eval(ACCWHLRFmin)

                    }, {
                        name: 'LR',
                        data: eval(ACCWHLLRmin)
                    }, {
                        name: 'RR',
                        data: eval(ACCWHLRRmin)
                    }]
                });

                Highcharts.chart('ACCFWHLRMS', {
                    chart: {

                        type: 'column',
                        zoomType: 'x'
                    },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    title: {
                        text: 'RMS',

                    },
                    subtitle: {
                        text: filename,

                    },

                    xAxis: {
                        categories: [
                            'X', 'Y', 'Z'
                        ],

                    },
                    yAxis: {

                        title: {
                            text: '加速度(g)'
                        }
                    },
                    tooltip: {
                        shared: true,
                        pointFormat: '<span style="color:{series.color}">{series.name}: <b>{point.y:,.1f}</b><br/>'
                    },
                    plotOptions: {
                        column: {
                            borderWidth: 0
                        }
                    },
                    series: [{
                        name: 'LF',
                        data: eval(ACCWHLLFrms)
                    }, {
                        name: 'RF',
                        data: eval(ACCWHLRFrms)

                    }, {
                        name: 'LR',
                        data: eval(ACCWHLLRrms)
                    }, {
                        name: 'RR',
                        data: eval(ACCWHLRRrms)
                    }]
                });




            }

        }


    });

    connection.start().then(function () {
        layer.msg("已开始监视");
        //document.getElementById("StartUpload").disabled = false;
    }).catch(function (err) {
        return console.error(err.toString());
    });


    //$("#loginfo").click(function () {

    //    layer.open({
    //        type: 2,
    //        title: '数据错误日志信息',
    //        shadeClose: true,
    //        shade: 0.8,
    //        area: ['720px', '80%'],
    //        content: urlloginfo, //iframe的url
    //        btn: ['确认', '取消'],

    //        success: function (layero, index) {

    //            bodylog = layer.getChildFrame('body', index);
    //            for (var i in fileinfo) {
    //                bodylog.append('<p>' + "警告！文件名为" + fileinfo[i] + "的数据有问题，请查看源数据" + '</p>');
    //            }

    //        }
    //    });


    /*});*/
    
   
});