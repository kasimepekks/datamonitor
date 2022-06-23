


layui.use(['element', 'layer', 'laydate', 'table', 'form'], function () {
    var $ = layui.jquery;
    layer = layui.layer;
    
    var element = layui.element;
    var laydate = layui.laydate;
    var datevalue, startdate, enddate, brakecount, bumpcount;
    var a = GetRequest();
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
            //text
            $.ajax({

                type: "POST",
                //请求的媒体类型
                dataType: 'json',
                //请求地址
                url: urlgettextdata,
                data: {
                    startdate: startdate,

                    enddate: enddate,
                    vehicleid: a['id']
                },

                success: function (data) {
                    if (data.length != 0) {
                        

                        document.getElementById('time').innerHTML = startdate + "to" + enddate;
                        document.getElementById('currentmile').innerHTML = (data[0]/1000).toFixed(1)+" km";
                        document.getElementById('accummile').innerHTML = (data[1]/1000).toFixed(1)+" km";

                    }
                   

                }
                , error: function (e) {
                    layer.msg(e);
                }
            });
            //速度分布图表
            $.ajax({

                type: "POST",
                //请求的媒体类型
                dataType: 'json',
                //请求地址
                url: urlgetdatetime,
                data: {
                    startdate: startdate,

                    enddate: enddate,
                    vehicleid: a['id']
                },
                success: function (data) {
                    if (data.length != 0) {
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

                        var sum0_30 = "0-30";
                        var sum30_60 = "30-60";
                        var sum60_90 = "60-90";
                        var sum90_120 = "90-120";
                        var sumabove = "120-";


                        var TotalDis = 0;
                        var speeddata = [];
                        var speeddata2 = [];

                        speeddata2.push((item[sum0_10] + item[sum10_20] + item[sum20_30]) / 1000);
                        speeddata2.push((item[sum30_40] + item[sum40_50] + item[sum50_60]) / 1000);
                        speeddata2.push((item[sum60_70] + item[sum70_80] + item[sum80_90]) / 1000);
                        speeddata2.push((item[sum90_100] + item[sum100_110] + item[sum110_120]) / 1000);
                        speeddata2.push(item[sumabove120] / 1000);

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
                        var sumtitle2 = [];
                        sumtitle2.push(sum0_30);
                        sumtitle2.push(sum30_60);
                        sumtitle2.push(sum60_90);
                        sumtitle2.push(sum90_120);
                        sumtitle2.push(sumabove);


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
                            TotalDis += speeddata[i]
                        }

                        var speedpiedata = [];
                        for (var i = 0; i < speeddata2.length; i++) {
                            speedpiedata.push([sumtitle2[i], speeddata2[i]]);
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
                                text: "总里程：" + TotalDis.toFixed(1) + " 公里"
                            },
                            xAxis: {
                                categories: [
                                    '0到10', '10到20', '20到30', '30到40', '40到50', '50到60', '60到70', '70到80', '80到90', '90到100', '100到110', '110到120', 'Above120'
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
                                name: '里程',
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
                    }
                    else {
                        layer.close(index);
                    }

                }
                , error: function (e) {
                    layer.msg(e);
                }

            });
            //里程分布图（每天）
            $.ajax({

                type: "POST",
                //请求的媒体类型
                dataType: 'json',
                //请求地址
                url: urlgetdatetimeperday,
                data: {
                    startdate: startdate,

                    enddate: enddate,
                    vehicleid: a['id']
                },
                success: function (data) {
                    if (data.length != 0) {
                        var distanceperday = [];
                        var perday = [];
                        for (var i = 0; i < data.length; i++) {
                            var item = data[i];
                            var daytime = item["day"].year + "-" + item["day"].month + "-" + item["day"].day;
                            distanceperday.push(item["distance"] / 1000);
                            perday.push(daytime);

                        }

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
                                text: startdate + "to" + enddate + "  共" + data.length + "天"
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
                                name: "里程",
                                data: eval(distanceperday)
                            }]


                        });
                    }

                    else {
                        layer.close(index);
                    }


                }
                , error: function (e) {
                    layer.msg(e);
                }

            });
            //里程分布图（每小时时刻）
            $.ajax({

                type: "POST",
                //请求的媒体类型
                dataType: 'json',
                //请求地址
                url: urlgetdatetimeperhour,
                data: {
                    startdate: startdate,

                    enddate: enddate,
                    vehicleid: a['id']
                },
                success: function (data) {
                    if (data.length != 0) {

                        var distanceperhour = [];
                        var perhour = [];
                        for (var i = 0; i < data.length; i++) {
                            var item = data[i];

                            distanceperhour.push(item["distance"] / 1000);
                            perhour.push(item["hour"]-6<0?24+item["hour"]-6:item["hour"]-6);

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
									 title: {
                                    text: '点时刻'
                                }

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
                                name: "里程",
                                data: eval(distanceperhour)
                            }]


                        });
                    }
                    else {
                        layer.close(index);
                    }

                }
                , error: function (e) {
                    layer.msg(e);
                }

            });

            //轨迹
            $.ajax({

                type: "POST",
                //请求的媒体类型
                dataType: 'json',
                //请求地址
                url: urlgetgpsdata,
                data: {
                    startdate: startdate,

                    enddate: enddate,
                    vehicleid: a['id']
                },
                success: function (data) {
                    if (data.length != 0) {
                        var gpscount = data.length;

                        var map;
                        var myIcon;
                        var marker;
                        var maxlabel;
                        var markermaxspeed
                       
                        var maxspeedgpspointreal = [];
                        var maxspeedgpstranspointreal = [];
                        
                        var speedmaxreal;
                        if (navigator.onLine) {

                             myIcon = new BMap.Icon('/Pictures/car.png',
                                new BMap.Size(52, 26), {
                                anchor: new BMap.Size(27, 13)
                            });
                            /* 百度地图API功能*/
                            map = new BMap.Map("mapcontainer");
                           
                            map.enableScrollWheelZoom();
                            map.centerAndZoom();
                            document.getElementById('mapwrapper').removeAttribute('hidden');
                            var allPoint = [];
                            var transPoint = [];
                            var testpoint = [];
                            var speedpoint = [];
                            
                            
                            var index=0;
                            var lushu;
                            var k = 20;
                            var callbackindex = 0;
                            var totalspeed = 0;
                            for (var i = 0; i < gpscount; i++) {
                                var item = data[i];
                                //获取除了最后一个数之前的所有gps点存储到allpoint
                                if (i < gpscount - 1) {
                                    allPoint.push(new BMap.Point(item["lon"], item["lat"]));
                                   
                                    totalspeed += item["speed"];
                                    if ((i + 1) % k == 0 && i != 0) {
                                        speedpoint.push(totalspeed / k);
                                        totalspeed = 0;
                                    }
                                    if (i == gpscount - 2) {
                                        speedpoint.push(totalspeed / (gpscount % k));
                                    }
                                }
                                //获取最后一个数存储到maxspeedgpspointreal，最后一个数是真实的最大speed点
                                else {
                                    maxspeedgpspointreal.push(new BMap.Point(item["lon"], item["lat"]));
                                    speedmaxreal = item["speed"]
                                }
                                
                             
                            }
                            

                            callback = function (xyResult) {
                                
                                for (var i = 0; i < xyResult.length; i++) {
                                    transPoint.push(new BMap.Point(xyResult[i]["x"], xyResult[i]["y"]));
                                    testpoint.push(new BMap.Point(xyResult[i]["x"], xyResult[i]["y"]));
                                }

                               
                                if (speedpoint[callbackindex] > 120) {
                                        map.addOverlay(new BMap.Polyline(testpoint, {
                                            strokeColor: "blue",
                                            strokeWeight: 5,
                                            strokeOpacity: 0.5
                                        }));

                                    }
                                else if (speedpoint[callbackindex] > 80) {
                                        map.addOverlay(new BMap.Polyline(testpoint, {
                                            strokeColor: "blue",
                                            strokeWeight: 5,
                                            strokeOpacity: 0.5
                                        }));
                                    }
                                else if (speedpoint[callbackindex] > 40) {
                                        map.addOverlay(new BMap.Polyline(testpoint, {
                                            strokeColor: "blue",
                                            strokeWeight: 5,
                                            strokeOpacity: 0.5
                                        }));
                                    }
                                    else {
                                        map.addOverlay(new BMap.Polyline(testpoint, {
                                            strokeColor: "blue",
                                            strokeWeight: 5,
                                            strokeOpacity: 0.5
                                        }));
                                    }
                                callbackindex = callbackindex + 1;
                                
                                testpoint = [];
                                map.setViewport(transPoint);

                                marker = new BMap.Marker(transPoint[0], {
                                    //引入小车图标
                                    icon: myIcon
                                });
                                //展示时小车样式
                                var label = new BMap.Label(a['id'], { offset: new BMap.Size(0, -30) });
                                label.setStyle({ border: "2px red rgb(204, 204, 204)", color: "rgb(2, 0, 0)", borderRadius: "10px", padding: "5px", background: "rgb(222, 255, 255)", });
                                marker.setLabel(label);
                                {
                                //map.addOverlay(marker);

                                /*BMapLib.LuShu.prototype._move = function (initPos, targetPos, effect) {
                                    var pointsArr = [initPos, targetPos];  //点数组
                                    var me = this,
                                        //当前的帧数
                                        currentCount = 0,
                                        //步长，米/秒
                                        timer = 10,
                                        step = this._opts.speed / (1000 / timer),
                                        //初始坐标
                                        init_pos = this._projection.lngLatToPoint(initPos),
                                        //获取结束点的(x,y)坐标
                                        target_pos = this._projection.lngLatToPoint(targetPos),
                                        //总的步长
                                        count = Math.round(me._getDistance(init_pos, target_pos) / step);
                                    //显示折线 syj201607191107
                                    // 画线操作
                                    this._map.addOverlay(new BMap.Polyline(pointsArr, {
                                        strokeColor: "blue",
                                        strokeWeight: 5,
                                        strokeOpacity: 0.5
                                    }));
                                    //如果小于1直接移动到下一点
                                    if (count < 1) {
                                        me._moveNext(++me.i);
                                        return;
                                    }
                                    me._intervalFlag = setInterval(function () {
                                        //两点之间当前帧数大于总帧数的时候，则说明已经完成移动
                                        if (currentCount >= count) {
                                            clearInterval(me._intervalFlag);
                                            //移动的点已经超过总的长度
                                            if (me.i > me._path.length) {
                                                return;
                                            }
                                            //运行下一个点
                                            me._moveNext(++me.i);
                                        } else {
                                            currentCount++;
                                            var x = effect(init_pos.x, target_pos.x, currentCount, count),
                                                y = effect(init_pos.y, target_pos.y, currentCount, count),
                                                pos = me._projection.pointToLngLat(new BMap.Pixel(x, y));
                                            //设置marker
                                            if (currentCount == 1) {
                                                var proPos = null;
                                                if (me.i - 1 >= 0) {
                                                    proPos = me._path[me.i - 1];
                                                }
                                                if (me._opts.enableRotation == true) {
                                                    me.setRotation(proPos, initPos, targetPos);
                                                }
                                                if (me._opts.autoView) {
                                                    if (!me._map.getBounds().containsPoint(pos)) {
                                                        me._map.setCenter(pos);
                                                    }
                                                }
                                            }
                                            //正在移动
                                            me._marker.setPosition(pos);
                                            //设置自定义overlay的位置
                                            me._setInfoWin(pos);
                                        }
                                    }, timer);
                                };*/
                                }
                                lushu = new BMapLib.LuShu(map, transPoint, {
                                    defaultContent: a['id'],
                                        autoView: true, //是否开启自动视野调整，如果开启那么路书在运动过程中会根据视野自动调整
                                        icon: myIcon,
                                        enableRotation: true, //是否设置marker随着道路的走向进行旋转
                                        speed: 2000, //速度很有关系，太快和太慢都会导致车标原地不动
                                    landmarkPois: []
                                });
                                    
                                marker.addEventListener("click", function () {
                                    marker.enableMassClear();   //设置后可以隐藏改点的覆盖物
                                    marker.hide();
                                    lushu.start();
                                    //map.clearOverlays();  //清除所有覆盖物
                                });
                                //绑定事件
                                $("run").onclick = function () {
                                    //map.clearOverlays();    //清除所有覆盖物
                                    marker.enableMassClear(); //设置后可以隐藏改点的覆盖物
                                    marker.hide();

                                    callback = function (Result) {
                                        for (var i = 0; i < Result.length; i++) {
                                            maxspeedgpstranspointreal.push(new BMap.Point(Result[i]["x"], Result[i]["y"]));

                                        }
                                        markermaxspeed = new BMap.Marker(maxspeedgpstranspointreal[0]);
                                        var opts = {
                                            position: maxspeedgpstranspointreal[0],    // 指定文本标注所在的地理位置
                                            offset: new BMap.Size(30, -30)    //设置文本偏移量
                                        }
                                        maxlabel = new BMap.Label("Max:" + speedmaxreal, opts);  // 创建文本标注对象
                                        maxlabel.setStyle({
                                            color: "red",
                                            fontSize: "12px",
                                            height: "20px",
                                            lineHeight: "20px",
                                            fontFamily: "微软雅黑"
                                        });

                                    }
                                    
                                    
                                    lushu.start();
                                    map.addOverlay(markermaxspeed);
                                    map.addOverlay(maxlabel);
                                    BMap.Convertor.transMore(maxspeedgpspointreal, 0, callback);

                                    
                                }
                                $("stop").onclick = function () {
                                    lushu.stop();
                                }
                                $("pause").onclick = function () {
                                    lushu.pause();
                                }
                                $("hide").onclick = function () {
                                    lushu.hideInfoWindow();
                                }
                                $("show").onclick = function () {
                                    lushu.showInfoWindow();
                                }
                                function $(element) {
                                    return document.getElementById(element);
                                }
                               
                            }
                            
                            BMap.Convertor.transMore(allPoint, 0, callback);

                            
                            
                        }
                                             
                        document.getElementById('textwrapper').removeAttribute('hidden');
                       
                        document.getElementById('maxspeed').innerHTML = speedmaxreal+" km/h";

                        layer.close(index);
                    }
                    else {
                        layer.close(index);
                    }

                }
                , error: function (e) {
                    layer.msg(e);
                }

            });

           
            //WFT损伤
            $.ajax({

                type: "POST",
                //请求的媒体类型
                dataType: 'json',
                //请求地址
                url: urlgetWFTdamage,
                data: {
                    startdate: startdate,

                    enddate: enddate,
                    vehicleid: a['id']
                },
                success: function (data) {
                    if (data.length!=0) {
                        var LFdamage = [];
                        var RFdamage = [];
                        var LRdamage = [];
                        var RRdamage = [];

                        var LFmax = [];
                        var RFmax = [];
                        var LRmax = [];
                        var RRmax = [];

                        var LFmin = [];
                        var RFmin = [];
                        var LRmin = [];
                        var RRmin = [];
                        //var chantitle = [];
                        for (var i = 0; i < data.length; i++) {
                            var item = data[i];
                            if (item["chantitle"] == "WFTFxLF" || item["chantitle"] == "WFTFyLF" || item["chantitle"] == "WFTFzLF") {
                                LFdamage.push(item["damage"]);
                                LFmax.push(item["max"]);
                                LFmin.push(item["min"]);
                            }
                            if (item["chantitle"] == "WFTFxRF" || item["chantitle"] == "WFTFyRF" || item["chantitle"] == "WFTFzRF") {
                                RFdamage.push(item["damage"]);
                                RFmax.push(item["max"]);
                                RFmin.push(item["min"]);
                            }
                            if (item["chantitle"] == "WFTFxLR" || item["chantitle"] == "WFTFyLR" || item["chantitle"] == "WFTFzLR") {
                                LRdamage.push(item["damage"]);
                                LRmax.push(item["max"]);
                                LRmin.push(item["min"]);
                            }
                            if (item["chantitle"] == "WFTFxRR" || item["chantitle"] == "WFTFyRR" || item["chantitle"] == "WFTFzRR") {
                                RRdamage.push(item["damage"]);
                                RRmax.push(item["max"]);
                                RRmin.push(item["min"]);
                            }
                        }

                        Highcharts.chart('WFTcolumncontainer', {
                            chart: {

                                type: 'column',

                            },
                            credits: {
                                enabled: false // 禁用版权信息
                            },
                            title: {
                                text: '轮心力累积损伤',

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

                        Highcharts.chart('WFTcolumncontainerMax', {
                            chart: {

                                type: 'column',

                            },
                            credits: {
                                enabled: false // 禁用版权信息
                            },
                            title: {
                                text: '轮心力极大值',

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
                                    text: '力（N）'
                                }
                            },
                            tooltip: {
                                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                                    '<td style="padding:0"><b>{point.y:.1f} 最大值</b></td></tr>',
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
                                data: eval(LFmax)
                            }, {
                                name: "RF",
                                data: eval(RFmax)
                            }, {
                                name: "LR",
                                data: eval(LRmax)
                            }, {
                                name: "RR",
                                data: eval(RRmax)
                            }]



                        });

                        Highcharts.chart('WFTcolumncontainerMin', {
                            chart: {

                                type: 'column',

                            },
                            credits: {
                                enabled: false // 禁用版权信息
                            },
                            title: {
                                text: '轮心力极小值',

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
                                    text: '力（N）'
                                }
                            },
                            tooltip: {
                                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                                    '<td style="padding:0"><b>{point.y:.1f} 最小值</b></td></tr>',
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
                                data: eval(LFmin)
                            }, {
                                name: "RF",
                                data: eval(RFmin)
                            }, {
                                name: "LR",
                                data: eval(LRmin)
                            }, {
                                name: "RR",
                                data: eval(RRmin)
                            }]

                        });
                    }
                    
                    else {
                        layer.close(index);
                    }


                }
                , error: function (e) {
                    layer.msg(e);
                }

            });

            //加速度和位移
            $.ajax({

                type: "POST",
                //请求的媒体类型
                dataType: 'json',
                //请求地址
                url: urlgetaccandisdata,
                data: {
                    startdate: startdate,

                    enddate: enddate,
                    vehicleid: a['id']
                },
                success: function (data) {
                    if (data.length != 0) {
                        //这里必须4个方位分开来存储，因为返回的数据顺序不是LF,RF,LR,RR，而是LF,LR,RF,RR
                        var DisLFMax = [];
                        var DisLFMin = [];
                        var DisRFMax = [];
                        var DisRFMin = [];
                        var DisLRMax = [];
                        var DisLRMin = [];
                        var DisRRMax = [];
                        var DisRRMin = [];

                        var DisMax = [];
                        var DisMin = [];

                        var LFmax = [];
                        var RFmax = [];
                        var LRmax = [];
                        var RRmax = [];

                        var LFmin = [];
                        var RFmin = [];
                        var LRmin = [];
                        var RRmin = [];

                        var LFSTmax = [];
                        var RFSTmax = [];
                        var LRSTmax = [];
                        var RRSTmax = [];

                        var LFSTmin = [];
                        var RFSTmin = [];
                        var LRSTmin = [];
                        var RRSTmin = [];
                        //var chantitle = [];
                        for (var i = 0; i < data.length; i++) {
                            var item = data[i];
                            if (item["chantitle"].includes("AccXWhlLF")   || item["chantitle"].includes("AccYWhlLF")   || item["chantitle"].includes("AccZWhlLF")  ) {
                               
                                LFmax.push(item["max"]);
                                LFmin.push(item["min"]);
                            }
                            if (item["chantitle"].includes("AccXWhlRF")   || item["chantitle"].includes("AccYWhlRF")   || item["chantitle"].includes("AccZWhlRF")  ) {
                                
                                RFmax.push(item["max"]);
                                RFmin.push(item["min"]);
                            }
                            if (item["chantitle"].includes("AccXWhlLR")   || item["chantitle"].includes("AccYWhlLR")   || item["chantitle"].includes("AccZWhlLR")  ) {
                                
                                LRmax.push(item["max"]);
                                LRmin.push(item["min"]);
                            }
                            if (item["chantitle"].includes("AccXWhlRR")   || item["chantitle"].includes("AccYWhlRR")   || item["chantitle"].includes("AccZWhlRR")  ) {
                                
                                RRmax.push(item["max"]);
                                RRmin.push(item["min"]);
                            }

                            if (item["chantitle"].includes("AccXSTLF") || item["chantitle"].includes("AccYSTLF") || item["chantitle"].includes("AccZSTLF")) {

                                LFSTmax.push(item["max"]);
                                LFSTmin.push(item["min"]);
                            }
                            if (item["chantitle"].includes("AccXSTRF") || item["chantitle"].includes("AccYSTRF") || item["chantitle"].includes("AccZSTRF")) {

                                RFSTmax.push(item["max"]);
                                RFSTmin.push(item["min"]);
                            }
                            if (item["chantitle"].includes("AccXSTLR") || item["chantitle"].includes("AccYSTLR") || item["chantitle"].includes("AccZSTLR")) {

                                LRSTmax.push(item["max"]);
                                LRSTmin.push(item["min"]);
                            }
                            if (item["chantitle"].includes("AccXSTRR") || item["chantitle"].includes("AccYSTRR") || item["chantitle"].includes("AccZSTRR")) {

                                RRSTmax.push(item["max"]);
                                RRSTmin.push(item["min"]);
                            }

                            if (item["chantitle"].includes("DisDmpLF")  ) {

                                DisLFMax.push(item["max"]);
                                DisLFMin.push(item["min"]);
                            }
                            if (item["chantitle"].includes("DisDmpRF")  ) {

                                DisRFMax.push(item["max"]);
                                DisRFMin.push(item["min"]);
                            }
                            if (item["chantitle"].includes("DisDmpLR")  ) {

                                DisLRMax.push(item["max"]);
                                DisLRMin.push(item["min"]);
                            }
                            if (item["chantitle"].includes("DisDmpRR")  ) {

                                DisRRMax.push(item["max"]);
                                DisRRMin.push(item["min"]);
                            }
                        }

                        

                        Highcharts.chart('WFTACCcolumncontainerMax', {
                            chart: {

                                type: 'column',

                            },
                            credits: {
                                enabled: false // 禁用版权信息
                            },
                            title: {
                                text: '轮心加速度极大值',

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
                                    text: '加速度（g）'
                                }
                            },
                            tooltip: {
                                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                                    '<td style="padding:0"><b>{point.y:.1f} 最大值</b></td></tr>',
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
                                data: eval(LFmax)
                            }, {
                                name: "RF",
                                data: eval(RFmax)
                            }, {
                                name: "LR",
                                data: eval(LRmax)
                            }, {
                                name: "RR",
                                data: eval(RRmax)
                            }]



                        });

                        Highcharts.chart('WFTACCcolumncontainerMin', {
                            chart: {

                                type: 'column',

                            },
                            credits: {
                                enabled: false // 禁用版权信息
                            },
                            title: {
                                text: '轮心加速度极小值',

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
                                    text: '加速度（g）'
                                }
                            },
                            tooltip: {
                                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                                    '<td style="padding:0"><b>{point.y:.1f} 最小值</b></td></tr>',
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
                                data: eval(LFmin)
                            }, {
                                name: "RF",
                                data: eval(RFmin)
                            }, {
                                name: "LR",
                                data: eval(LRmin)
                            }, {
                                name: "RR",
                                data: eval(RRmin)
                            }]

                        });

                        Highcharts.chart('STACCcolumncontainerMax', {
                            chart: {

                                type: 'column',

                            },
                            credits: {
                                enabled: false // 禁用版权信息
                            },
                            title: {
                                text: '塔柱加速度极大值',

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
                                    text: '加速度（g）'
                                }
                            },
                            tooltip: {
                                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                                    '<td style="padding:0"><b>{point.y:.1f} 最大值</b></td></tr>',
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
                                data: eval(LFSTmax)
                            }, {
                                name: "RF",
                                data: eval(RFSTmax)
                            }, {
                                name: "LR",
                                data: eval(LRSTmax)
                            }, {
                                name: "RR",
                                data: eval(RRSTmax)
                            }]



                        });

                        Highcharts.chart('STACCcolumncontainerMin', {
                            chart: {

                                type: 'column',

                            },
                            credits: {
                                enabled: false // 禁用版权信息
                            },
                            title: {
                                text: '塔柱加速度极小值',

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
                                    text: '加速度（g）'
                                }
                            },
                            tooltip: {
                                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                                    '<td style="padding:0"><b>{point.y:.1f} 最小值</b></td></tr>',
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
                                data: eval(LFSTmin)
                            }, {
                                name: "RF",
                                data: eval(RFSTmin)
                            }, {
                                name: "LR",
                                data: eval(LRSTmin)
                            }, {
                                name: "RR",
                                data: eval(RRSTmin)
                            }]

                        });

                        Highcharts.chart('STDIScolumncontainerMax', {
                            chart: {

                                type: 'column',

                            },
                            credits: {
                                enabled: false // 禁用版权信息
                            },
                            title: {
                                text: '塔柱位移极大值',

                            },
                            subtitle: {
                                text: startdate + "to" + enddate
                            },
                           
                            yAxis: {

                                title: {
                                    text: '位移（mm）'
                                }
                            },
                            tooltip: {
                                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                                    '<td style="padding:0"><b>{point.y:.1f} 最大值</b></td></tr>',
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
                                data: eval(DisLFMax)
                            }, {
                                name: "RF",
                                data: eval(DisRFMax)
                            }, {
                                name: "LR",
                                data: eval(DisLRMax)
                            }, {
                                name: "RR",
                                data: eval(DisRRMax)
                            }]

                        });

                        Highcharts.chart('STDIScolumncontainerMin', {
                            chart: {

                                type: 'column',

                            },
                            credits: {
                                enabled: false // 禁用版权信息
                            },
                            title: {
                                text: '塔柱位移极小值',

                            },
                            subtitle: {
                                text: startdate + "to" + enddate
                            },
                           
                            yAxis: {

                                title: {
                                    text: '位移（mm）'
                                }
                            },
                            tooltip: {
                                headerFormat: '<span style="font-size:10px">{point.key}</span><table>',
                                pointFormat: '<tr><td style="color:{series.color};padding:0">{series.name}: </td>' +
                                    '<td style="padding:0"><b>{point.y:.1f} 最小值</b></td></tr>',
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
                                data: eval(DisLFMin)
                            }, {
                                name: "RF",
                                data: eval(DisRFMin)
                            }, {
                                name: "LR",
                                data: eval(DisLRMin)
                            }, {
                                name: "RR",
                                data: eval(DisRRMin)
                            }]

                        });
                    }

                    else {
                        layer.close(index);
                    }


                }
                , error: function (e) {
                    layer.msg(e);
                }

            });
           
            
            //刹车直方图和饼图
            $.ajax({

                type: "POST",
                //请求的媒体类型
                dataType: 'json',
                //请求地址
                url: urlgetbrakedata,
                data: {
                    startdate: startdate,

                    enddate: enddate,
                    vehicleid: a['id']
                },
                success: function (data) {
                    if (data.length!=0) {
                        var brakechart=Highcharts.chart('brakehistogramcontainer', {

                            credits: {
                                enabled: false // 禁用版权信息
                            },
                            title: {
                                text: '制动强度直方图',

                            },
                            subtitle: {
                                text: startdate + "to" + enddate + "一共有" + data.length + "制动次数"
                            },
                            plotOptions: {
                                histogram: {
                                    dataLabels: {
                                        // 开启数据标签
                                        enabled: true
                                    },
                                    // 关闭鼠标跟踪，对应的提示框、点击事件会失效
                                    enableMouseTracking: true
                                }
                            },
                            xAxis: [{
                                title: { text: '制动强度（g）' }
                            }, {
                                title: { text: '制动强度（g）' },
                                opposite: true
                            }],
                            yAxis: [{
                                title: { text: '频次' }
                            }, {
                                title: { text: '频次' },
                                opposite: true
                            }],

                            series: [{
                                name: '频次',
                                type: 'histogram',
                                binsNumber: 'sturges',
                                xAxis: 1,
                                yAxis: 1,
                                baseSeries: 's1',
                                zIndex: -1
                            }, {
                                name: 'Data',
                                type: 'scatter',
                                data: data,
                                id: 's1',
                                visible: false,
                                showInLegend: false,
                                marker: {
                                    radius: 1.5
                                }
                            }]

                        });
                        //取消隐藏
                        document.getElementById('brakewrapper').removeAttribute('hidden');
                        Highcharts.addEvent(document.getElementById('brakewrapper'), 'click', function (e) {
                            var brakebinnumber = parseInt(document.getElementById('brakebinnumber').value);
                            var target = e.target,
                                button = null;
                            if (target.tagName === 'BUTTON') { // 判断点的是否是 button
                                button = target.id;
                                switch (button) {
                                    case 'brakeplain':
                                       
                                        brakechart.update({
                                                series: [{
                                                    name: '频次',
                                                    type: 'histogram',
                                                    binsNumber: brakebinnumber,
                                                    xAxis: 1,
                                                    yAxis: 1,
                                                    baseSeries: 's1',
                                                    zIndex: -1
                                                }, {
                                                    name: 'Data',
                                                    type: 'scatter',
                                                    data: data,
                                                    id: 's1',
                                                    visible: false,
                                                    showInLegend: false,
                                                    marker: {
                                                        radius: 1.5
                                                    }
                                                }]
                                            });
                                       
                                    break;
                                    
                                }
                            }
                        });

                        var lightbrakedata = 0;
                        var moderatebrakedata = 0;
                        var hardbrakedata = 0;
                        var fullbrakedata = 0;
                        var brakepeilist = [];
                        for (var i = 0; i < data.length; i++) {
                            if (data[i] < 0.2) {
                                lightbrakedata = lightbrakedata + 1;
                            }
                            else if (data[i] < 0.4) {
                                moderatebrakedata = moderatebrakedata + 1;
                            }
                            else if (data[i] < 0.6) {
                                hardbrakedata = hardbrakedata + 1;
                            }
                            else {
                                fullbrakedata = fullbrakedata + 1;
                            }
                        }
                        brakepeilist.push(["轻制动（0.2g）", lightbrakedata]);
                        brakepeilist.push(["中制动（0.4g）", moderatebrakedata]);
                        brakepeilist.push(["重制动（0.6g）", hardbrakedata]);
                        brakepeilist.push(["全制动（1.0g）", fullbrakedata]);

                        Highcharts.chart('brakepiecontainer', {
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
                                text: "制动强度占比图"
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
                                name: '制动强度',
                                colorByPoint: true,
                                data: eval(brakepeilist)
                            }],
                            exporting: {
                                width: 1000
                            }
                        });


                        layer.close(index);
                    }
                    else {
                        layer.close(index);
                    }
                   
                }
                , error: function (e) {
                    layer.msg(e);
                }

            });

           
            //冲击强度直方图
            $.ajax({

                type: "POST",
                //请求的媒体类型
                dataType: 'json',
                //请求地址
                url: urlgetbumpdata,
                data: {
                    startdate: startdate,

                    enddate: enddate,
                    vehicleid: a['id']
                },
                success: function (data) {
                    if (data.length!=0) {

                       var bumpchart= Highcharts.chart('bumphistogramcontainer', {

                            credits: {
                                enabled: false // 禁用版权信息
                            },
                            title: {
                                text: '冲击强度直方图',

                            },
                            subtitle: {
                                text: startdate + "to" + enddate + "一共有" + data.length + "个冲击"
                            },
                            plotOptions: {
                                histogram: {
                                    dataLabels: {
                                        // 开启数据标签
                                        enabled: true
                                    },
                                    // 关闭鼠标跟踪，对应的提示框、点击事件会失效
                                    enableMouseTracking: true
                                }
                            },
                            xAxis: [{
                                title: { text: '冲击强度（g）' }
                            }, {
                                title: { text: '冲击强度（g）' },
                                opposite: true
                            }],
                            yAxis: [{
                                title: { text: '频次' }
                            }, {
                                title: { text: '频次' },
                                opposite: true
                            }],

                            series: [{
                                name: '频次',
                                type: 'histogram',
                                binsNumber: 'sturges',
                                xAxis: 1,
                                yAxis: 1,
                                baseSeries: 's1',
                                zIndex: -1
                            }, {
                                name: 'Data',
                                type: 'scatter',
                                data: data,
                                id: 's1',
                                visible: false,
                                showInLegend: false,
                                marker: {
                                    radius: 1.5
                                }
                            }]

                        });

                        document.getElementById('bumpwrapper').removeAttribute('hidden');
                        Highcharts.addEvent(document.getElementById('bumpwrapper'), 'click', function (e) {
                            var bumpbinnumber = parseInt(document.getElementById('bumpbinnumber').value);
                            var target = e.target,
                                button = null;
                            if (target.tagName === 'BUTTON') { // 判断点的是否是 button
                                button = target.id;
                                switch (button) {
                                    case 'bumpplain':

                                        bumpchart.update({
                                            series: [{
                                                name: '频次',
                                                type: 'histogram',
                                                binsNumber: bumpbinnumber,
                                                xAxis: 1,
                                                yAxis: 1,
                                                baseSeries: 's1',
                                                zIndex: -1
                                            }, {
                                                name: 'Data',
                                                type: 'scatter',
                                                data: data,
                                                id: 's1',
                                                visible: false,
                                                showInLegend: false,
                                                marker: {
                                                    radius: 1.5
                                                }
                                            }]
                                        });

                                        break;

                                }
                            }
                        });

                        var lightbumpdata = 0;
                        var moderatebumpdata = 0;
                        var hardbumpdata = 0;
                        var fullbumpdata = 0;
                        var bumppeilist = [];
                        for (var i = 0; i < data.length; i++) {
                            if (data[i] < 5) {
                                lightbumpdata = lightbumpdata + 1;
                            }
                            else if (data[i] < 10) {
                                moderatebumpdata = moderatebumpdata + 1;
                            }
                            else if (data[i] < 15) {
                                hardbumpdata = hardbumpdata + 1;
                            }
                            else {
                                fullbumpdata = fullbumpdata + 1;
                            }
                        }
                        bumppeilist.push(["轻冲击-小卵石（5g）", lightbumpdata]);
                        bumppeilist.push(["中冲击-坑洼路（10g）", moderatebumpdata]);
                        bumppeilist.push(["重冲击-石块路（15g）", hardbumpdata]);
                        bumppeilist.push(["全冲击-方坑（20g）", fullbumpdata]);

                        Highcharts.chart('bumppiecontainer', {
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
                                text: "冲击强度占比图"
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
                                name: '冲击强度',
                                colorByPoint: true,
                                data: eval(bumppeilist)
                            }],
                            exporting: {
                                width: 1000
                            }
                        });

                        layer.close(index);
                    }
                    else {
                        layer.close(index);
                    }
                   
                }
                , error: function (e) {
                    layer.msg(e);
                }

            });

            //油门图表
            $.ajax({

                type: "POST",
                //请求的媒体类型
                dataType: 'json',
                //请求地址
                url: urlgetthrottledata,
                data: {
                    startdate: startdate,

                    enddate: enddate,
                    vehicleid: a['id']
                },
                success: function (data) {
                    if (data.length!=0) {
                        var throttlecount = data.length;
                        var accelerographlist = [];
                        var throttlelist = [];
                        var accelerographspeedlist = [];
                        var throttlespeedlist = [];
                        var reversecount = 0;

                        var light40num = 0;
                        var light70num = 0;
                        var light100num = 0;
                        var light140num = 0;

                        var moderate40num = 0;
                        var moderate70num = 0;
                        var moderate100num = 0;
                        var moderate140num = 0;

                        var hard40num = 0;
                        var hard70num = 0;
                        var hard100num = 0;
                        var hard140num = 0;

                        var full40num = 0;
                        var full70num = 0;
                        var full100num = 0;
                        var full140num = 0;
                        
                        var lightthrottle = [];
                        var moderatethrottle = [];
                        var hardthrottle = [];
                        var fullthrottle = [];


                        var throttlepie = [];
                      

                        for (var i = 0; i < data.length; i++) {
                            var item = data[i];
                            //这里只考虑前进方向，不考虑倒车
                            if (item["reverse"] == 0) {
                                accelerographlist.push(item["accelerograph"]);
                                throttlelist.push(item["throttleAcc"]);

                                accelerographspeedlist.push([item["speed"], item["accelerograph"]]);
                                throttlespeedlist.push([item["speed"], item["throttleAcc"]]);
                            }
                            else {
                                reversecount++;
                            }
                            
                        }
                        for (var i = 0; i < throttlespeedlist.length; i++) {
                            if (throttlespeedlist[i][0] < 40) {
                                if (throttlespeedlist[i][1] > -0.18) {
                                    light40num++;
                                }
                                else if (throttlespeedlist[i][1] > -0.25) {
                                    moderate40num++;
                                }
                                else if (throttlespeedlist[i][1] > -0.31) {
                                    hard40num++;
                                }
                                else {
                                    full40num++;
                                }
                            }
                            else if (throttlespeedlist[i][0] < 70) {
                                if (throttlespeedlist[i][1] > -0.11) {
                                    light70num++;
                                }
                                else if (throttlespeedlist[i][1] > -0.18) {
                                    moderate70num++;
                                }
                                else if (throttlespeedlist[i][1] > -0.26){
                                    hard70num++;
                                }
                                else {
                                    full70num++;
                                }

                            }
                            else if (throttlespeedlist[i][0] < 100) {
                                if (throttlespeedlist[i][1] > -0.06) {
                                    light100num++;
                                }
                                else if (throttlespeedlist[i][1] > -0.11) {
                                    moderate100num++;
                                }
                                else if (throttlespeedlist[i][1] > -0.19) {
                                    hard100num++;
                                }
                                else {
                                    full100num++;
                                }

                            }
                            else {
                                if (throttlespeedlist[i][1] > -0.04) {
                                    light140num++;
                                }
                                else if (throttlespeedlist[i][1] > -0.06) {
                                    moderate140num++;
                                }
                                else if (throttlespeedlist[i][1] > -0.09) {
                                    hard140num++;
                                }
                                else {
                                    full140num++;
                                }

                            }
                        }
                        lightthrottle.push(light40num);
                        lightthrottle.push(light70num);
                        lightthrottle.push(light100num);
                        lightthrottle.push(light140num);
                        moderatethrottle.push(moderate40num);
                        moderatethrottle.push(moderate70num);
                        moderatethrottle.push(moderate100num);
                        moderatethrottle.push(moderate140num);
                        hardthrottle.push(hard40num);
                        hardthrottle.push(hard70num);
                        hardthrottle.push(hard100num);
                        hardthrottle.push(hard140num);
                        fullthrottle.push(full40num);
                        fullthrottle.push(full70num);
                        fullthrottle.push(full100num);
                        fullthrottle.push(full140num);
                        
                        var lightnum = light40num + light70num + light100num + light140num;
                        var moderatenum = moderate40num + moderate70num + moderate100num + moderate140num;
                        var hardnum = hard40num + hard70num + hard100num + hard140num;
                        var fullnum = full40num + full70num + full100num + full140num;


                        throttlepie.push(["轻油门", lightnum]);
                        throttlepie.push(["中油门", moderatenum]);
                        throttlepie.push(["重油门", hardnum]);
                        throttlepie.push(["全油门", fullnum]);

                        var Accelerographchart = Highcharts.chart('Accelerographhistogramcontainer', {

                            credits: {
                                enabled: false // 禁用版权信息
                            },
                            title: {
                                text: '油门开度直方图',

                            },
                            subtitle: {
                                text: startdate + "to" + enddate + "一共有" + throttlecount + "油门次数。其中包括" + reversecount+"次倒车"
                            },
                            plotOptions: {
                                histogram: {
                                    dataLabels: {
                                        // 开启数据标签
                                        enabled: true
                                    },
                                    // 关闭鼠标跟踪，对应的提示框、点击事件会失效
                                    enableMouseTracking: true
                                }
                            },
                            xAxis: [{
                                title: { text: '油门开度（%）' }
                            }, {
                                title: { text: '油门开度（%）' },
                                opposite: true
                            }],
                            yAxis: [{
                                title: { text: '频次' }
                            }, {
                                title: { text: '频次' },
                                opposite: true
                            }],

                            series: [{
                                name: '频次',
                                type: 'histogram',
                                binsNumber: 'sturges',
                                xAxis: 1,
                                yAxis: 1,
                                baseSeries: 's1',
                                zIndex: -1
                            }, {
                                name: 'Data',
                                type: 'scatter',
                                data: accelerographlist,
                                id: 's1',
                                visible: false,
                                showInLegend: false,
                                marker: {
                                    radius: 1.5
                                }
                            }]

                        });

                        document.getElementById('Accelerographwrapper').removeAttribute('hidden');
                        Highcharts.addEvent(document.getElementById('Accelerographwrapper'), 'click', function (e) {
                            var Accelerographbinnumber = parseInt(document.getElementById('Accelerographbinnumber').value);
                            var target = e.target,
                                button = null;
                            if (target.tagName === 'BUTTON') { // 判断点的是否是 button
                                button = target.id;
                                switch (button) {
                                    case 'Accelerographplain':

                                        Accelerographchart.update({
                                            series: [{
                                                name: '频次',
                                                type: 'histogram',
                                                binsNumber: Accelerographbinnumber,
                                                xAxis: 1,
                                                yAxis: 1,
                                                baseSeries: 's1',
                                                zIndex: -1
                                            }, {
                                                name: 'Data',
                                                type: 'scatter',
                                                data: accelerographlist,
                                                id: 's1',
                                                visible: false,
                                                showInLegend: false,
                                                marker: {
                                                    radius: 1.5
                                                }
                                            }]
                                        });

                                        break;

                                }
                            }
                        });
                       
                        var Throttleaccchart = Highcharts.chart('Throttleacchistogramcontainer', {

                            credits: {
                                enabled: false // 禁用版权信息
                            },
                            title: {
                                text: '油门强度直方图',

                            },
                            subtitle: {
                                text: startdate + "to" + enddate + "一共有" + throttlecount + "油门次数"
                            },
                            plotOptions: {
                                histogram: {
                                    dataLabels: {
                                        // 开启数据标签
                                        enabled: true
                                    },
                                    // 关闭鼠标跟踪，对应的提示框、点击事件会失效
                                    enableMouseTracking: true
                                }
                            },
                            xAxis: [{
                                title: { text: '油门强度（g）' }
                            }, {
                                title: { text: '油门强度（g）' },
                                opposite: true
                            }],
                            yAxis: [{
                                title: { text: '频次' }
                            }, {
                                title: { text: '频次' },
                                opposite: true
                            }],

                            series: [{
                                name: '频次',
                                type: 'histogram',
                                binsNumber: 'sturges',
                                xAxis: 1,
                                yAxis: 1,
                                baseSeries: 's1',
                                zIndex: -1
                            }, {
                                name: 'Data',
                                type: 'scatter',
                                data: throttlelist,
                                id: 's1',
                                visible: false,
                                showInLegend: false,
                                marker: {
                                    radius: 1.5
                                }
                            }]

                        });

                        document.getElementById('Throttleaccwrapper').removeAttribute('hidden');
                        Highcharts.addEvent(document.getElementById('Throttleaccwrapper'), 'click', function (e) {
                            var Throttleaccbinnumber = parseInt(document.getElementById('Throttleaccbinnumber').value);
                            var target = e.target,
                                button = null;
                            if (target.tagName === 'BUTTON') { // 判断点的是否是 button
                                button = target.id;
                                switch (button) {
                                    case 'Throttleaccplain':

                                        Throttleaccchart.update({
                                            series: [{
                                                name: '频次',
                                                type: 'histogram',
                                                binsNumber: Throttleaccbinnumber,
                                                xAxis: 1,
                                                yAxis: 1,
                                                baseSeries: 's1',
                                                zIndex: -1
                                            }, {
                                                name: 'Data',
                                                type: 'scatter',
                                                data: throttlelist,
                                                id: 's1',
                                                visible: false,
                                                showInLegend: false,
                                                marker: {
                                                    radius: 1.5
                                                }
                                            }]
                                        });

                                        break;

                                }
                            }
                        })

                        var Accelerographspeedchart = Highcharts.chart('Accelerographspeedcontainer', {
                            chart: {
                                type: 'scatter',
                                zoomType: 'xy'
                            },
                            credits: {
                                enabled: false // 禁用版权信息
                            },
                            title: {
                                text: '速度与油门开度关系图'
                            },
                            subtitle: {
                                text: startdate + "to" + enddate
                            },
                            xAxis: {
                                title: {
                                    enabled: true,
                                    text: '速度（km/h）'
                                },
                                startOnTick: true,
                                endOnTick: true,
                                showLastLabel: true
                            },
                            yAxis: {
                                title: {
                                    text: '油门开度 (%)'
                                }
                            },
                           
                            plotOptions: {
                                scatter: {
                                    marker: {
                                        radius: 5,
                                        states: {
                                            hover: {
                                                enabled: true,
                                                lineColor: 'rgb(100,100,100)'
                                            }
                                        }
                                    },
                                    states: {
                                        hover: {
                                            marker: {
                                                enabled: false
                                            }
                                        }
                                    },
                                    tooltip: {
                                        headerFormat: '<b>{series.name}</b><br>',
                                        pointFormat: '{point.x} km/h, {point.y} %'
                                    }
                                }
                            },
                            series: [{
                                name: '速度-开度',
                                color: 'rgba(223, 83, 83, .5)',
                                data: accelerographspeedlist
                            }]
	
	
                        });

                        var Throttleaccspeedchart = Highcharts.chart('Throttleaccspeedcontainer', {
                            chart: {
                                type: 'scatter',
                                zoomType: 'xy'
                            },
                            credits: {
                                enabled: false // 禁用版权信息
                            },
                            title: {
                                text: '速度与油门强度关系图'
                            },
                            subtitle: {
                                text: startdate + "to" + enddate
                            },
                            xAxis: {
                                title: {
                                    enabled: true,
                                    text: '速度（km/h）'
                                },
                                startOnTick: true,
                                endOnTick: true,
                                showLastLabel: true
                            },
                            yAxis: {
                                title: {
                                    text: '油门强度 (g)'
                                }
                            },

                            plotOptions: {
                                scatter: {
                                    marker: {
                                        radius: 5,
                                        states: {
                                            hover: {
                                                enabled: true,
                                                lineColor: 'rgb(100,100,100)'
                                            }
                                        }
                                    },
                                    states: {
                                        hover: {
                                            marker: {
                                                enabled: false
                                            }
                                        }
                                    },
                                    tooltip: {
                                        headerFormat: '<b>{series.name}</b><br>',
                                        pointFormat: '{point.x} km/h, {point.y} g'
                                    }
                                }
                            },
                            series: [{
                                name: '速度-强度',
                                color: 'rgba(223, 83, 83, .5)',
                                data: throttlespeedlist
                            }]


                        });

                        var throttlecolumnchart = Highcharts.chart('Throttlecolumncontainer', {
                            title: {
                                text: '油门强度标准柱形图'
                            },
                            credits: {
                                enabled: false // 禁用版权信息
                            },
                            xAxis: {
                                title: {
                                    enabled: true,
                                    text: '速度（km/h）'
                                },
                                categories: ['0-40', ' 40-70', '70-100', '100以上']
                            },
                            yAxis: {
                                title: {
                                    text: '次数'
                                }
                            },
                            plotOptions: {
                                series: {
                                    stacking: 'normal'
                                }
                            },
                            
                            series: [{
                                type: 'column',
                                name: '轻油门',
                                data: lightthrottle
                            }, {
                                type: 'column',
                                name: '中油门',
                                data: moderatethrottle
                            }, {
                                type: 'column',
                                name: '重油门',
                                data: hardthrottle
                            }, {
                                type: 'column',
                                name: '全油门',
                                data: fullthrottle

                            
                            }]
                        });

                        Highcharts.chart('Throttlepiecontainer', {
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
                                text: "油门强度标准占比图"
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
                                name: '制动强度',
                                colorByPoint: true,
                                data: eval(throttlepie)
                            }],
                            exporting: {
                                width: 1000
                            }
                        });

                        layer.close(index);
                    }
                    else {
                        layer.close(index);
                    }

                }
                , error: function (e) {
                    layer.msg(e);
                }

            });


            //转向图表
            $.ajax({

                type: "POST",
                //请求的媒体类型
                dataType: 'json',
                //请求地址
                url: urlgetsteeringdata,
                data: {
                    startdate: startdate,

                    enddate: enddate,
                    vehicleid: a['id']
                },
                success: function (data) {
                    if (data.length!=0) {
                        var steeringcount = data.length;
                        var steeringanglist = [];
                        var steeringacclist = [];
                        var steeringangspeedlist = [];
                        var steeringaccspeedlist = [];
                        for (var i = 0; i < data.length; i++) {
                            var item = data[i];
                            steeringanglist.push(item["strgWhlAng"]);
                            steeringacclist.push(item["steeringAcc"]);

                            steeringangspeedlist.push([item["speed"], item["strgWhlAng"]]);
                            steeringaccspeedlist.push([item["speed"], item["steeringAcc"]])
                        }


                        var steeringangchart = Highcharts.chart('StrgWhlAnghistogramcontainer', {

                            credits: {
                                enabled: false // 禁用版权信息
                            },
                            title: {
                                text: '转向角直方图',

                            },
                            subtitle: {
                                text: startdate + "to" + enddate + "一共有" + steeringcount + "转向次数"
                            },
                            plotOptions: {
                                histogram: {
                                    dataLabels: {
                                        // 开启数据标签
                                        enabled: true
                                    },
                                    // 关闭鼠标跟踪，对应的提示框、点击事件会失效
                                    enableMouseTracking: true
                                }
                            },
                            xAxis: [{
                                title: { text: '转向角（°）' }
                            }, {
                                title: { text: '转向角（°）' },
                                opposite: true
                            }],
                            yAxis: [{
                                title: { text: '频次' }
                            }, {
                                title: { text: '频次' },
                                opposite: true
                            }],

                            series: [{
                                name: '频次',
                                type: 'histogram',
                                binsNumber: 'sturges',
                                xAxis: 1,
                                yAxis: 1,
                                baseSeries: 's1',
                                zIndex: -1
                            }, {
                                name: 'Data',
                                type: 'scatter',
                                data: steeringanglist,
                                id: 's1',
                                visible: false,
                                showInLegend: false,
                                marker: {
                                    radius: 1.5
                                }
                            }]

                        });

                        document.getElementById('StrgWhlAngwrapper').removeAttribute('hidden');
                        Highcharts.addEvent(document.getElementById('StrgWhlAngwrapper'), 'click', function (e) {
                            var StrgWhlAngbinnumber = parseInt(document.getElementById('StrgWhlAngbinnumber').value);
                            var target = e.target,
                                button = null;
                            if (target.tagName === 'BUTTON') { // 判断点的是否是 button
                                button = target.id;
                                switch (button) {
                                    case 'StrgWhlAngplain':

                                        steeringangchart.update({
                                            series: [{
                                                name: '频次',
                                                type: 'histogram',
                                                binsNumber: StrgWhlAngbinnumber,
                                                xAxis: 1,
                                                yAxis: 1,
                                                baseSeries: 's1',
                                                zIndex: -1
                                            }, {
                                                name: 'Data',
                                                type: 'scatter',
                                                data: steeringanglist,
                                                id: 's1',
                                                visible: false,
                                                showInLegend: false,
                                                marker: {
                                                    radius: 1.5
                                                }
                                            }]
                                        });

                                        break;

                                }
                            }
                        });

                        var Steeringaccchart = Highcharts.chart('SteeringAcchistogramcontainer', {

                            credits: {
                                enabled: false // 禁用版权信息
                            },
                            title: {
                                text: '转向强度直方图',

                            },
                            subtitle: {
                                text: startdate + "to" + enddate + "一共有" + steeringcount + "转向次数"
                            },
                            plotOptions: {
                                histogram: {
                                    dataLabels: {
                                        // 开启数据标签
                                        enabled: true
                                    },
                                    // 关闭鼠标跟踪，对应的提示框、点击事件会失效
                                    enableMouseTracking: true
                                }
                            },
                            xAxis: [{
                                title: { text: '转向强度（g）' }
                            }, {
                                title: { text: '转向强度（g）' },
                                opposite: true
                            }],
                            yAxis: [{
                                title: { text: '频次' }
                            }, {
                                title: { text: '频次' },
                                opposite: true
                            }],

                            series: [{
                                name: '频次',
                                type: 'histogram',
                                binsNumber: 'sturges',
                                xAxis: 1,
                                yAxis: 1,
                                baseSeries: 's1',
                                zIndex: -1
                            }, {
                                name: 'Data',
                                type: 'scatter',
                                data: steeringacclist,
                                id: 's1',
                                visible: false,
                                showInLegend: false,
                                marker: {
                                    radius: 1.5
                                }
                            }]

                        });

                        document.getElementById('SteeringAccwrapper').removeAttribute('hidden');
                        Highcharts.addEvent(document.getElementById('SteeringAccwrapper'), 'click', function (e) {
                            var SteeringAccbinnumber = parseInt(document.getElementById('SteeringAccbinnumber').value);
                            var target = e.target,
                                button = null;
                            if (target.tagName === 'BUTTON') { // 判断点的是否是 button
                                button = target.id;
                                switch (button) {
                                    case 'SteeringAccplain':

                                        Steeringaccchart.update({
                                            series: [{
                                                name: '频次',
                                                type: 'histogram',
                                                binsNumber: SteeringAccbinnumber,
                                                xAxis: 1,
                                                yAxis: 1,
                                                baseSeries: 's1',
                                                zIndex: -1
                                            }, {
                                                name: 'Data',
                                                type: 'scatter',
                                                data: steeringacclist,
                                                id: 's1',
                                                visible: false,
                                                showInLegend: false,
                                                marker: {
                                                    radius: 1.5
                                                }
                                            }]
                                        });

                                        break;

                                }
                            }
                        })

                        var steeringangspeedchart = Highcharts.chart('StrgWhlAngspeedcontainer', {
                            chart: {
                                type: 'scatter',
                                zoomType: 'xy'
                            },
                            credits: {
                                enabled: false // 禁用版权信息
                            },
                            title: {
                                text: '速度与转向角关系图'
                            },
                            subtitle: {
                                text: startdate + "to" + enddate
                            },
                            xAxis: {
                                title: {
                                    enabled: true,
                                    text: '速度（km/h）'
                                },
                                startOnTick: true,
                                endOnTick: true,
                                showLastLabel: true
                            },
                            yAxis: {
                                title: {
                                    text: '转向角 (°)'
                                }
                            },

                            plotOptions: {
                                scatter: {
                                    marker: {
                                        radius: 5,
                                        states: {
                                            hover: {
                                                enabled: true,
                                                lineColor: 'rgb(100,100,100)'
                                            }
                                        }
                                    },
                                    states: {
                                        hover: {
                                            marker: {
                                                enabled: false
                                            }
                                        }
                                    },
                                    tooltip: {
                                        headerFormat: '<b>{series.name}</b><br>',
                                        pointFormat: '{point.x} km/h, {point.y} °'
                                    }
                                }
                            },
                            series: [{
                                name: '速度-转角',
                                color: 'rgba(223, 83, 83, .5)',
                                data: steeringangspeedlist
                            }]


                        });

                        var Throttleaccspeedchart = Highcharts.chart('SteeringAccspeedcontainer', {
                            chart: {
                                type: 'scatter',
                                zoomType: 'xy'
                            },
                            credits: {
                                enabled: false // 禁用版权信息
                            },
                            title: {
                                text: '速度与转向强度关系图'
                            },
                            subtitle: {
                                text: startdate + "to" + enddate
                            },
                            xAxis: {
                                title: {
                                    enabled: true,
                                    text: '速度（km/h）'
                                },
                                startOnTick: true,
                                endOnTick: true,
                                showLastLabel: true
                            },
                            yAxis: {
                                title: {
                                    text: '转向强度 (g)'
                                }
                            },

                            plotOptions: {
                                scatter: {
                                    marker: {
                                        radius: 5,
                                        states: {
                                            hover: {
                                                enabled: true,
                                                lineColor: 'rgb(100,100,100)'
                                            }
                                        }
                                    },
                                    states: {
                                        hover: {
                                            marker: {
                                                enabled: false
                                            }
                                        }
                                    },
                                    tooltip: {
                                        headerFormat: '<b>{series.name}</b><br>',
                                        pointFormat: '{point.x} km/h, {point.y} g'
                                    }
                                }
                            },
                            series: [{
                                name: '速度-强度',
                                color: 'rgba(223, 83, 83, .5)',
                                data: steeringaccspeedlist
                            }]


                        });

                        //正向 右转
                        var lightsteeringdata = 0;
                        var moderatesteeringdata = 0;
                        var hardsteeringdata = 0;
                        var fullsteeringdata = 0;
                        var steeringpeilist = [];
                        //反向 左转
                        var lightsteeringdataneg = 0;
                        var moderatesteeringdataneg = 0;
                        var hardsteeringdataneg = 0;
                        var fullsteeringdataneg = 0;
                        var steeringpeilistneg = [];

                        for (var i = 0; i < steeringacclist.length; i++) {
                            if (steeringacclist[i] > 0) {
                                if (steeringacclist[i] < 0.2) {
                                    lightsteeringdata = lightsteeringdata + 1;
                                }
                                else if (steeringacclist[i] < 0.4) {
                                    moderatesteeringdata = moderatesteeringdata + 1;
                                }
                                else if (steeringacclist[i] < 0.6) {
                                    hardsteeringdata = hardsteeringdata + 1;
                                }
                                else {
                                    fullsteeringdata = fullsteeringdata + 1;
                                }
                            }
                            else {
                                if (steeringacclist[i] > -0.2) {
                                    lightsteeringdataneg = lightsteeringdataneg + 1;
                                }
                                else if (steeringacclist[i] > -0.4) {
                                    moderatesteeringdataneg = moderatesteeringdataneg + 1;
                                }
                                else if (steeringacclist[i] > -0.6) {
                                    hardsteeringdataneg = hardsteeringdataneg + 1;
                                }
                                else {
                                    fullsteeringdataneg = fullsteeringdataneg + 1;
                                }

                            }
                            }
                           
                        steeringpeilist.push(["轻转向（0.2g）", lightsteeringdata]);
                        steeringpeilist.push(["中转向（0.4g）", moderatesteeringdata]);
                        steeringpeilist.push(["重转向（0.6g）", hardsteeringdata]);
                        steeringpeilist.push(["全转向（1.0g）", fullsteeringdata]);

                        steeringpeilistneg.push(["轻转向（0.2g）", lightsteeringdataneg]);
                        steeringpeilistneg.push(["中转向（0.4g）", moderatesteeringdataneg]);
                        steeringpeilistneg.push(["重转向（0.6g）", hardsteeringdataneg]);
                        steeringpeilistneg.push(["全转向（1.0g）", fullsteeringdataneg]);

                        Highcharts.chart('steeringpiecontainer', {
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
                                text: "右转强度占比图"
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
                                name: '转向强度',
                                colorByPoint: true,
                                data: eval(steeringpeilist)
                            }],
                            exporting: {
                                width: 1000
                            }
                        });

                        Highcharts.chart('steeringpienegcontainer', {
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
                                text: "左转强度占比图"
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
                                name: '转向强度',
                                colorByPoint: true,
                                data: eval(steeringpeilistneg)
                            }],
                            exporting: {
                                width: 1000
                            }
                        });


                        layer.close(index);
                    }
                    else {
                        layer.close(index);
                    }

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