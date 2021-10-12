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

    
    element = layui.element;

    $("#startquartzjob").click(function () {

        $.ajax({
            type: "POST",
            //请求的媒体类型
            contentType: "application/json;charset=UTF-8",
            //请求地址 实时数据
            url: urlstartquartzjob,
            success: function (result) {

                layer.msg(result);
            }


        })


    });
    $("#stopquartzjob").click(function () {

        $.ajax({
            type: "POST",
            //请求的媒体类型
            contentType: "application/json;charset=UTF-8",
            //请求地址 实时数据
            url: urlstopquartzjob,
            success: function (result) {

                layer.msg(result);
            }


        })


    });
})