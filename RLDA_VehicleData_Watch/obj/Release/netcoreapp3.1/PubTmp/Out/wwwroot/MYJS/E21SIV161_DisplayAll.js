var fileinfo = [];
var map;
var lushu;
var routpath=[];
var polyline;
layui.use(['element', 'layer', 'table', 'form'], function () {
    var $ = layui.jquery;
    layer = layui.layer;

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
    //var n = 0;
    //var id = "";

    $(".layui-table-box").hide();
    form.on('switch(tabledisplay)', function (data) {

        if (data.elem.checked) {
            $(".layui-table-box").show();

        }
        else {
            $(".layui-table-box").hide()
        };

    });

    if (navigator.onLine) {


        map = new BMap.Map("allmap");

        var startpoint = new BMap.Point(121.472644, 31.231706);
        map.centerAndZoom(startpoint, 17);
        map.enableScrollWheelZoom();
        var myIcon = new BMap.Icon('/Pictures/car.png',
            new BMap.Size(52, 26), {
            anchor: new BMap.Size(27, 13)
        });
    }

    var connection = new signalR.HubConnectionBuilder().withUrl("/MyHub").build();

    connection.on("SpeedtoDistance", function (_vehicleID, distance, speed, brake, Lat, Lon, zerotime) {
        //判断服务器传过来的是哪辆车就显示哪辆车的信息，因为每辆车的数据源不一样
        if (_vehicleID == "E21SIV161") {
            $("#distance").text(distance.toFixed(2));
           


            if (navigator.onLine) {

               /* map.clearOverlays(polyline);*/
                var allPoint = [];
                var testPoint = [];
                for (var i = 0; i < Lat.length; i++) {
                    allPoint.push(new BMap.Point(Lon[i], Lat[i]));

                }
                //allPoint.push(new BMap.Point(Lon[0], Lat[0]));
                //allPoint.push(new BMap.Point(Lon[Lon.length-1], Lat[Lat.length-1]));
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
                    //if (lushu) {
                    //    lushu.pause();
                    //}
                    routpath=routpath.concat(testPoint);
                    if (lushu == null) {
                        lushu = new BMapLib.LuShu(map, routpath, {
                            defaultContent: "E21SIV161",
                            autoView: true, //是否开启自动视野调整，如果开启那么路书在运动过程中会根据视野自动调整
                            icon: myIcon,
                            enableRotation: true, //是否设置marker随着道路的走向进行旋转
                            speed: 20,
                            landmarkPois: []
                        });
                        lushu.start();
                    }

                    else {
                        lushu.goPath(testPoint)
                    }

                    


                    //var marker = new BMap.Marker(point);
                    //map.addOverlay(marker);

                }

                BMap.Convertor.transMore(allPoint, 0, callback);


            }
        }

    });

    connection.start().then(function () {
        layer.msg("已开始监视");
        //document.getElementById("StartUpload").disabled = false;
    }).catch(function (err) {
        return console.error(err.toString());
    });


});