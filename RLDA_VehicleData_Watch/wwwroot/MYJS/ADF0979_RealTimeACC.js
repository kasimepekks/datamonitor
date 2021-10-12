Highcharts.setOptions({
    lang: {
        contextButtonTitle: "图表导出菜单",
        decimalPoint: ".",
        downloadJPEG: "下载JPEG图片",
        downloadPDF: "下载PDF文件",
        downloadPNG: "下载PNG文件",
        downloadSVG: "下载SVG文件",
        drillUpText: "返回 {series.name}",
        loading: "加载中",
        months: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"],
        noData: "没有数据",

        printChart: "打印图表",
        resetZoom: "恢复缩放",
        resetZoomTitle: "恢复图表",
        shortMonths: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"],
        thousandsSep: ",",
        weekdays: ["星期一", "星期二", "星期三", "星期三", "星期四", "星期五", "星期六", "星期天"]
    }
});

//import _ from 'lodash';
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
layui.use(['element', 'layer', 'table', 'form'], function () {

    
    //import 'default-passive-events';
  
    var $ = layui.jquery;
    layer = layui.layer;
    var speedout = [];
    var n = 0;
    //$("#StartUpload").click(function () {
    //    $.ajax({
    //        //请求方式
    //        type: "POST",

    //        url: urlupload,
    //        //数据，json字符串

    //        //请求成功
    //        success: function (result) {
    //            layer.msg(result);
    //        },
    //    });



    //});
    var connection = new signalR.HubConnectionBuilder().withUrl("/MyHub").build();
    let ref = true;
    var firstspeed;
    var id="";
    function requestData() {


        $.ajax({

            type: "POST",
            //请求的媒体类型
            contentType: "application/json;charset=UTF-8",
            //请求地址
            url: urlgetdata,

            success: function (result) {
                //console.log(result);
                if (result.length > 0 && result[0]["id"]!=id)
              { 
                id = result[0]["id"];               //var array = result["data"];
                var time = [];
                var speed = [];
                var speedsingle = [];
                //var dataGyro_X = [];
                //var dataGyro_Y = [];
                //var dataGyro_Z = [];
                //var singledataGyro_X = [];
                //var singledataGyro_Y = [];
                //var singledataGyro_Z = [];
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

                

                for (var i in result) {
                    if (result[i]["accXWhlLf"] > 100 || result[i]["accXWhlRf"] > 100 || result[i]["accXWhlLr"] > 100 || result[i]["accXWhlRr"] > 100) {
                        if (n < 10000) {
                            n++;
                        }
                        
                    }
                    time.push(result[i]["time"]);
                    speedsingle.push(result[i]["speed"]);
                    //singledataGyro_X.push(result[i]["Gyro_X"]);
                    //singledataGyro_Y.push(result[i]["Gyro_Y"]);
                    //singledataGyro_Z.push(result[i]["Gyro_Z"]);

                    //dataGyro_X.push([result[i]["Time"], result[i]["Gyro_X"]]);
                    //dataGyro_Y.push([result[i]["Time"], result[i]["Gyro_Y"]]);
                    //dataGyro_Z.push([result[i]["Time"], result[i]["Gyro_Z"]]);
                    speed.push([result[i]["time"], result[i]["speed"]]);


                    dataAcc_X_Whl_LF.push([result[i]["time"], result[i]["accXWhlLf"]]);
                    dataAcc_X_Whl_RF.push([result[i]["time"], result[i]["accXWhlRf"]]);
                    dataAcc_X_Whl_LR.push([result[i]["time"], result[i]["accXWhlLr"]]);
                    dataAcc_X_Whl_RR.push([result[i]["time"], result[i]["accXWhlRr"]]);
                    dataAcc_Y_Whl_LF.push([result[i]["time"], result[i]["accYWhlLf"]]);
                    dataAcc_Y_Whl_RF.push([result[i]["time"], result[i]["accYWhlRf"]]);
                    dataAcc_Y_Whl_LR.push([result[i]["time"], result[i]["accYWhlLr"]]);
                    dataAcc_Y_Whl_RR.push([result[i]["time"], result[i]["accYWhlRr"]]);
                    dataAcc_Z_Whl_LF.push([result[i]["time"], result[i]["accZWhlLf"]]);
                    dataAcc_Z_Whl_RF.push([result[i]["time"], result[i]["accZWhlRf"]]);
                    dataAcc_Z_Whl_LR.push([result[i]["time"], result[i]["accZWhlLr"]]);
                    dataAcc_Z_Whl_RR.push([result[i]["time"], result[i]["accZWhlRr"]]);

                    dataAcc_X_FM.push([result[i]["time"], result[i]["accXFm"]]);
                    dataAcc_X_RM.push([result[i]["time"], result[i]["accXRm"]]);

                    dataAcc_Y_FM.push([result[i]["time"], result[i]["accYFm"]]);
                    dataAcc_Y_RM.push([result[i]["time"], result[i]["accYRm"]]);

                    dataAcc_Z_FM.push([result[i]["time"], result[i]["accZFm"]]);
                    dataAcc_Z_RM.push([result[i]["time"], result[i]["accZRm"]]);
                }
                if (n>0) {
                    $("#warninginfo").text(n);
                    fileinfo.push(result[0]["id"]);
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
                        text: getNowFormatDate()
                    },
                    subtitle: {
                        text: id
                    },
                    tooltip: {
                        valueDecimals: 2
                    },
                    series: [{
                    
                        name: 'Acc_X_Whl_LF',
                        data: dataAcc_X_Whl_LF

                    },
                    {
                        name: 'Acc_X_Whl_RF',
                        data: dataAcc_X_Whl_RF

                    },
                    {
                        name: 'Acc_X_Whl_LR',
                        data: dataAcc_X_Whl_LR

                    },
                    {
                        name: 'Acc_X_Whl_RR',
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
                            text: getNowFormatDate()
                        },
                        subtitle: {
                            text: id
                        },
                        tooltip: {
                            valueDecimals: 2
                        },
                        series: [
                            {
                                name: 'Acc_Y_Whl_LF',
                                data: dataAcc_Y_Whl_LF

                            },
                            {
                                name: 'Acc_Y_Whl_RF',
                                data: dataAcc_Y_Whl_RF

                            },
                            {
                                name: 'Acc_Y_Whl_LR',
                                data: dataAcc_Y_Whl_LR

                            },
                            {
                                name: 'Acc_Y_Whl_RR',
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
                        text: getNowFormatDate()
                    },
                    subtitle: {
                        text: id
                    },
                    tooltip: {
                        valueDecimals: 2
                    },
                    series: [
                    {
                        name: 'Acc_Z_Whl_LF',
                        data: dataAcc_Z_Whl_LF

                    },
                    {
                            name: 'Acc_Z_Whl_RF',
                            data: dataAcc_Z_Whl_RF

                    },
                    {
                        name: 'Acc_Z_Whl_LR',
                        data: dataAcc_Z_Whl_LR

                    },
                    {
                        name: 'Acc_Z_Whl_RR',
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
                        text: getNowFormatDate()
                    },
                    subtitle: {
                        text: id
                    },
                    tooltip: {
                        valueDecimals: 2
                    },
                    series: [{

                        name: 'Acc_X_FM',
                        data: dataAcc_X_FM

                    },
                    {
                        name: 'Acc_X_RM',
                        data: dataAcc_X_RM

                    },
                    {
                        name: 'Acc_Y_FM',
                        data: dataAcc_Y_FM

                    },
                    {
                        name: 'Acc_Y_RM',
                        data: dataAcc_Y_RM

                    },
                    {
                        name: 'Acc_Z_FM',
                        data: dataAcc_Z_FM

                    },
                    {
                        name: 'Acc_Z_RM',
                        data: dataAcc_Z_RM

                    }
                    ]




                });


               


              }
                
            },

            cache: false

        });

       


    }

    connection.on("ReloadData", function () {

        requestData();
    });
    connection.start().then(function () {
        layer.msg("已开始监视");
        //document.getElementById("StartUpload").disabled = false;
    }).catch(function (err) {
        return console.error(err.toString());
    });
    //$("#StartWatching").click(function () {

    //    if (ref == true) {
    //        requestData(); 
    //        var chart = Highcharts.chart('container', {
    //            chart: {
    //                type: 'gauge',
    //                plotBackgroundColor: null,
    //                plotBackgroundImage: null,
    //                plotBorderWidth: 0,
    //                plotShadow: false
    //            },
    //            credits: {
    //                enabled: false // 禁用版权信息
    //            },
    //            title: false,
    //            pane: {
    //                startAngle: -150,
    //                endAngle: 150,
    //                background: [{
    //                    backgroundColor: {
    //                        linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
    //                        stops: [
    //                            [0, '#FFF'],
    //                            [1, '#333']
    //                        ]
    //                    },
    //                    borderWidth: 0,
    //                    outerRadius: '109%'
    //                }, {
    //                    backgroundColor: {
    //                        linearGradient: { x1: 0, y1: 0, x2: 0, y2: 1 },
    //                        stops: [
    //                            [0, '#333'],
    //                            [1, '#FFF']
    //                        ]
    //                    },
    //                    borderWidth: 1,
    //                    outerRadius: '107%'
    //                }, {
    //                    // default background
    //                }, {
    //                    backgroundColor: '#DDD',
    //                    borderWidth: 0,
    //                    outerRadius: '105%',
    //                    innerRadius: '103%'
    //                }]
    //            },
    //            // the value axis
    //            yAxis: {
    //                min: 0,
    //                max: 200,
    //                minorTickInterval: 'auto',
    //                minorTickWidth: 1,
    //                minorTickLength: 10,
    //                minorTickPosition: 'inside',
    //                minorTickColor: '#666',
    //                tickPixelInterval: 30,
    //                tickWidth: 2,
    //                tickPosition: 'inside',
    //                tickLength: 10,
    //                tickColor: '#666',
    //                labels: {
    //                    step: 2,
    //                    rotation: 'auto'
    //                },
    //                title: {
    //                    text: 'km/h'
    //                },
    //                plotBands: [{
    //                    from: 0,
    //                    to: 120,
    //                    color: '#55BF3B' // green
    //                }, {
    //                    from: 120,
    //                    to: 160,
    //                    color: '#DDDF0D' // yellow
    //                }, {
    //                    from: 160,
    //                    to: 200,
    //                    color: '#DF5353' // red
    //                }]
    //            },
    //            series: [{
    //                name: 'Speed',
    //                data: [0],
    //                tooltip: {
    //                    valueSuffix: ' km/h'
    //                }
    //            }]
    //        }, function (chart) {

    //            var i = 0;
    //            var g = 1;
    //                setInterval(function () {
                        
    //                    if (chart.series[0].points[0] != null || chart.series[0].points[0] != undefined) {
                          
    //                    var point = chart.series[0].points[0];
    //                }
    //                if (speedout[i] != null && speedout[g] != firstspeed) {
    //                    point.update(eval(speedout[i]));
    //                    i += 511;
    //                }
    //                else {
    //                    i = 0;
    //                    g = 0;
    //                }

    //            }, 1000);

    //        });

    //        layer.msg("已开启监控");
    //        ref = false;
    //    } else {
    //        clearTimeout(timer1);
    //        layer.msg("已关闭监控");
    //        ref = true;
           
              
                
    //         }
    //});
  
  
    $("#loginfo").click(function () {

        layer.open({
            type: 2,
            title: '数据错误日志信息',
            shadeClose: true,
            shade: 0.8,
            area: ['720px', '80%'],
            content: urlloginfo, //iframe的url
            btn: ['确认', '取消'],
           
            success: function (layero, index) {
               
                bodylog = layer.getChildFrame('body', index);
                for (var i in fileinfo) {
                    bodylog.append('<p>' + "警告！文件名为" + fileinfo[i] + "的数据有问题，请查看源数据" + '</p>');
                }
               
            }
        });


    });


});


