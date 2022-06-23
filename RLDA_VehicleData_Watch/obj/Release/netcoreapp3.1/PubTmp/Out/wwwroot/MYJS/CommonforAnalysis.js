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
document.write('<script src="../../Highcharts-8.2.2/code/highcharts.js"></script>');
document.write('<script src="../../Highcharts-8.2.2/code/highcharts-more.js"></script>');
document.write('<script src="../../Highcharts-8.2.2/code/modules/data.js"></script>');
document.write('<script src="../../Highcharts-8.2.2/code/modules/exporting.js"></script>');
document.write('<script src="../../Highcharts-8.2.2/code/modules/export-data.js"></script>');
document.write('<script src="../../lib/echarts/echarts.min.js"></script>');
document.write('<script src="../../lib/echarts/ecStat.min.js"></script>');


document.write('<script src="../../Highcharts-8.2.2/code/modules/histogram-bellcurve.js"></script>');
//document.write("<script> var urlloginfo = '/PopUp/LogPopUp' </script >");
document.write('<script type="text/javascript" src="//api.map.baidu.com/api?v=2.0&ak=FFvnSXHMHaC0CQgTguKY17zkEwwfqjsP"></script>');
document.write('<script src="../../MYJS/GPSConverttoBaidu.js"></script>');
document.write('<script src="../../MYJS/LuShu_min.js"></script>');

var urlgetdatetime = '/Analysis/SpeedAnalysis' ;
var urlgetdatetimeperday = '/Analysis/SpeedPerDayAnalysis' ;
var urlgetdatetimeperhour = '/Analysis/SpeedPerHourAnalysis' ;
var urlgetWFTdamage = '/Analysis/WFTDamageAnalysis';

var urlgetbrakedata = '/Analysis/BrakeDistributionAnalysis' ;
var urlgetbrakecount = '/Analysis/BrakeCountAnalysis';

var urlgetbumpdata = '/Analysis/BumpDistributionAnalysis';
var urlgetbumpcount = '/Analysis/BumpCountAnalysis';

var urlgetthrottledata = '/Analysis/ThrottleAnalysis';

var urlgetsteeringdata = '/Analysis/SteeringAnalysis';

var urlgetgpsdata = '/Analysis/GPSAnalysis';

var urlgetaccandisdata = '/Analysis/ACCandDisAnalysis';

var urlgettextdata = '/Analysis/TextperDayAnalysis';