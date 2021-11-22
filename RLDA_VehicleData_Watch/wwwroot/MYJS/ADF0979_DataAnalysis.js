layui.use(['element', 'layer', 'laydate', 'table', 'form'], function () {
    var $ = layui.jquery;
    layer = layui.layer;
    var table = layui.table;
    var element = layui.element;
    var laydate = layui.laydate;
    var datevalue,startdate, enddate,brakecount;
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
    laydate.render({
        elem: '#startend'
        , type: 'datetime'
        , range: '到'
        , format: 'yyyy-M-d H:m:s'
        , done: function (val, date, endDate) {
            datevalue = val;
            var startenddate = val.split("到");
            startdate = startenddate[0];
           
            enddate = startenddate[1];
          
        }


    });


    $("#SpeedAnalysis").click(function () {

        if (datevalue != undefined) {
            var index = layer.load();

            $.ajax({

                type: "POST",
                //请求的媒体类型
                dataType: 'json',
                //请求地址
                url: urlgetdatetime,
                data: {
                    startdate: startdate,
                    
                    enddate: enddate,
                    
                },
                success: function (data) {
                    
                    var item = data[0];
                    var time = "time";
                    var sum0_10 = "sum0_10";
                    var sum10_20 = "sum10_20";
                    var sum20_30 = "sum20_30";
                    var sum30_40 = "sum30_40";
                    var sum40_50 = "sum40_50";
                    var sum50_60 = "sum50_60";
                    var sum60_70 = "sum60_70";
                    var sum70_80 = "sum70_80";
                    var sum80_90 = "sum80_90";
                    var sum90_100 = "sum90_100";
                    var sum100_110 = "sum100_110";
                    var sum110_120 = "sum110_120";
                    var sumabove120 = "sumabove120";
                    var TotalDis = 0;
                    var speeddata = [];
                    speeddata.push(item[sum0_10] / 1000);
                    speeddata.push(item[sum10_20] / 1000);
                    speeddata.push(item[sum20_30] / 1000);
                    speeddata.push(item[sum30_40] / 1000);
                    speeddata.push(item[sum40_50] / 1000);
                    speeddata.push(item[sum50_60] / 1000);
                   
                    speeddata.push(item[sum60_70] / 1000);
                    speeddata.push(item[sum70_80] / 1000);
                    speeddata.push(item[sum80_90] / 1000);
                    speeddata.push(item[sum90_100] / 1000);
                    speeddata.push(item[sum100_110] / 1000);
                    speeddata.push(item[sum110_120] / 1000);
                    speeddata.push(item[sumabove120] / 1000);
                    
                    var sumtitle = [];
                    sumtitle.push(sum0_10);
                    sumtitle.push(sum10_20);
                    sumtitle.push(sum20_30);
                    sumtitle.push(sum30_40);
                    sumtitle.push(sum40_50);
                    sumtitle.push(sum50_60);
                    
                    sumtitle.push(sum60_70);
                    sumtitle.push(sum70_80);
                    sumtitle.push(sum80_90);
                    sumtitle.push(sum90_100);
                    sumtitle.push(sum100_110);
                    sumtitle.push(sum110_120);
                    sumtitle.push(sumabove120);


                    for (var i in speeddata) {
                        TotalDis+=speeddata[i]
                    }
                    
                    var speedpiedata = [];
                    for (var i = 0; i < speeddata.length; i++) {
                        speedpiedata.push([sumtitle[i], speeddata[i]]);
                    }
                    //console.log(speedpiedata);
                    Highcharts.chart('columncontainer', {
                        chart: {

                            type: 'column',
                            zoomType: 'x'
                        },
                        credits: {
                            enabled: false // 禁用版权信息
                        },
                        title: {
                            text: '速度分布图',

                        },
                        subtitle: {
                            text: "总里程：" + TotalDis.toFixed(1)+" 公里"
                        },
                        xAxis: {
                            categories: [
                                '0到10', '10到20', '20到30', '30到40', '40到50', '50到60', '60到70', '70到80', '80到90', '90到100', '100到110', '110到120','Above120'
                            ],

                        },
                        yAxis: {

                            title: {
                                text: '公里'
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
                            name: item[time],
                            data: eval(speeddata)
                        }]


                    });


                    Highcharts.chart('barcontainer', {
                        chart: {
                            plotBackgroundColor: null,
                            plotBorderWidth: null,
                            plotShadow: false,
                            type: 'pie'
                        },
                        credits: {
                            enabled: false // 禁用版权信息
                        },
                        title: {
                            text: "速度占比图"
                        },
                        subtitle: {
                            text: item[time]
                        },
                        tooltip: {
                            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
                        },
                        plotOptions: {
                            pie: {
                                allowPointSelect: true,
                                cursor: 'pointer',
                                dataLabels: {
                                    enabled: true,
                                    format: '<b>{point.name}</b>: {point.percentage:.1f} %',
                                    style: {
                                        color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                    }

                                },

                                showInLegend: true
                            }
                        },
                        series: [{
                            name: '速度',
                            colorByPoint: true,
                            data: eval(speedpiedata)
                        }],
                        exporting: {
                            width: 1000
                        }
                    });


                



                    layer.close(index);
                }
                , error: function (e) {
                    layer.msg(e);
                }

            });

            $.ajax({

                type: "POST",
                //请求的媒体类型
                dataType: 'json',
                //请求地址
                url: urlgetdatetimeperday,
                data: {
                    startdate: startdate,

                    enddate: enddate,

                },
                success: function (data) {
                    var distanceperday = [];
                    var perday = [];
                    for (var i = 0; i < data.length; i++) {
                        var item = data[i];
                        var daytime = item["day"].year + "-" + item["day"].month + "-" + item["day"].day;
                        distanceperday.push(item["distance"]/1000);
                        perday.push(daytime);
                        //console.log(item["day"]);
                        //console.log(item["distance"]);
                        //console.log(item["day"].year);
                    }
                    //console.log(perday);
                  
                    Highcharts.chart('speedperdaycolumncontainer', {
                        chart: {

                            type: 'column',
                            zoomType: 'x'
                        },
                        credits: {
                            enabled: false // 禁用版权信息
                        },
                        title: {
                            text: '里程分布图（每天）',

                        },
                        subtitle: {
                            text: startdate+"to"+enddate+"  共"+data.length+"天"
                        },
                        xAxis: {
                            categories: eval(perday),

                        },
                        yAxis: {

                            title: {
                                text: '公里'
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
                          name:"日期",
                            data: eval(distanceperday)
                        }]


                    });


                    layer.close(index);
                }
                , error: function (e) {
                    layer.msg(e);
                }

            });

            $.ajax({

                type: "POST",
                //请求的媒体类型
                dataType: 'json',
                //请求地址
                url: urlgetdatetimeperhour,
                data: {
                    startdate: startdate,

                    enddate: enddate,

                },
                success: function (data) {
                    //console.log(data);
                    var distanceperhour = [];
                    var perhour = [];
                    for (var i = 0; i < data.length; i++) {
                        var item = data[i];
                       
                        distanceperhour.push(item["distance"] / 1000);
                        perhour.push(item["hour"]);

                    }
                    

                    Highcharts.chart('speedperhourcolumncontainer', {
                        chart: {

                            type: 'column',
                            zoomType: 'x'
                        },
                        credits: {
                            enabled: false // 禁用版权信息
                        },
                        title: {
                            text: '里程分布图（每小时时刻）',

                        },
                        subtitle: {
                            text: startdate + "to" + enddate
                        },
                        xAxis: {
                            categories: eval(perhour),

                        },
                        yAxis: {

                            title: {
                                text: '公里'
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
                            name:"小时",
                            data: eval(distanceperhour)
                        }]


                    });



                    layer.close(index);
                }
                , error: function (e) {
                    layer.msg(e);
                }

            });

            $.ajax({

                type: "POST",
                //请求的媒体类型
                dataType: 'json',
                //请求地址
                url: urlgetWFTdamage,
                data: {
                    startdate: startdate,

                    enddate: enddate,

                },
                success: function (data) {
                    
                    var LFdamage = [];
                    var RFdamage = [];
                    var LRdamage = [];
                    var RRdamage = [];
                    //var chantitle = [];
                    for (var i = 0; i < data.length; i++) {
                        var item = data[i];
                        if (item["chantitle"] == "WFT_Fx_LF" || item["chantitle"] == "WFT_Fy_LF" || item["chantitle"] == "WFT_Fz_LF") {
                            LFdamage.push(item["damage"])
                        }
                        if (item["chantitle"] == "WFT_Fx_RF" || item["chantitle"] == "WFT_Fy_RF" || item["chantitle"] == "WFT_Fz_RF") {
                            RFdamage.push(item["damage"])
                        }
                        if (item["chantitle"] == "WFT_Fx_LR" || item["chantitle"] == "WFT_Fy_LR" || item["chantitle"] == "WFT_Fz_LR") {
                            LRdamage.push(item["damage"])
                        }
                        if (item["chantitle"] == "WFT_Fx_RR" || item["chantitle"] == "WFT_Fy_RR" || item["chantitle"] == "WFT_Fz_RR") {
                            RRdamage.push(item["damage"])
                        }
                    }

                    //console.log(LFdamage);
                    //console.log(LRdamage);
                    //console.log(RFdamage);
                    Highcharts.chart('WFTcolumncontainer', {
                        chart: {

                            type: 'column',
                            
                        },
                        credits: {
                            enabled: false // 禁用版权信息
                        },
                        title: {
                            text: 'WFT损伤累积',

                        },
                        subtitle: {
                            text: startdate + "to" + enddate
                        },
                        xAxis: {
                            categories: ['X', 'Y', 'Z'],
                            crosshair: true

                        },
                        yAxis: {

                            title: {
                                text: '损伤'
                            }
                        },
                        tooltip: {
                            headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                            pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                                '<td style="padding:0"><b>{point.y:.1f} damage</b></td></tr>',
                            footerFormat: '</table>',
                            shared: true,
                            useHTML: true
                        },
                        plotOptions: {
                            column: {
                                borderWidth: 0
                            }
                        },
                        series: [{
                            name: "LF",
                            data: eval(LFdamage)
                        }, {
                                name: "RF",
                                data: eval(RFdamage)
                            }, {
                                name: "LR",
                                data: eval(LRdamage)
                            }, {
                                name: "RR",
                                data: eval(RRdamage)
                            }]
                        


                    });



                    layer.close(index);
                }
                , error: function (e) {
                    layer.msg(e);
                }

            });

            $.ajax({

                type: "POST",
                //请求的媒体类型
                dataType: 'json',
                //请求地址
                url: urlgetbrakecount,
                data: {
                    startdate: startdate,

                    enddate: enddate,
                    vehicleid: 'ADF0979'
                },
                success: function (data) {
                   
                    brakecount = data;
                }
                , error: function (e) {
                    layer.msg(e);
                }

            });

            $.ajax({

                type: "POST",
                //请求的媒体类型
                dataType: 'json',
                //请求地址
                url: urlgetbrakedata,
                data: {
                    startdate: startdate,

                    enddate: enddate,
                    vehicleid:'ADF0979'
                },
                success: function (data) {
                    
                   
                    Highcharts.chart('brakehistogramcontainer', {
                       
                        credits: {
                            enabled: false // 禁用版权信息
                        },
                        title: {
                            text: '刹车强度分布图',

                        },
                        subtitle: {
                            text: startdate + "to" + enddate + "一共有" + brakecount+"刹车次数"
                        },
                        xAxis: [{
                            title: { text: '刹车强度'}
                        }, {
                            title: { text: '刹车强度' },
                            opposite: true
                        }],
                        yAxis: [{
                            title: { text: '刹车强度'}
                        }, {
                            title: { text: '频次' },
                            opposite: true
                        }],
                        
                        series: [{
                            name: 'Histogram',
                            type: 'histogram',
                            xAxis: 1,
                            yAxis: 1,
                            baseSeries: 's1',
                            zIndex: -1
                        }, {
                            name: 'Data',
                            type: 'scatter',
                            data: data,
                            id: 's1',
                            marker: {
                                radius: 1.5
                            }
                        }]

                    });



                    layer.close(index);
                }
                , error: function (e) {
                    layer.msg(e);
                }

            });


        }
        else {
            //console.log("jieshu");
            layer.msg("请先选择日期！！！");
        }
    });
});