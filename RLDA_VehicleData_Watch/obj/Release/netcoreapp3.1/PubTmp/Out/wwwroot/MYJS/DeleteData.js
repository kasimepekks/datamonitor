

//获得最新能够导入数据的日期
layui.use(['element', 'layer', 'laydate', 'form'], function () {
    var form = layui.form;
    var datevalue, startdate, enddate,selectvalue;
    console.log(selectvalue);
    var a = GetRequest();
    //console.log("id:" + a['id']);
    var laydate = layui.laydate;
    var $ = layui.jquery;
    layer = layui.layer;
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

    form.on('select(sqlform)', function (data) {
        selectvalue = data.value;
        console.log(data.value); //得到被选中的值
       
    });      
    
    $("#delete").click(function () {

        if (datevalue != undefined && selectvalue != undefined) {
            
            layer.open({
                content: '请再次确认是否进行删除操作',
                btn: ['确认', '取消'],
                yes: function (index, layero) {
                    //do something
                    var loadindex = layer.load();
                    $.ajax({

                        type: "POST",
                        //请求的媒体类型
                        dataType: 'text',//这里改为json就不会传回success需要的数据了
                        //请求地址
                        url: urldeletedata,
                        data: {
                            startdate: startdate,

                            enddate: enddate,

                            vehicleid: a['id'],
                            formvalue: selectvalue
                        },
                        success: function (data) {
                            layer.open({
                                title: '数据删除操作'
                                , content: data
                            });
                         
                        }
                        , error: function () { }

                    });
                    layer.close(index); //如果设定了yes回调，需进行手工关闭
                    layer.close(loadindex);
                }
            });
            
            
           
        }
        else {
            layer.msg("请先选择日期和数据表！！！");
        }
    });

    $("#deleteall").click(function () {

        if (datevalue != undefined) {

            layer.open({
                content: '请再次确认是否进行删除操作',
                btn: ['确认', '取消'],
                yes: function (index, layero) {
                    //do something
                    var loadindex = layer.load();
                    $.ajax({

                        type: "POST",
                        //请求的媒体类型
                        dataType: 'text',//这里改为json就不会传回success需要的数据了
                        //请求地址
                        url: urldeletealldata,
                        data: {
                            startdate: startdate,

                            enddate: enddate,

                            vehicleid: a['id'],
                            
                        },
                        success: function (data) {
                            layer.open({
                                title: '数据删除操作'
                                , content: data
                            });

                        }
                        , error: function () { }

                    });
                    layer.close(index); //如果设定了yes回调，需进行手工关闭
                    layer.close(loadindex);
                }
            });
        }
    });


});