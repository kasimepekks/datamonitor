
layui.use(['element', 'layer', 'table', 'form'], function () {
    var $ = layui.jquery;
    layer = layui.layer;
    element = layui.element;
   
    var distancetext = $("#distancetext");
    var damagetext = $("#damagetext");
    
    var connection = new signalR.HubConnectionBuilder().withUrl("/MyHub").build();
    connection.on("SpeedtoDistance", function (distance,speed,brake) {
        
        //console.log(speed);
        distancetext.html(distance.toFixed(2));
        var myChart = echarts.init(document.getElementById('speedchart'));
        var DisChart = echarts.init(document.getElementById('dischart'));
        var BrakeChart = echarts.init(document.getElementById('brakechart'));
        var speedoption,disoption,brakeoption;
        speedoption = {
            series: [{
                type: 'gauge',
                //radius: '55%',
                min: 0,
                max: 240,
                axisLine: {
                    lineStyle: {
                        width: 30,
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
                    fontSize: 15
                },
                detail: {
                    valueAnimation: true,
                    fontSize: 20,
                    formatter: '{value} km/h',
                    color: 'auto'
                },
                data: [{
                    value: speed[0]
                }]
            }]


            //series: [{
            //    type: 'gauge',
            //    startAngle: 180,
            //    endAngle: 0,
            //    min: 0,
            //    max: 240,
            //    splitNumber: 12,
            //    itemStyle: {
            //        color: '#58D9F9',
            //        shadowColor: 'rgba(0,138,255,0.45)',
            //        shadowBlur: 10,
            //        shadowOffsetX: 2,
            //        shadowOffsetY: 2
            //    },
            //    progress: {
            //        show: true,
            //        roundCap: true,
            //        width: 18
            //    },
            //    pointer: {
            //        icon: 'path://M2090.36389,615.30999 L2090.36389,615.30999 C2091.48372,615.30999 2092.40383,616.194028 2092.44859,617.312956 L2096.90698,728.755929 C2097.05155,732.369577 2094.2393,735.416212 2090.62566,735.56078 C2090.53845,735.564269 2090.45117,735.566014 2090.36389,735.566014 L2090.36389,735.566014 C2086.74736,735.566014 2083.81557,732.63423 2083.81557,729.017692 C2083.81557,728.930412 2083.81732,728.84314 2083.82081,728.755929 L2088.2792,617.312956 C2088.32396,616.194028 2089.24407,615.30999 2090.36389,615.30999 Z',
            //        length: '75%',
            //        width: 16,
            //        offsetCenter: [0, '5%']
            //    },
            //    axisLine: {
            //        roundCap: true,
            //        lineStyle: {
            //            width: 18
            //        }
            //    },
            //    axisTick: {
            //        splitNumber: 2,
            //        lineStyle: {
            //            width: 2,
            //            color: '#999'
            //        }
            //    },
            //    splitLine: {
            //        length: 12,
            //        lineStyle: {
            //            width: 3,
            //            color: '#999'
            //        }
            //    },
            //    axisLabel: {
            //        distance: 30,
            //        color: '#999',
            //        fontSize: 15
            //    },
            //    title: {
            //        show: false
            //    },
            //    detail: {
            //        backgroundColor: '#fff',
            //        borderColor: '#999',
            //        borderWidth: 0,
            //        width: '60%',
            //        lineHeight: 40,
            //        height: 40,
            //        borderRadius: 8,
            //        offsetCenter: [0, '35%'],
            //        valueAnimation: true,
            //        formatter: function (value) {
            //            return '{value|' + value.toFixed(0) + '}{unit|km/h}';
            //        },
            //        rich: {
            //            value: {
            //                fontSize: 50,
            //                fontWeight: 'bolder',
            //                color: '#777'
            //            },
            //            unit: {
            //                fontSize: 20,
            //                color: '#999',
            //                padding: [0, 0, -20, 10]
            //            }
            //        }
            //    },
            //    data: [{
            //        value: speed[0]
            //    }]
            //}]

        };
        disoption = {
            series: [{
                type: 'gauge',
                //radius: '55%',
                min: 0,
                max: 120000,
                axisLine: {
                    lineStyle: {
                        width: 30,
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
                    fontSize: 15
                },
                detail: {
                    valueAnimation: true,
                    fontSize: 20,
                    formatter: '{value} km',
                    color: 'auto'
                },
                data: [{
                    value: 0
                }]
            }]



            //series: [{
            //    type: 'gauge',
            //    startAngle: 180,
            //    endAngle: 0,
            //    min: 0,
            //    max: 120000,
            //    splitNumber: 12,
            //    itemStyle: {
            //        color: '#58D9F9',
            //        shadowColor: 'rgba(0,138,255,0.45)',
            //        shadowBlur: 10,
            //        shadowOffsetX: 2,
            //        shadowOffsetY: 2
            //    },
            //    progress: {
            //        show: true,
            //        roundCap: true,
            //        width: 18
            //    },
            //    pointer: {
            //        icon: 'path://M2090.36389,615.30999 L2090.36389,615.30999 C2091.48372,615.30999 2092.40383,616.194028 2092.44859,617.312956 L2096.90698,728.755929 C2097.05155,732.369577 2094.2393,735.416212 2090.62566,735.56078 C2090.53845,735.564269 2090.45117,735.566014 2090.36389,735.566014 L2090.36389,735.566014 C2086.74736,735.566014 2083.81557,732.63423 2083.81557,729.017692 C2083.81557,728.930412 2083.81732,728.84314 2083.82081,728.755929 L2088.2792,617.312956 C2088.32396,616.194028 2089.24407,615.30999 2090.36389,615.30999 Z',
            //        length: '75%',
            //        width: 16,
            //        offsetCenter: [0, '5%']
            //    },
            //    axisLine: {
            //        roundCap: true,
            //        lineStyle: {
            //            width: 19
            //        }
            //    },
            //    axisTick: {
            //        splitNumber: 2,
            //        lineStyle: {
            //            width: 2,
            //            color: '#999'
            //        }
            //    },
            //    splitLine: {
            //        length: 12,
            //        lineStyle: {
            //            width: 3,
            //            color: '#999'
            //        }
            //    },
            //    axisLabel: {
            //        distance: 30,
            //        color: '#999',
            //        fontSize: 10
            //    },
            //    title: {
            //        show: false
            //    },
            //    detail: {
            //        backgroundColor: '#fff',
            //        borderColor: '#999',
            //        borderWidth: 0,
            //        width: '60%',
            //        lineHeight: 40,
            //        height: 40,
            //        borderRadius: 8,
            //        offsetCenter: [0, '35%'],
            //        valueAnimation: true,
            //        formatter: function (value) {
            //            return '{value|' + value.toFixed(2) + '}{unit|km}';
            //        },
            //        rich: {
            //            value: {
            //                fontSize: 50,
            //                fontWeight: 'bolder',
            //                color: '#777'
            //            },
            //            unit: {
            //                fontSize: 20,
            //                color: '#999',
            //                padding: [0, 0, -20, 10]
            //            }
            //        }
            //    },
            //    data: [{
            //        value: 0
            //    }]
            //}]
        }
        brakeoption = {
            series: [{
                type: 'gauge',
                //radius: '55%',
                min: 0,
                max: 100,
                axisLine: {
                    lineStyle: {
                        width: 30,
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
                    fontSize: 15
                },
                detail: {
                    valueAnimation: true,
                    fontSize: 20,
                    formatter: 'Brake {value} %',
                    color: 'auto'
                },
                data: [{
                    value: 0
                }]
            }]
        }

        var i = 0;
       
        setInterval(function () {
            
           
            if (i < 10) {
                speedoption.series[0].data[0].value = speed[i];
                brakeoption.series[0].data[0].value = brake[i];
                i++;
                myChart.setOption(speedoption, true);
                BrakeChart.setOption(brakeoption, true);
            }
            else {
                speedoption.series[0].data[0].value = speed[9];
                brakeoption.series[0].data[0].value = brake[9];
                //myChart.setOption(speedoption, true);
                //BrakeChart.setOption(brakeoption, true);
            }
        }, 1000);
        disoption.series[0].data[0].value = distance.toFixed(2);
        DisChart.setOption(disoption, true);
        myChart.setOption(speedoption);
        BrakeChart.setOption(brakeoption, true);
    });

    connection.start().then(function () {
        layer.msg("已开始监视");
        //document.getElementById("StartUpload").disabled = false;
    }).catch(function (err) {
        return console.error(err.toString());
    });
    

  

    //var map = new BMap.Map("map");
    //var startPoint = { lon: 121.48, lat: 31.23 };
    //map.centerAndZoom(new BMap.Point(startPoint.lon, startPoint.lat), 15);
    //map.enableScrollWheelZoom(true);

    //var myIcon = new BMap.Icon("http://lbsyun.baidu.com/jsdemo/img/Mario.png", new BMap.Size(32, 70), { //小车图片
    //    //offset: new BMap.Size(0, -5),    //相当于CSS精灵
    //    imageOffset: new BMap.Size(0, 0)//图片的偏移量。为了是图片底部中心对准坐标点。
    //});
    //var carMk = new BMap.Marker(new BMap.Point(startPoint.lon, startPoint.lat), { icon: myIcon });
    //map.addOverlay(carMk);

    //function renderLastPoint(point) {
    //    // 实例化一个驾车导航用来生成路线
    //    var driving = new BMap.DrivingRoute(map);
    //    var sp = new BMap.Point(startPoint.lon, startPoint.lat);
    //    var ep = new BMap.Point(point.lon, point.lat);
    //    driving.search(sp, ep);
    //    //设置新的开始点
    //    startPoint = point;

    //    driving.setSearchCompleteCallback(function (res) {
    //        //console.info(res);
    //        if (driving.getStatus() == BMAP_STATUS_SUCCESS) {
    //            //获取两点之间的实际点组
    //            var plan = res.getPlan(0);
    //            var arrPois = [];
    //            for (var j = 0; j < plan.getNumRoutes(); j++) {
    //                var route = plan.getRoute(j);
    //                arrPois = arrPois.concat(route.getPath());
    //            }
    //            //把实际点加到地图上
    //            //根据点组的长度画线和画点
    //            drawMap(arrPois);
    //        }
    //    });
    //}

    //var t30 = 10 * 1000;

    //function drawMap(pointArr) {
    //    if (pointArr.length == 0) {
    //        return;
    //    }
    //    var t = t30;//30秒
    //    //计算每次执行的时间
    //    var at = t / pointArr.length;
    //    var i = 0;

    //    var f = function () {
    //        if ((i + 1) > (pointArr.length - 1)) {
    //            return;
    //        }
    //        var sp = pointArr[i];
    //        var ep = pointArr[i + 1];

    //        //地图画线
    //        var polyline = new BMap.Polyline([sp, ep], { strokeColor: "blue", strokeWeight: 2, strokeOpacity: 0.5 });//创建折线
    //        map.addOverlay(polyline);
    //        //移动点
    //        carMk.setPosition(ep);
    //        var bound = map.getBounds();//地图可视区域
    //        if (bound.containsPoint(ep) == false) {
    //            map.panTo(ep);
    //        }

    //        i++;
    //        setTimeout(function () {
    //            f();
    //        }, at);
    //    };

    //    f();

    //}

    ////模拟业务
    //var ii = 0;
    //var _task = setInterval(function () {
    //    var lastPoint = { lon: 116.424374 + ii * 0.01, lat: 39.914668 };//终点
    //    if (lastPoint.lon == startPoint.lon && lastPoint.lat == startPoint.lat) {
    //        //相同点，则不需要画图
    //        return;
    //    }
    //    ii++;

    //    renderLastPoint(lastPoint);

    //}, t30);
  

    //var map = new BMap.Map("map");
    //var point = new BMap.Point(108.95, 34.27);
    //map.centerAndZoom(point, 12);
    //map.enableScrollWheelZoom();
    //var geolocation = new BMap.Geolocation();
    //geolocation.getCurrentPosition(function (r) {
    //    console.log(r.point)
    //    if (this.getStatus() == BMAP_STATUS_SUCCESS) {
    //        var mk = new BMap.Marker(r.point);
    //        map.addOverlay(mk);//标出所在地
    //        map.panTo(r.point);//地图中心移动
    //        //alert('您的位置：'+r.point.lng+','+r.point.lat);
    //        var point = new BMap.Point(r.point.lng, r.point.lat);//用所定位的经纬度查找所在地省市街道等信息
    //        var gc = new BMap.Geocoder();
    //        gc.getLocation(point, function (rs) {
    //            var addComp = rs.addressComponents; console.log(rs.address);//地址信息
    //            //alert(rs.address);//弹出所在地址

    //        });
    //    } else {
    //        alert('failed' + this.getStatus());
    //    }
    //}, { enableHighAccuracy: true })
});