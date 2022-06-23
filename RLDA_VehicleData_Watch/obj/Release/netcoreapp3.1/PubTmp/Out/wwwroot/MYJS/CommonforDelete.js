/// <reference path="../highcharts-8.2.2/code/modules/export-data.js" />

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


document.write('<link href="../../layui/css/layui.css" rel="stylesheet" />');
document.write('<script src="../../layui/layui.js"></script>');

var urldeletedata = '/Delete/DataSingleDelete';

var urldeletealldata = '/Delete/AllDataDelete';