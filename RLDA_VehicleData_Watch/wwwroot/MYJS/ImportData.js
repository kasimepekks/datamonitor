//获取a标签中传过来的id值
function GetRequest() {
    var url = location.search; //获取url中"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = decodeURIComponent(strs[i].split("=")[1]);
        }
    }
    return theRequest;
}
function getDate(dates) {
    var dd = new Date();
    dd.setDate(dd.getDate() + dates);
    var y = dd.getFullYear();
    var m = dd.getMonth() + 1;
    var d = dd.getDate();

    return y + "-" + m + "-" + d;
}

//获得最新能够导入数据的日期
layui.use(['element', 'layer', 'laydate', 'form'], function () {
        
    var datevalue, startdate, enddate,finisheddate;

    var a = GetRequest();
    //console.log("id:" + a['id']);
    var laydate = layui.laydate;
    var $ = layui.jquery;
    layer = layui.layer;

    $.ajax({

        type: "POST",
        //请求的媒体类型
        dataType: 'text',//这里改为json就不会传回success需要的数据了
        //请求地址
        url: urlgetfinisheddate,
        data: {
           vehicleid: a['id']
        },
        success: function (data) {

            finisheddate = data;
            document.getElementById('finisheddate').innerHTML = data;
            console.log(data + "and" + getDate(-1));
            laydate.render({
                elem: '#startend'
                , type: 'datetime'
                
               /* , min: data.replace(/\//g, '-')*/
                , max: getDate(-1)
                , range: '到'
                , format: 'yyyy-M-dd'
                , done: function (val, date, endDate) {
                    datevalue = val;
                    var startenddate = val.split("到");
                    startdate = startenddate[0];

                    enddate = startenddate[1];

                }


            });


           
        }
        , error: function () { }

    });

    
    $("#AnalysisandImport").click(function () {
       
        if (datevalue != undefined) {
            var index = layer.load();

            $.ajax({

                type: "POST",
                //请求的媒体类型
                dataType: 'text',//这里改为json就不会传回success需要的数据了
                //请求地址
                url: urlimportdata,
                data: {
                    startdate: startdate,

                    enddate: enddate,

                    vehicleid: a['id']
                },
                success: function (data) {
                    layer.open({
                        title: '数据计算导入操作'
                        , content: data
                    });
                    /*layer.msg(data);*/
                    layer.close(index);
                }
                ,error: function () { }

            });
           
        }
        else {
            layer.msg("请先选择日期！！！");
        }
    });

    $("#delete").click(function () {

        if (datevalue != undefined) {
            var index = layer.load();

            $.ajax({

                type: "POST",
                //请求的媒体类型
                dataType: 'text',//这里改为json就不会传回success需要的数据了
                //请求地址
                url: urlimportdata,
                data: {
                    startdate: startdate,

                    enddate: enddate,

                    vehicleid: a['id']
                },
                success: function (data) {
                    layer.open({
                        title: '数据计算导入操作'
                        , content: data
                    });
                    /*layer.msg(data);*/
                    layer.close(index);
                }
                , error: function () { }

            });

        }
        else {
            layer.msg("请先选择日期！！！");
        }
    });


});