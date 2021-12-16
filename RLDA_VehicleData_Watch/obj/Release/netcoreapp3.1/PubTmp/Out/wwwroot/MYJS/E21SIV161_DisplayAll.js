var map;
var lushu;
var routpath=[];
var polyline;
layui.use(['element', 'layer', 'table', 'form'], function () {
    var $ = layui.jquery;
    layer = layui.layer;
    element = layui.element;
    var connection;
    $.ajax({

        type: "POST",
        //请求的媒体类型
        dataType: 'text',//这里改为json就不会传回success需要的数据了
        //请求地址
        url: urlfilewatcher,
        data: {
            _vehicleID: "E21SIV161"
        },

    });



    //var hideorshow = false;
    //$(".third-class").on('click', function () {
    //    //$(".layui-nav-third-child").hide();
    //    if (!hideorshow) {
    //        $(this).next().show();
    //        hideorshow = true;
    //    }
    //    else {
    //        $(this).next().hide();
    //        hideorshow = false;
    //    }
    //});

    //var form = layui.form;
    //var table = layui.table;
    
    //var n = 0;
    //var id = "";

    //$(".layui-table-box").hide();
    //form.on('switch(tabledisplay)', function (data) {

    //    if (data.elem.checked) {
    //        $(".layui-table-box").show();

    //    }
    //    else {
    //        $(".layui-table-box").hide()
    //    };

    //});

    if (navigator.onLine) {



        var myIcon = new BMap.Icon('/Pictures/car.png',
            new BMap.Size(52, 26), {
            anchor: new BMap.Size(27, 13)
        });
    }

    connection = new signalR.HubConnectionBuilder().withUrl("/MyHub").build();
    connection.serverTimeoutInMilliseconds = 30000;
    connection.keepAliveIntervalInMilliseconds = 15000;

    connection.on("SpeedtoDistance", function (_vehicleID, distance, speed, brake, Lat, Lon, zerotime) {
        var number = speed.length;
        //判断服务器传过来的是哪辆车就显示哪辆车的信息，因为每辆车的数据源不一样
        if (_vehicleID == "E21SIV161") {
            $("#distance").text(distance.toFixed(2));
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
                        value: speed[0].toFixed(0)
                    }]
                }]

            };

            setInterval(function () {


                if (zerotime < number) {
                    speedoption.series[0].data[0].value = speed[zerotime].toFixed(0);
                                      
                    zerotime++;
                    myChart.setOption(speedoption, true);
                   
                }
                else {
                    speedoption.series[0].data[0].value = speed[number - 1].toFixed(0);
                  
                    myChart.setOption(speedoption, true);
                   
                }
            }, 1000);

            if (navigator.onLine) {

               /* map.clearOverlays(polyline);*/
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
                   
                    routpath=routpath.concat(testPoint);
                    if (lushu == null) {
                        lushu = new BMapLib.LuShu(map, routpath, {
                            defaultContent: "E21SIV161",
                            autoView: true, //是否开启自动视野调整，如果开启那么路书在运动过程中会根据视野自动调整
                            icon: myIcon,
                            enableRotation: true, //是否设置marker随着道路的走向进行旋转
                            speed: 2000,
                            landmarkPois: []
                        });
                        lushu.start();
                    }

                    else {
                        lushu.goPath(testPoint)
                    }
                  

                }

                BMap.Convertor.transMore(allPoint, 0, callback);


            }
        }

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

    connection.onclose(async () => {
        await  start();
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


    //});
});