

layui.use(['element', 'layer', 'table', 'form'], function () {
    var $ = layui.jquery;
    layer = layui.layer;
    element = layui.element;
    var a = GetRequest();
    var connection;
    $("title").text(a['id']+"监控页面");
   
    var lushu;
    var routpath = [];
    var polyline;


    function filewatchcontroller() {
        $.ajax({

            type: "POST",
            //请求的媒体类型
            dataType: 'text',//这里改为json就不会传回success需要的数据了
            //请求地址
            url: urlfilewatcher,
            data: {
                _vehicleID: a['id']
            },

        });
    }

    filewatchcontroller();

    connection = new signalR.HubConnectionBuilder().withUrl("/MyHub").build();
    connection.serverTimeoutInMilliseconds = 30000;
    connection.keepAliveIntervalInMilliseconds = 15000;

    connection.on("SpeedtoDistance", function (_vehicleID, distance, speed, Brake, Lat, Lon, StrgWhlAng, zerotime) {
       /* console.time("SpeedtoDistance");*/
        if (_vehicleID == a['id']) {

            var number = speed.length;
            var speed0 = speed[0];
            //判断服务器传过来的是哪辆车就显示哪辆车的信息，因为每辆车的数据源不一样

            $("#distance").text(distance.toFixed(2));

            if (navigator.onLine) {
                

                var allPoint = [];
                var testPoint = [];
                for (var i = 0; i < Lat.length; i++) {
                    allPoint.push(new BMap.Point(Lon[i], Lat[i]));

                }
                //百度坐标转换的回调函数
                callback = function (xyResult) {

                    for (var i = 0; i < xyResult.length; i++) {
                        testPoint.push(new BMap.Point(xyResult[i]["x"], xyResult[i]["y"]));

                    }


                    polyline = new BMap.Polyline(testPoint, {
                        strokeColor: "blue",
                        strokeWeight: 3,
                        strokeOpacity: 0.7
                    });
                    map.addOverlay(polyline);
                    map.setViewport(routpath);
                    routpath = routpath.concat(testPoint);
                    if (lushu == null) {
                        lushu = new BMapLib.LuShu(map, routpath, {
                            defaultContent: _vehicleID,
                            autoView: true, //是否开启自动视野调整，如果开启那么路书在运动过程中会根据视野自动调整
                            icon: myIcon,
                            enableRotation: true, //是否设置marker随着道路的走向进行旋转
                            speed: 200, //速度很有关系，太快和太慢都会导致车标原地不动
                            landmarkPois: []
                        });
                        lushu.start();

                    }

                    else {
                        lushu.goPath(routpath)
                    }
                    

                }

                BMap.Convertor.transMore(allPoint, 0, callback);


            }

            var myChart = echarts.init(document.getElementById('speedchart'));
            var speedoption;
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
                        value: speed0.toFixed(0)
                    }]
                }]

            };

            myChart.setOption(speedoption, true);
         
            //setInterval(function () {


            //    if (zerotime < number) {
            //        speedoption.series[0].data[0].value = speed[zerotime].toFixed(0);

            //        zerotime++;
            //        myChart.setOption(speedoption, true);

            //    }
            //    else {
            //        speedoption.series[0].data[0].value = speed[number - 1].toFixed(0);

            //        myChart.setOption(speedoption, true);

            //    }
            //}, 1000);

        }
        /*console.timeEnd("SpeedtoDistance");*/
    });

    connection.on("ReloadDataWFT", function (_vehicleID, name, timedomainresult, statisticresult) {
        
        if (_vehicleID == a['id']) {

            if (timedomainresult.length > 0) {
                id = name;               
                
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

                    if (statisticresult[i]["chantitle"] == "WFTFxLF" || statisticresult[i]['chantitle'] == "WFTFyLF" || statisticresult[i]['chantitle'] == "WFTFzLF") {
                        WFTLFmax.push(statisticresult[i]["max"]);
                        WFTLFmin.push(statisticresult[i]["min"]);

                    }
                    if (statisticresult[i]["chantitle"] == "WFTFxRF" || statisticresult[i]['chantitle'] == "WFTFyRF" || statisticresult[i]['chantitle'] == "WFTFzRF") {
                        WFTRFmax.push(statisticresult[i]["max"]);
                        WFTRFmin.push(statisticresult[i]["min"]);

                    }
                    if (statisticresult[i]["chantitle"] == "WFTFxLR" || statisticresult[i]['chantitle'] == "WFTFyLR" || statisticresult[i]['chantitle'] == "WFTFzLR") {
                        WFTLRmax.push(statisticresult[i]["max"]);
                        WFTLRmin.push(statisticresult[i]["min"]);
                    }
                    if (statisticresult[i]["chantitle"] == "WFTFxRR" || statisticresult[i]['chantitle'] == "WFTFyRR" || statisticresult[i]['chantitle'] == "WFTFzRR") {
                        WFTRRmax.push(statisticresult[i]["max"]);
                        WFTRRmin.push(statisticresult[i]["min"]);

                    }

                    if (statisticresult[i]["chantitle"] == "DisDmpLF") {
                        DMPLFmax.push(statisticresult[i]["max"]);
                        DMPLFmin.push(statisticresult[i]["min"])
                    }
                    if (statisticresult[i]["chantitle"] == "DisDmpRF") {
                        DMPRFmax.push(statisticresult[i]["max"]);
                        DMPRFmin.push(statisticresult[i]["min"])
                    }
                    if (statisticresult[i]["chantitle"] == "DisDmpLR") {
                        DMPLRmax.push(statisticresult[i]["max"]);
                        DMPLRmin.push(statisticresult[i]["min"])
                    }
                    if (statisticresult[i]["chantitle"] == "DisDmpRR") {
                        DMPRRmax.push(statisticresult[i]["max"]);
                        DMPRRmin.push(statisticresult[i]["min"])
                    }

                }
               
                Highcharts.chart('WFTLFRFMAX', {
                    chart: {

                        type: 'column',
                        zoomType: 'x'
                    },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    title: {
                        text: '前轮力最大值',

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
                        text: '后轮力最大值',

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
                        text: '前轮力最小值',

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
                        text: '后轮力最小值',

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

            }

        }

    });

    connection.on("ReloadDataACC", function (_vehicleID, name, acctimedomainresult, accstatisticresult) {
        /*console.time("ReloadDataACC");*/
        if (_vehicleID == a['id']) {
            
            if (acctimedomainresult.length > 0) {
             
                id = name;               
              
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
                               
                var dataAcc_X_ST_LF = [];
                var dataAcc_X_ST_RF = [];
                var dataAcc_X_ST_LR = [];
                var dataAcc_X_ST_RR = [];

                var dataAcc_Y_ST_LF = [];
                var dataAcc_Y_ST_RF = [];
                var dataAcc_Y_ST_LR = [];
                var dataAcc_Y_ST_RR = [];

                var dataAcc_Z_ST_LF = [];
                var dataAcc_Z_ST_RF = [];
                var dataAcc_Z_ST_LR = [];
                var dataAcc_Z_ST_RR = [];

                var Dis_Dmp_LF = [];
                var Dis_Dmp_RF = [];
                var Dis_Dmp_LR = [];
                var Dis_Dmp_RR = [];

                for (var i in acctimedomainresult) {
                   
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

                    dataAcc_X_ST_LF.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accXSTLF"]]);
                    dataAcc_X_ST_RF.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accXSTRF"]]);
                    dataAcc_X_ST_LR.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accXSTLR"]]);
                    dataAcc_X_ST_RR.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accXSTRR"]]);
                    dataAcc_Y_ST_LF.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accYSTLF"]]);
                    dataAcc_Y_ST_RF.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accYSTRF"]]);
                    dataAcc_Y_ST_LR.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accYSTLR"]]);
                    dataAcc_Y_ST_RR.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accYSTRR"]]);
                    dataAcc_Z_ST_LF.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accZSTLF"]]);
                    dataAcc_Z_ST_RF.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accZSTRF"]]);
                    dataAcc_Z_ST_LR.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accZSTLR"]]);
                    dataAcc_Z_ST_RR.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["accZSTRR"]]);

                    Dis_Dmp_LF.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["disDmpLF"]]);
                    Dis_Dmp_RF.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["disDmpRF"]]);
                    Dis_Dmp_LR.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["disDmpLR"]]);
                    Dis_Dmp_RR.push([acctimedomainresult[i]["time"], acctimedomainresult[i]["disDmpRR"]]);
                }

               
                //4个轮心X
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
                //4个轮心Y
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
                //4个轮心Z
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
                //4个减震器位移
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

                //4个塔柱X
                Highcharts.chart('STAccXcontainer', {
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
                        text: "塔柱加速度X"
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
                        data: dataAcc_X_ST_LF

                    },
                    {
                        name: 'RF',
                        data: dataAcc_X_ST_RF

                    },
                    {
                        name: 'LR',
                        data: dataAcc_X_ST_LR

                    },
                    {
                        name: 'RR',
                        data: dataAcc_X_ST_RR

                    }
                    ]

                });
                //4个塔柱Y
                Highcharts.chart('STAccYcontainer', {
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
                        text: "塔柱加速度Y"
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
                        data: dataAcc_Y_ST_LF

                    },
                    {
                        name: 'RF',
                        data: dataAcc_Y_ST_RF

                    },
                    {
                        name: 'LR',
                        data: dataAcc_Y_ST_LR

                    },
                    {
                        name: 'RR',
                        data: dataAcc_Y_ST_RR

                    }
                    ]

                });
                //4个塔柱Z
                Highcharts.chart('STAccZcontainer', {
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
                        text: "塔柱加速度Z"
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
                        data: dataAcc_Z_ST_LF

                    },
                    {
                        name: 'RF',
                        data: dataAcc_Z_ST_RF

                    },
                    {
                        name: 'LR',
                        data: dataAcc_Z_ST_LR

                    },
                    {
                        name: 'RR',
                        data: dataAcc_Z_ST_RR

                    }
                    ]

                });
                
               
            };

            if (accstatisticresult.length > 0) {
                var filename = name;
               
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

                var ACCSTLFmax = [];
                var ACCSTRFmax = [];
                var ACCSTLRmax = [];
                var ACCSTRRmax = [];
                var ACCSTLFmin = [];
                var ACCSTRFmin = [];
                var ACCSTLRmin = [];
                var ACCSTRRmin = [];
                var ACCSTLFrms = [];
                var ACCSTRFrms = [];
                var ACCSTLRrms = [];
                var ACCSTRRrms = [];


                for (var i in accstatisticresult) {
                                       
                    //轮心
                    if (accstatisticresult[i]['chantitle'] == "AccXWhlLF") {
                        ACCWHLLFmax.push(accstatisticresult[i]["max"]);
                        ACCWHLLFmin.push(accstatisticresult[i]["min"]);
                        ACCWHLLFrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "AccXWhlRF") {
                        ACCWHLRFmax.push(accstatisticresult[i]["max"]);
                        ACCWHLRFmin.push(accstatisticresult[i]["min"]);
                        ACCWHLRFrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "AccXWhlLR") {
                        ACCWHLLRmax.push(accstatisticresult[i]["max"]);
                        ACCWHLLRmin.push(accstatisticresult[i]["min"]);
                        ACCWHLLRrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "AccXWhlRR") {
                        ACCWHLRRmax.push(accstatisticresult[i]["max"]);
                        ACCWHLRRmin.push(accstatisticresult[i]["min"]);
                        ACCWHLRRrms.push(accstatisticresult[i]["rms"])
                    }
                    //塔柱
                    if (accstatisticresult[i]['chantitle'] == "AccXSTLF") {
                        ACCSTLFmax.push(accstatisticresult[i]["max"]);
                        ACCSTLFmin.push(accstatisticresult[i]["min"]);
                        ACCSTLFrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "AccXSTRF") {
                        ACCSTRFmax.push(accstatisticresult[i]["max"]);
                        ACCSTRFmin.push(accstatisticresult[i]["min"]);
                        ACCSTRFrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "AccXSTLR") {
                        ACCSTLRmax.push(accstatisticresult[i]["max"]);
                        ACCSTLRmin.push(accstatisticresult[i]["min"]);
                        ACCSTLRrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "AccXSTRR") {
                        ACCSTRRmax.push(accstatisticresult[i]["max"]);
                        ACCSTRRmin.push(accstatisticresult[i]["min"]);
                        ACCSTRRrms.push(accstatisticresult[i]["rms"])
                    }
                    
                }

                for (var i in accstatisticresult) {

                    if (accstatisticresult[i]['chantitle'] == "AccYWhlLF") {
                        ACCWHLLFmax.push(accstatisticresult[i]["max"]);
                        ACCWHLLFmin.push(accstatisticresult[i]["min"]);
                        ACCWHLLFrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "AccYWhlRF") {
                        ACCWHLRFmax.push(accstatisticresult[i]["max"]);
                        ACCWHLRFmin.push(accstatisticresult[i]["min"]);
                        ACCWHLRFrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "AccYWhlLR") {
                        ACCWHLLRmax.push(accstatisticresult[i]["max"]);
                        ACCWHLLRmin.push(accstatisticresult[i]["min"]);
                        ACCWHLLRrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "AccYWhlRR") {
                        ACCWHLRRmax.push(accstatisticresult[i]["max"]);
                        ACCWHLRRmin.push(accstatisticresult[i]["min"]);
                        ACCWHLRRrms.push(accstatisticresult[i]["rms"])
                    }

                    if (accstatisticresult[i]['chantitle'] == "AccYSTLF") {
                        ACCSTLFmax.push(accstatisticresult[i]["max"]);
                        ACCSTLFmin.push(accstatisticresult[i]["min"]);
                        ACCSTLFrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "AccYSTRF") {
                        ACCSTRFmax.push(accstatisticresult[i]["max"]);
                        ACCSTRFmin.push(accstatisticresult[i]["min"]);
                        ACCSTRFrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "AccYSTLR") {
                        ACCSTLRmax.push(accstatisticresult[i]["max"]);
                        ACCSTLRmin.push(accstatisticresult[i]["min"]);
                        ACCSTLRrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "AccYSTRR") {
                        ACCSTRRmax.push(accstatisticresult[i]["max"]);
                        ACCSTRRmin.push(accstatisticresult[i]["min"]);
                        ACCSTRRrms.push(accstatisticresult[i]["rms"])
                    }
                }
                for (var i in accstatisticresult) {

                    if (accstatisticresult[i]['chantitle'] == "AccZWhlLF") {
                        ACCWHLLFmax.push(accstatisticresult[i]["max"]);
                        ACCWHLLFmin.push(accstatisticresult[i]["min"]);
                        ACCWHLLFrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "AccZWhlRF") {
                        ACCWHLRFmax.push(accstatisticresult[i]["max"]);
                        ACCWHLRFmin.push(accstatisticresult[i]["min"]);
                        ACCWHLRFrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "AccZWhlLR") {
                        ACCWHLLRmax.push(accstatisticresult[i]["max"]);
                        ACCWHLLRmin.push(accstatisticresult[i]["min"]);
                        ACCWHLLRrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "AccZWhlRR") {
                        ACCWHLRRmax.push(accstatisticresult[i]["max"]);
                        ACCWHLRRmin.push(accstatisticresult[i]["min"]);
                        ACCWHLRRrms.push(accstatisticresult[i]["rms"])
                    }

                    if (accstatisticresult[i]['chantitle'] == "AccZSTLF") {
                        ACCSTLFmax.push(accstatisticresult[i]["max"]);
                        ACCSTLFmin.push(accstatisticresult[i]["min"]);
                        ACCSTLFrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "AccZSTRF") {
                        ACCSTRFmax.push(accstatisticresult[i]["max"]);
                        ACCSTRFmin.push(accstatisticresult[i]["min"]);
                        ACCSTRFrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "AccZSTLR") {
                        ACCSTLRmax.push(accstatisticresult[i]["max"]);
                        ACCSTLRmin.push(accstatisticresult[i]["min"]);
                        ACCSTLRrms.push(accstatisticresult[i]["rms"])
                    }
                    if (accstatisticresult[i]['chantitle'] == "AccZSTRR") {
                        ACCSTRRmax.push(accstatisticresult[i]["max"]);
                        ACCSTRRmin.push(accstatisticresult[i]["min"]);
                        ACCSTRRrms.push(accstatisticresult[i]["rms"])
                    }
                }
              
                //4个轮心XYZ极大值对比
                Highcharts.chart('ACCFWHLMAX', {
                    chart: {

                        type: 'column',
                        zoomType: 'x'
                    },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    title: {
                        text: '轮心最大值',

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
                        text: '轮心最小值',

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
                        text: '轮心RMS',

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

                //4个塔柱XYZ极大值对比
                Highcharts.chart('ACCFSTMAX', {
                    chart: {

                        type: 'column',
                        zoomType: 'x'
                    },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    title: {
                        text: '塔柱最大值',

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
                        data: eval(ACCSTLFmax)
                    }, {
                        name: 'RF',
                        data: eval(ACCSTRFmax)

                    }, {
                        name: 'LR',
                        data: eval(ACCSTLRmax)
                    }, {
                        name: 'RR',
                        data: eval(ACCSTRRmax)
                    }]
                });

                Highcharts.chart('ACCFSTMIN', {
                    chart: {

                        type: 'column',
                        zoomType: 'x'
                    },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    title: {
                        text: '塔柱最小值',

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
                        data: eval(ACCSTLFmin)
                    }, {
                        name: 'RF',
                        data: eval(ACCSTRFmin)

                    }, {
                        name: 'LR',
                        data: eval(ACCSTLRmin)
                    }, {
                        name: 'RR',
                        data: eval(ACCSTRRmin)
                    }]
                });

                Highcharts.chart('ACCFSTRMS', {
                    chart: {

                        type: 'column',
                        zoomType: 'x'
                    },
                    credits: {
                        enabled: false // 禁用版权信息
                    },
                    title: {
                        text: '塔柱RMS',

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
                        data: eval(ACCSTLFrms)
                    }, {
                        name: 'RF',
                        data: eval(ACCSTRFrms)

                    }, {
                        name: 'LR',
                        data: eval(ACCSTLRrms)
                    }, {
                        name: 'RR',
                        data: eval(ACCSTRRrms)
                    }]
                });
            }

        }
       /* console.timeEnd("ReloadDataACC");*/
    });

    connection.start().then(function () {
        layer.msg("已开始监视");
       
    }).catch(function (err) {
        setTimeout(() => start(), 10000);
        return console.error(err.toString());
    });

    async function start() {
        try {
            await connection.start();
            console.log("connected");
        } catch (err) {
            console.log(err);
            setTimeout(() => start(), 10000);
        }
    };

    connection.onclose(async (e) => {
      
        await  start();
    });


});