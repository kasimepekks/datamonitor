
layui.use(['element', 'layer', 'laydate','table', 'form'], function () {

    var date="";
    //import 'default-passive-events';

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
    var laydate = layui.laydate;
    laydate.render({
        elem: '#dateselect'
        , done: function (value) {
            date = value;
           
        }
    });

    
    
    $("#adddistance").click(function () {
       
        if (date != "") {
            var index = layer.load();

            $.ajax({

                type: "POST",
                //请求的媒体类型
                dataType: 'text',//这里改为json就不会传回success需要的数据了
                //请求地址
                url: urlgetdata,
                data: {
                    datetime: date
                },
                success: function (data) {
                    
                    layer.msg(data);
                    layer.close(index);
                }
                ,error: function () { }

            });
           
        }
        else {
            layer.msg("请先选择日期！！！");
        }
    });
});