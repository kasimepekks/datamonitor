
layui.use(['element', 'layer', 'laydate', 'table', 'form'], function () {
    var $ = layui.jquery;
    layer = layui.layer;
    var table = layui.table;
    var element = layui.element;
    var laydate = layui.laydate;
    var a = GetRequest();
    var datevalue, startdate, enddate;
  
    var LFXGDDamage = 467364;
    var RFXGDDamage = 529439;
    var LRXGDDamage = 328738;
    var RRXGDDamage = 397234;
    var LFYGDDamage = 178701;
    var RFYGDDamage = 227184;
    var LRYGDDamage = 37014;
    var RRYGDDamage = 47765;
    var LFZGDDamage = 4617056;
    var RFZGDDamage = 4618563;
    var LRZGDDamage = 3859613;
    var RRZGDDamage = 3551023;

  
    laydate.render({
        elem: '#startend'
        , type: 'datetime'
        , range: '到'
        , format: 'yyyy-M-d'
        , done: function (val, date, endDate) {
            datevalue = val;
            var startenddate = val.split("到");
            startdate = startenddate[0];

            enddate = startenddate[1];
            
        }


    });
    $("#damageprediction").click(function () {

        if (datevalue != undefined) {
            var index = layer.load();

            $.ajax({

                type: "POST",
                //请求的媒体类型
                dataType: 'json',
                //请求地址
                url: urlgetwftcumulation,
                data: {
                    startdate: startdate,

                    enddate: enddate,
                    vehicleid: a['id']
                },
                success: function (data) {
                    if (data != "No") {
                        //原始从后端发过来的没有叠加过的损伤值
                        var LFXdamage = [];
                        var RFXdamage = [];
                        var LRXdamage = [];
                        var RRXdamage = [];
                        var LFYdamage = [];
                        var RFYdamage = [];
                        var LRYdamage = [];
                        var RRYdamage = [];
                        var LFZdamage = [];
                        var RFZdamage = [];
                        var LRZdamage = [];
                        var RRZdamage = [];
                        //以下为叠加过的损伤值
                        var LFXdamagecum = [];
                        var RFXdamagecum = [];
                        var LRXdamagecum = [];
                        var RRXdamagecum = [];
                        var LFYdamagecum = [];
                        var RFYdamagecum = [];
                        var LRYdamagecum = [];
                        var RRYdamagecum = [];
                        var LFZdamagecum = [];
                        var RFZdamagecum = [];
                        var LRZdamagecum = [];
                        var RRZdamagecum = [];
                        //整数与叠加的损伤组成的数据
                        var LFXdateanddamagecum = [];
                        var RFXdateanddamagecum = [];
                        var LRXdateanddamagecum = [];
                        var RRXdateanddamagecum = [];
                        var LFYdateanddamagecum = [];
                        var RFYdateanddamagecum = [];
                        var LRYdateanddamagecum = [];
                        var RRYdateanddamagecum = [];
                        var LFZdateanddamagecum = [];
                        var RFZdateanddamagecum = [];
                        var LRZdateanddamagecum = [];
                        var RRZdateanddamagecum = [];
                        //var LFXrealdateanddamagecum = [];
                        var LFXdatetime = [];//真实日期数据

                        var RFXdatetime = [];
                        var LRXdatetime = [];
                        var RRXdatetime = [];

                        var LFYdatetime = [];
                        var RFYdatetime = [];
                        var LRYdatetime = [];
                        var RRYdatetime = [];

                        var LFZdatetime = [];
                        var RFZdatetime = [];
                        var LRZdatetime = [];
                        var RRZdatetime = [];
                        var datenumber = [];//日期转为整数值，从1开始，目的是为了能够拟合直线
                        for (var i = 0; i < data.length; i++) {
                            var item = data[i];
                            if (item["chantitle"] == "WFTFxLF") {
                                LFXdamage.push(item["damage"]);
                                LFXdatetime.push(item["datetime"]);
                                RFXdatetime.push(item["datetime"]);
                                LRXdatetime.push(item["datetime"]);
                                RRXdatetime.push(item["datetime"]);
                                LFYdatetime.push(item["datetime"]);
                                RFYdatetime.push(item["datetime"]);
                                LRYdatetime.push(item["datetime"]);
                                RRYdatetime.push(item["datetime"]);
                                LFZdatetime.push(item["datetime"]);
                                RFZdatetime.push(item["datetime"]);
                                LRZdatetime.push(item["datetime"]);
                                RRZdatetime.push(item["datetime"]);
                            }
                            if (item["chantitle"] == "WFTFxRF") {
                                RFXdamage.push(item["damage"])
                            }
                            if (item["chantitle"] == "WFTFxLR") {
                                LRXdamage.push(item["damage"])
                            }
                            if (item["chantitle"] == "WFTFxRR") {
                                RRXdamage.push(item["damage"])
                            }
                            if (item["chantitle"] == "WFTFyLF") {
                                LFYdamage.push(item["damage"]);

                            }
                            if (item["chantitle"] == "WFTFyRF") {
                                RFYdamage.push(item["damage"])
                            }
                            if (item["chantitle"] == "WFTFyLR") {
                                LRYdamage.push(item["damage"])
                            }
                            if (item["chantitle"] == "WFTFyRR") {
                                RRYdamage.push(item["damage"])
                            }
                            if (item["chantitle"] == "WFTFzLF") {
                                LFZdamage.push(item["damage"]);

                            }
                            if (item["chantitle"] == "WFTFzRF") {
                                RFZdamage.push(item["damage"])
                            }
                            if (item["chantitle"] == "WFTFzLR") {
                                LRZdamage.push(item["damage"])
                            }
                            if (item["chantitle"] == "WFTFzRR") {
                                RRZdamage.push(item["damage"])
                            }
                        }
                        //叠加损伤操作
                        for (var i = 0; i < LFXdamage.length; i++) {
                            var LFX = 0, LFY = 0, LFZ = 0, RFX = 0, RFY = 0, RFZ = 0, LRX = 0, LRY = 0, LRZ = 0, RRX = 0, RRY = 0, RRZ = 0;

                            for (var j = 0; j <= i; j++) {
                                LFX = LFX + LFXdamage[j];
                                LFY = LFY + LFYdamage[j];
                                LFZ = LFZ + LFZdamage[j];
                                RFX = RFX + RFXdamage[j];
                                RFY = RFY + RFYdamage[j];
                                RFZ = RFZ + RFZdamage[j];
                                LRX = LRX + LRXdamage[j];
                                LRY = LRY + LRYdamage[j];
                                LRZ = LRZ + LRZdamage[j];
                                RRX = RRX + RRXdamage[j];
                                RRY = RRY + RRYdamage[j];
                                RRZ = RRZ + RRZdamage[j];
                            }
                            LFXdamagecum.push(LFX);
                            LFYdamagecum.push(LFY);
                            LFZdamagecum.push(LFZ);
                            RFXdamagecum.push(RFX);
                            RFYdamagecum.push(RFY);
                            RFZdamagecum.push(RFZ);
                            LRXdamagecum.push(LRX);
                            LRYdamagecum.push(LRY);
                            LRZdamagecum.push(LRZ);
                            RRXdamagecum.push(RRX);
                            RRYdamagecum.push(RRY);
                            RRZdamagecum.push(RRZ);
                            datenumber.push(i + 1);
                        }
                        //天数和损伤的点数集合，用于线性拟合
                        for (var i = 0; i < LFXdamage.length; i++) {
                            LFXdateanddamagecum.push([datenumber[i], LFXdamagecum[i]]);
                            RFXdateanddamagecum.push([datenumber[i], RFXdamagecum[i]]);
                            LRXdateanddamagecum.push([datenumber[i], LRXdamagecum[i]]);
                            RRXdateanddamagecum.push([datenumber[i], RRXdamagecum[i]]);
                            
                            LFYdateanddamagecum.push([datenumber[i], LFYdamagecum[i]]);
                            RFYdateanddamagecum.push([datenumber[i], RFYdamagecum[i]]);
                            LRYdateanddamagecum.push([datenumber[i], LRYdamagecum[i]]);
                            RRYdateanddamagecum.push([datenumber[i], RRYdamagecum[i]]);
                            LFZdateanddamagecum.push([datenumber[i], LFZdamagecum[i]]);
                            RFZdateanddamagecum.push([datenumber[i], RFZdamagecum[i]]);
                            LRZdateanddamagecum.push([datenumber[i], LRZdamagecum[i]]);
                            RRZdateanddamagecum.push([datenumber[i], RRZdamagecum[i]]);
                        }


                        var LFXChart = echarts.init(document.getElementById('LFXcontainer'));
                        var RFXChart = echarts.init(document.getElementById('RFXcontainer'));
                        var LRXChart = echarts.init(document.getElementById('LRXcontainer'));
                        var RRXChart = echarts.init(document.getElementById('RRXcontainer'));
                        var LFYChart = echarts.init(document.getElementById('LFYcontainer'));
                        var RFYChart = echarts.init(document.getElementById('RFYcontainer'));
                        var LRYChart = echarts.init(document.getElementById('LRYcontainer'));
                        var RRYChart = echarts.init(document.getElementById('RRYcontainer'));
                        var LFZChart = echarts.init(document.getElementById('LFZcontainer'));
                        var RFZChart = echarts.init(document.getElementById('RFZcontainer'));
                        var LRZChart = echarts.init(document.getElementById('LRZcontainer'));
                        var RRZChart = echarts.init(document.getElementById('RRZcontainer'));
                        var allpredictionChart = echarts.init(document.getElementById('Predictioncontainer'));
                        var allpredictionresidueChart = echarts.init(document.getElementById('residuecontainer'));

                        var LFXoption;
                        var RFXoption;
                        var LRXoption;
                        var RRXoption;
                        var LFYoption;
                        var RFYoption;
                        var LRYoption;
                        var RRYoption;
                        var LFZoption;
                        var RFZoption;
                        var LRZoption;
                        var RRZoption;
                        var allpredictiondayoption;
                        var allpredictionresidueoption;
                        //叠加后的损伤
                        var WFTLFXdata = LFXdateanddamagecum;
                        var WFTRFXdata = RFXdateanddamagecum;
                        var WFTLRXdata = LRXdateanddamagecum;
                        var WFTRRXdata = RRXdateanddamagecum;
                        var WFTLFYdata = LFYdateanddamagecum;
                        var WFTRFYdata = RFYdateanddamagecum;
                        var WFTLRYdata = LRYdateanddamagecum;
                        var WFTRRYdata = RRYdateanddamagecum;
                        var WFTLFZdata = LFZdateanddamagecum;
                        var WFTRFZdata = RFZdateanddamagecum;
                        var WFTLRZdata = LRZdateanddamagecum;
                        var WFTRRZdata = RRZdateanddamagecum;

                        //剩余损伤百分比
                        var wftLFXresidue = ((LFXGDDamage - LFX) / LFXGDDamage * 100).toFixed(2);
                        var wftRFXresidue = ((RFXGDDamage - RFX) / RFXGDDamage * 100).toFixed(2);
                        var wftLRXresidue = ((LRXGDDamage - LRX) / LRXGDDamage * 100).toFixed(2);
                        var wftRRXresidue = ((RRXGDDamage - RRX) / RRXGDDamage * 100).toFixed(2);
                        
                        var wftLFYresidue = ((LFYGDDamage - LFY) / LFYGDDamage * 100).toFixed(2);
                        var wftRFYresidue = ((RFYGDDamage - RFY) / RFYGDDamage * 100).toFixed(2);
                        var wftLRYresidue = ((LRYGDDamage - LRY) / LRYGDDamage * 100).toFixed(2);
                        var wftRRYresidue = ((RRYGDDamage - RRY) / RRYGDDamage * 100).toFixed(2);

                        var wftLFZresidue = ((LFZGDDamage - LFZ) / LFZGDDamage * 100).toFixed(2);
                        var wftRFZresidue = ((RFZGDDamage - RFZ) / RFZGDDamage * 100).toFixed(2);
                        var wftLRZresidue = ((LRZGDDamage - LRZ) / LRZGDDamage * 100).toFixed(2);
                        var wftRRZresidue = ((RRZGDDamage - RRZ) / RRZGDDamage * 100).toFixed(2);

                        var LFXRegression = ecStat.regression('linear', WFTLFXdata);
                        var RFXRegression = ecStat.regression('linear', WFTRFXdata);
                        var LRXRegression = ecStat.regression('linear', WFTLRXdata);
                        var RRXRegression = ecStat.regression('linear', WFTRRXdata);
                        var LFYRegression = ecStat.regression('linear', WFTLFYdata);
                        var RFYRegression = ecStat.regression('linear', WFTRFYdata);
                        var LRYRegression = ecStat.regression('linear', WFTLRYdata);
                        var RRYRegression = ecStat.regression('linear', WFTRRYdata);
                        var LFZRegression = ecStat.regression('linear', WFTLFZdata);
                        var RFZRegression = ecStat.regression('linear', WFTRFZdata);
                        var LRZRegression = ecStat.regression('linear', WFTLRZdata);
                        var RRZRegression = ecStat.regression('linear', WFTRRZdata);

                        var trendLFXRegression = LFXRegression.points;
                        var trendRFXRegression = RFXRegression.points;
                        var trendLRXRegression = LRXRegression.points;
                        var trendRRXRegression = RRXRegression.points;
                        var trendLFYRegression = LFYRegression.points;
                        var trendRFYRegression = RFYRegression.points;
                        var trendLRYRegression = LRYRegression.points;
                        var trendRRYRegression = RRYRegression.points;

                        var trendLFZRegression = LFZRegression.points;
                        var trendRFZRegression = RFZRegression.points;
                        var trendLRZRegression = LRZRegression.points;
                        var trendRRZRegression = RRZRegression.points;

                        var allpredicitontime = [];
                        var allpredicitondayspan = [];
                        var allpredicitonresidue = [];



                        var LFXk = LFXRegression.parameter.gradient;
                        var LFXb = LFXRegression.parameter.intercept;
                        var LFXEquaGDNmuber = Math.ceil((LFXGDDamage - LFXb) / LFXk); //计算抵达GD损伤的时间整数
                        trendLFXRegression.push([LFXEquaGDNmuber, Math.ceil(LFXEquaGDNmuber * LFXk + LFXb)]);//把计算好的时间整数再放入拟合直线中计算结果再放入拟合数据中
                        var LFXtdatetime = new Date(LFXdatetime[0]);
                        var LFXGDtime = new Date(LFXtdatetime.setDate(LFXtdatetime.getDate() + LFXEquaGDNmuber));

                        LFXdatetime.push(LFXGDtime.toLocaleDateString());
                        allpredicitontime.push(LFXGDtime.toLocaleDateString());
                        var RFXk = RFXRegression.parameter.gradient;
                        var RFXb = RFXRegression.parameter.intercept;
                        var RFXEquaGDNmuber = Math.ceil((RFXGDDamage - RFXb) / RFXk);
                        trendRFXRegression.push([RFXEquaGDNmuber, Math.ceil(RFXEquaGDNmuber * RFXk + RFXb)]);
                        var RFXtdatetime = new Date(RFXdatetime[0]);
                        var RFXGDtime = new Date(RFXtdatetime.setDate(RFXtdatetime.getDate() + RFXEquaGDNmuber));

                        RFXdatetime.push(RFXGDtime.toLocaleDateString());
                        allpredicitontime.push(RFXGDtime.toLocaleDateString());
                        var LRXk = LRXRegression.parameter.gradient;
                        var LRXb = LRXRegression.parameter.intercept;
                        var LRXEquaGDNmuber = Math.ceil((LRXGDDamage - LRXb) / LRXk);
                        trendLRXRegression.push([LRXEquaGDNmuber, Math.ceil(LRXEquaGDNmuber * LRXk + LRXb)]);
                        var LRXtdatetime = new Date(LRXdatetime[0]);
                        var LRXGDtime = new Date(LRXtdatetime.setDate(LRXtdatetime.getDate() + LRXEquaGDNmuber));

                        LRXdatetime.push(LRXGDtime.toLocaleDateString());
                        allpredicitontime.push(LRXGDtime.toLocaleDateString());
                        var RRXk = RRXRegression.parameter.gradient;
                        var RRXb = RRXRegression.parameter.intercept;
                        var RRXEquaGDNmuber = Math.ceil((RRXGDDamage - RRXb) / RRXk);
                        trendRRXRegression.push([RRXEquaGDNmuber, Math.ceil(RRXEquaGDNmuber * RRXk + RRXb)]);
                        var RRXtdatetime = new Date(RRXdatetime[0]);
                        var RRXGDtime = new Date(RRXtdatetime.setDate(RRXtdatetime.getDate() + RRXEquaGDNmuber));

                        RRXdatetime.push(RRXGDtime.toLocaleDateString());
                        allpredicitontime.push(RRXGDtime.toLocaleDateString());
                        var LFYk = LFYRegression.parameter.gradient;
                        var LFYb = LFYRegression.parameter.intercept;
                        var LFYEquaGDNmuber = Math.ceil((LFYGDDamage - LFYb) / LFYk);
                        trendLFYRegression.push([LFYEquaGDNmuber, Math.ceil(LFYEquaGDNmuber * LFYk + LFYb)]);
                        var LFYtdatetime = new Date(LFYdatetime[0]);
                        var LFYGDtime = new Date(LFYtdatetime.setDate(LFYtdatetime.getDate() + LFYEquaGDNmuber));

                        LFYdatetime.push(LFYGDtime.toLocaleDateString());
                        allpredicitontime.push(LFYGDtime.toLocaleDateString());
                        var RFYk = RFYRegression.parameter.gradient;
                        var RFYb = RFYRegression.parameter.intercept;
                        var RFYEquaGDNmuber = Math.ceil((RFYGDDamage - RFYb) / RFYk);
                        trendRFYRegression.push([RFYEquaGDNmuber, Math.ceil(RFYEquaGDNmuber * RFYk + RFYb)]);
                        var RFYtdatetime = new Date(RFYdatetime[0]);
                        var RFYGDtime = new Date(RFYtdatetime.setDate(RFYtdatetime.getDate() + RFYEquaGDNmuber));

                        RFYdatetime.push(RFYGDtime.toLocaleDateString());
                        allpredicitontime.push(RFYGDtime.toLocaleDateString());
                        var LRYk = LRYRegression.parameter.gradient;
                        var LRYb = LRYRegression.parameter.intercept;
                        var LRYEquaGDNmuber = Math.ceil((LRYGDDamage - LRYb) / LRYk);
                        trendLRYRegression.push([LRYEquaGDNmuber, Math.ceil(LRYEquaGDNmuber * LRYk + LRYb)]);
                        var LRYtdatetime = new Date(LRYdatetime[0]);
                        var LRYGDtime = new Date(LRYtdatetime.setDate(LRYtdatetime.getDate() + LRYEquaGDNmuber));

                        LRYdatetime.push(LRYGDtime.toLocaleDateString());
                        allpredicitontime.push(LRYGDtime.toLocaleDateString());
                        var RRYk = RRYRegression.parameter.gradient;
                        var RRYb = RRYRegression.parameter.intercept;
                        var RRYEquaGDNmuber = Math.ceil((RRYGDDamage - RRYb) / RRYk);
                        trendRRYRegression.push([RRYEquaGDNmuber, Math.ceil(RRYEquaGDNmuber * RRYk + RRYb)]);
                        var RRYtdatetime = new Date(RRYdatetime[0]);
                        var RRYGDtime = new Date(RRYtdatetime.setDate(RRYtdatetime.getDate() + RRYEquaGDNmuber));

                        RRYdatetime.push(RRYGDtime.toLocaleDateString());
                        allpredicitontime.push(RRYGDtime.toLocaleDateString());
                        var LFZk = LFZRegression.parameter.gradient;
                        var LFZb = LFZRegression.parameter.intercept;
                        var LFZEquaGDNmuber = Math.ceil((LFZGDDamage - LFZb) / LFZk);
                        trendLFZRegression.push([LFZEquaGDNmuber, Math.ceil(LFZEquaGDNmuber * LFZk + LFZb)]);
                        var LFZtdatetime = new Date(LFZdatetime[0]);
                        var LFZGDtime = new Date(LFZtdatetime.setDate(LFZtdatetime.getDate() + LFZEquaGDNmuber));

                        LFZdatetime.push(LFZGDtime.toLocaleDateString());
                        allpredicitontime.push(LFZGDtime.toLocaleDateString());

                        var RFZk = RFZRegression.parameter.gradient;
                        var RFZb = RFZRegression.parameter.intercept;
                        var RFZEquaGDNmuber = Math.ceil((RFZGDDamage - RFZb) / RFZk);
                        trendRFZRegression.push([RFZEquaGDNmuber, Math.ceil(RFZEquaGDNmuber * RFZk + RFZb)]);
                        var RFZtdatetime = new Date(RFZdatetime[0]);
                        var RFZGDtime = new Date(RFZtdatetime.setDate(RFZtdatetime.getDate() + RFZEquaGDNmuber));

                        RFZdatetime.push(RFZGDtime.toLocaleDateString());
                        allpredicitontime.push(RFZGDtime.toLocaleDateString());

                        var LRZk = LRZRegression.parameter.gradient;
                        var LRZb = LRZRegression.parameter.intercept;
                        var LRZEquaGDNmuber = Math.ceil((LRZGDDamage - LRZb) / LRZk);
                        trendLRZRegression.push([LRZEquaGDNmuber, Math.ceil(LRZEquaGDNmuber * LRZk + LRZb)]);
                        var LRZtdatetime = new Date(LRZdatetime[0]);
                        var LRZGDtime = new Date(LRZtdatetime.setDate(LRZtdatetime.getDate() + LRZEquaGDNmuber));

                        LRZdatetime.push(LRZGDtime.toLocaleDateString());
                        allpredicitontime.push(LRZGDtime.toLocaleDateString());

                        var RRZk = RRZRegression.parameter.gradient;
                        var RRZb = RRZRegression.parameter.intercept;
                        var RRZEquaGDNmuber = Math.ceil((RRZGDDamage - RRZb) / RRZk);
                        trendRRZRegression.push([RRZEquaGDNmuber, Math.ceil(RRZEquaGDNmuber * RRZk + RRZb)]);
                        var RRZtdatetime = new Date(RRZdatetime[0]);
                        var RRZGDtime = new Date(RRZtdatetime.setDate(RRZtdatetime.getDate() + RRZEquaGDNmuber));

                        RRZdatetime.push(RRZGDtime.toLocaleDateString());
                        allpredicitontime.push(RRZGDtime.toLocaleDateString());

                        allpredicitondayspan.push(LFXEquaGDNmuber);
                        allpredicitondayspan.push(RFXEquaGDNmuber);
                        allpredicitondayspan.push(LRXEquaGDNmuber);
                        allpredicitondayspan.push(RRXEquaGDNmuber);
                        allpredicitondayspan.push(LFYEquaGDNmuber);
                        allpredicitondayspan.push(RFYEquaGDNmuber);
                        allpredicitondayspan.push(LRYEquaGDNmuber);
                        allpredicitondayspan.push(RRYEquaGDNmuber);
                        allpredicitondayspan.push(LFZEquaGDNmuber);
                        allpredicitondayspan.push(RFZEquaGDNmuber);
                        allpredicitondayspan.push(LRZEquaGDNmuber);
                        allpredicitondayspan.push(RRZEquaGDNmuber);

                        allpredicitonresidue.push(wftLFXresidue);
                        allpredicitonresidue.push(wftRFXresidue);
                        allpredicitonresidue.push(wftLRXresidue);
                        allpredicitonresidue.push(wftRRXresidue);
                        allpredicitonresidue.push(wftLFYresidue);
                        allpredicitonresidue.push(wftRFYresidue);
                        allpredicitonresidue.push(wftLRYresidue);
                        allpredicitonresidue.push(wftRRYresidue);
                        allpredicitonresidue.push(wftLFZresidue);
                        allpredicitonresidue.push(wftRFZresidue);
                        allpredicitonresidue.push(wftLRZresidue);
                        allpredicitonresidue.push(wftRRZresidue);
                        //var maxday = Math.max(allpredicitondayspan);

                        //console.log(allpredicitondayspan);

                        allpredictiondayoption = {

                            title: {
                                text: 'WFT剩余天数',
                                subtext: startdate + "to" + enddate,
                                x: 'center',
                                y: 'top',
                                textAlign: 'center'

                            },
                            tooltip: {
                                trigger: 'axis',

                                formatter: function (params) { // params 为一个数组，数组的每个元素 包含了 该折线图的点 所有的参数信息，比如 value(数值)、seriesName（系列名）、dataIndex（数据项的序号）
                                    let dateIndex = 0; // 当前指示点的 日期序号
                                    let tipList = params.map((seg) => {
                                        let { value, seriesName, dataIndex } = seg;
                                        dateIndex = dataIndex;
                                        //yvalue = value[0];
                                        return `${params[0].name}：${params[0].value}`
                                    })
                                    tipList.unshift(`${allpredicitontime[dateIndex]}`)
                                    return tipList.join('<br/>')
                                },
                            },
                            legend: {
                                data: ['天数']
                            },
                            xAxis: {
                                 type: 'value',
                                    axisLabel: {
                                        formatter: '{value}(天数)'
                                    }
                            },
                            yAxis: [
                                {
								type: 'category',
                                data: ["LFX", "RFX", "LRX", "RRX", "LFY", "RFY", "LRY", "RRY", "LFZ", "RFZ", "LRZ", "RRZ"]
                                   
                                }
                            ],
                            series: [{
                                name: 'WFT',
                                type: 'bar',
                                showBackground: true,
                                //itemStyle: {
                                //    color: new echarts.graphic.LinearGradient(
                                //        0, 0, 0, 1,
                                //        [
                                //            { offset: 0, color: '#83bff6' },
                                //            { offset: 0.5, color: '#188df0' },
                                //            { offset: 1, color: '#188df0' }
                                //        ]
                                //    )
                                //},
                                //emphasis: {
                                //    itemStyle: {
                                //        color: new echarts.graphic.LinearGradient(
                                //            0, 0, 0, 1,
                                //            [
                                //                { offset: 0, color: '#2378f7' },
                                //                { offset: 0.7, color: '#2378f7' },
                                //                { offset: 1, color: '#83bff6' }
                                //            ]
                                //        )
                                //    }
                                //},
                                label: {
                                    show: true,
                                    position: 'inside'
                                },
                                data: allpredicitondayspan
                            }
                            ]


                        }
                        allpredictionChart.setOption(allpredictiondayoption);

                        allpredictionresidueoption = {

                            title: {
                                text: 'WFT剩余损伤',
                                subtext: startdate + "to" + enddate,
                                x: 'center',
                                y: 'top',
                                textAlign: 'center'

                            },
                            tooltip: {
                                trigger: 'axis',
                                axisPointer: {
                                    type: 'shadow'
                                }
                               
                            },
                            legend: {
                                data: ['剩余损伤']
                            },
                            xAxis: {
                               type: 'value',
                                axisLabel: {
                                    formatter: '{value}(%)'
                                }
                            },
                            yAxis: [
                                {
                                    type: 'category',
                                    data: ["LFX", "RFX", "LRX", "RRX", "LFY", "RFY", "LRY", "RRY", "LFZ", "RFZ", "LRZ", "RRZ"]
                                }
                            ],
                            series: [{
                                name: 'WFT',
                                type: 'bar',
                                showBackground: true,
                                
                                
                                label: {
                                    show: true,
                                    position: 'inside'
                                },
                                data: allpredicitonresidue
                            }
                            ]


                        }
                        allpredictionresidueChart.setOption(allpredictionresidueoption);

                        LFXoption = {

                            title: {
                                text: '左前X',
                                subtext: startdate + "to" + enddate,

                                left: 'center'
                            },
                            legend: {
                                bottom: 5
                            },
                             grid: {
                                    left: '0%',
                                    right: '4%',
                                   
                                    containLabel: true
                                    },
                            tooltip: {
                                trigger: 'axis',
                                formatter: function (params) { // params 为一个数组，数组的每个元素 包含了 该折线图的点 所有的参数信息，比如 value(数值)、seriesName（系列名）、dataIndex（数据项的序号）
                                    let dateIndex = 0; // 当前指示点的 日期序号
                                    let tipList = params.map((seg) => {
                                        let { value, seriesName, dataIndex } = seg;
                                        dateIndex = dataIndex;
                                        yvalue = value[1];
                                        return `${seriesName}：${yvalue}`
                                    })
                                    tipList.unshift(`${LFXdatetime[dateIndex]}`)
                                    return tipList.join('<br/>')
                                },

                                axisPointer: {
                                    type: 'cross'
                                }
                            },
                            xAxis: {
                                max: Math.ceil(LFXEquaGDNmuber * 1.5),

                                //minInterval: 0.5,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },

                            },
                            yAxis: {
                                max: LFXGDDamage * 1.25,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },
                            },
                            series: [{
                                name: '累积损伤点（按天）',
                                type: 'scatter',
                                label: {
                                    emphasis: {
                                        show: true,
                                        position: 'left',
                                        textStyle: {
                                            color: 'blue',
                                            fontSize: 16
                                        }
                                    }
                                },
                                data: WFTLFXdata,
                                markLine: {
                                    silent: true,
                                    lineStyle: {
                                        normal: {
                                            color: '#01fef9'                   // 这儿设置安全基线颜色
                                        }
                                    },
                                    data: [{
                                        yAxis: LFXGDDamage
                                    }],
                                    label: {
                                        normal: {
                                            formatter: 'SD',
											align: 'center'
                                        }
                                    },
                                }


                            }, {
                                name: '趋势拟合线',
                                type: 'line',
                                showSymbol: false,
                                data: LFXRegression.points,
                                markPoint: {
                                    itemStyle: {
                                        normal: {
                                            color: 'transparent'
                                        }
                                    },
                                    label: {
                                        normal: {
                                            show: false,
                                            position: 'right',
                                            formatter: LFXRegression.expression,
                                            textStyle: {
                                                color: '#333',
                                                fontSize: 14
                                            }
                                        }
                                    },
                                    data: [{
                                        coord: LFXRegression.points[LFXRegression.points.length - 1]
                                    }]
                                }

                            }]
                        };
                        LFXChart.setOption(LFXoption);

                        RFXoption = {

                            title: {
                                text: '右前X',
                                subtext: startdate + "to" + enddate,

                                left: 'center'
                            },
                            legend: {
                                bottom: 5
                            },
									grid: {
                                         left: '0%',
                                    right: '4%',
                                                             
                                    containLabel: true
                                    },
                            tooltip: {
                                trigger: 'axis',
                                formatter: function (params) { // params 为一个数组，数组的每个元素 包含了 该折线图的点 所有的参数信息，比如 value(数值)、seriesName（系列名）、dataIndex（数据项的序号）
                                    let dateIndex = 0; // 当前指示点的 日期序号
                                    let tipList = params.map((seg) => {
                                        let { value, seriesName, dataIndex } = seg;
                                        dateIndex = dataIndex;
                                        yvalue = value[1];
                                        return `${seriesName}：${yvalue}`
                                    })
                                    tipList.unshift(`${RFXdatetime[dateIndex]}`)
                                    return tipList.join('<br/>')
                                },

                                axisPointer: {
                                    type: 'cross'
                                }
                            },
                            xAxis: {
                                max: Math.ceil(RFXEquaGDNmuber * 1.5),

                                //minInterval: 0.5,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },

                            },
                            yAxis: {
                                max: RFXGDDamage * 1.25,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },
                            },
                            series: [{
                                name: '累积损伤点（按天）',
                                type: 'scatter',
                                label: {
                                    emphasis: {
                                        show: true,
                                        position: 'left',
                                        textStyle: {
                                            color: 'blue',
                                            fontSize: 16
                                        }
                                    }
                                },
                                data: WFTRFXdata,
                                markLine: {
                                    silent: true,
                                    lineStyle: {
                                        normal: {
                                            color: '#01fef9'                   // 这儿设置安全基线颜色
                                        }
                                    },
                                    data: [{
                                        yAxis: RFXGDDamage
                                    }],
                                    label: {
                                        normal: {
                                            formatter: 'SD',
											align: 'center'
                                        }
                                    },
                                }


                            }, {
                                name: '趋势拟合线',
                                type: 'line',
                                showSymbol: false,
                                data: RFXRegression.points,
                                markPoint: {
                                    itemStyle: {
                                        normal: {
                                            color: 'transparent'
                                        }
                                    },
                                    label: {
                                        normal: {
                                            show: false,
                                            position: 'right',
                                            formatter: RFXRegression.expression,
                                            textStyle: {
                                                color: '#333',
                                                fontSize: 14
                                            }
                                        }
                                    },
                                    data: [{
                                        coord: RFXRegression.points[RFXRegression.points.length - 1]
                                    }]
                                }

                            }]
                        };
                        RFXChart.setOption(RFXoption);

                        LRXoption = {

                            title: {
                                text: '左后X',
                                subtext: startdate + "to" + enddate,

                                left: 'center'
                            },
                            legend: {
                                bottom: 5
                            },
									grid: {
                                    left: '0%',
                                    right: '4%',
                                   
                                    containLabel: true
                                    },
                            tooltip: {
                                trigger: 'axis',
                                formatter: function (params) { // params 为一个数组，数组的每个元素 包含了 该折线图的点 所有的参数信息，比如 value(数值)、seriesName（系列名）、dataIndex（数据项的序号）
                                    let dateIndex = 0; // 当前指示点的 日期序号
                                    let tipList = params.map((seg) => {
                                        let { value, seriesName, dataIndex } = seg;
                                        dateIndex = dataIndex;
                                        yvalue = value[1];
                                        return `${seriesName}：${yvalue}`
                                    })
                                    tipList.unshift(`${LRXdatetime[dateIndex]}`)
                                    return tipList.join('<br/>')
                                },

                                axisPointer: {
                                    type: 'cross'
                                }
                            },
                            xAxis: {
                                max: Math.ceil(LRXEquaGDNmuber * 1.5),

                                //minInterval: 0.5,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },

                            },
                            yAxis: {
                                max: LRXGDDamage * 1.25,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },
                            },
                            series: [{
                                name: '累积损伤点（按天）',
                                type: 'scatter',
                                label: {
                                    emphasis: {
                                        show: true,
                                        position: 'left',
                                        textStyle: {
                                            color: 'blue',
                                            fontSize: 16
                                        }
                                    }
                                },
                                data: WFTLRXdata,
                                markLine: {
                                    silent: true,
                                    lineStyle: {
                                        normal: {
                                            color: '#01fef9'                   // 这儿设置安全基线颜色
                                        }
                                    },
                                    data: [{
                                        yAxis: LRXGDDamage
                                    }],
                                    label: {
                                        normal: {
                                             formatter: 'SD',
											align: 'center'      
                                        }
                                    },
                                }


                            }, {
                                name: '趋势拟合线',
                                type: 'line',
                                showSymbol: false,
                                data: LRXRegression.points,
                                markPoint: {
                                    itemStyle: {
                                        normal: {
                                            color: 'transparent'
                                        }
                                    },
                                    label: {
                                        normal: {
                                            show: false,
                                            position: 'right',
                                            formatter: LRXRegression.expression,
                                            textStyle: {
                                                color: '#333',
                                                fontSize: 14
                                            }
                                        }
                                    },
                                    data: [{
                                        coord: LRXRegression.points[LRXRegression.points.length - 1]
                                    }]
                                }

                            }]
                        };
                        LRXChart.setOption(LRXoption);

                        RRXoption = {

                            title: {
                                text: '右后X',
                                subtext: startdate + "to" + enddate,

                                left: 'center'
                            },
                            legend: {
                                bottom: 5
                            },
									grid: {
                                    left: '0%',
                                    right: '4%',
                                   
                                    containLabel: true
                                    },
                            tooltip: {
                                trigger: 'axis',
                                formatter: function (params) { // params 为一个数组，数组的每个元素 包含了 该折线图的点 所有的参数信息，比如 value(数值)、seriesName（系列名）、dataIndex（数据项的序号）
                                    let dateIndex = 0; // 当前指示点的 日期序号
                                    let tipList = params.map((seg) => {
                                        let { value, seriesName, dataIndex } = seg;
                                        dateIndex = dataIndex;
                                        yvalue = value[1];
                                        return `${seriesName}：${yvalue}`
                                    })
                                    tipList.unshift(`${RRXdatetime[dateIndex]}`)
                                    return tipList.join('<br/>')
                                },

                                axisPointer: {
                                    type: 'cross'
                                }
                            },
                            xAxis: {
                                max: Math.ceil(RRXEquaGDNmuber * 1.5),

                                //minInterval: 0.5,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },

                            },
                            yAxis: {
                                max: RRXGDDamage * 1.25,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },
                            },
                            series: [{
                                name: '累积损伤点（按天）',
                                type: 'scatter',
                                label: {
                                    emphasis: {
                                        show: true,
                                        position: 'left',
                                        textStyle: {
                                            color: 'blue',
                                            fontSize: 16
                                        }
                                    }
                                },
                                data: WFTRRXdata,
                                markLine: {
                                    silent: true,
                                    lineStyle: {
                                        normal: {
                                            color: '#01fef9'                   // 这儿设置安全基线颜色
                                        }
                                    },
                                    data: [{
                                        yAxis: RRXGDDamage
                                    }],
                                    label: {
                                        normal: {
                                             formatter: 'SD',
											align: 'center'        // 这儿设置安全基线
                                        }
                                    },
                                }


                            }, {
                                name: '趋势拟合线',
                                type: 'line',
                                showSymbol: false,
                                data: RRXRegression.points,
                                markPoint: {
                                    itemStyle: {
                                        normal: {
                                            color: 'transparent'
                                        }
                                    },
                                    label: {
                                        normal: {
                                            show: false,
                                            position: 'right',
                                            formatter: RRXRegression.expression,
                                            textStyle: {
                                                color: '#333',
                                                fontSize: 14
                                            }
                                        }
                                    },
                                    data: [{
                                        coord: RRXRegression.points[RRXRegression.points.length - 1]
                                    }]
                                }

                            }]
                        };
                        RRXChart.setOption(RRXoption);

                        LFYoption = {

                            title: {
                                text: '左前Y',
                                subtext: startdate + "to" + enddate,

                                left: 'center'
                            },
                            legend: {
                                bottom: 5
                            },
									grid: {
                                    left: '0%',
                                    right: '4%',
                                   
                                    containLabel: true
                                    },
                            tooltip: {
                                trigger: 'axis',
                                formatter: function (params) { // params 为一个数组，数组的每个元素 包含了 该折线图的点 所有的参数信息，比如 value(数值)、seriesName（系列名）、dataIndex（数据项的序号）
                                    let dateIndex = 0; // 当前指示点的 日期序号
                                    let tipList = params.map((seg) => {
                                        let { value, seriesName, dataIndex } = seg;
                                        dateIndex = dataIndex;
                                        yvalue = value[1];
                                        return `${seriesName}：${yvalue}`
                                    })
                                    tipList.unshift(`${LFYdatetime[dateIndex]}`)
                                    return tipList.join('<br/>')
                                },

                                axisPointer: {
                                    type: 'cross'
                                }
                            },
                            xAxis: {
                                max: Math.ceil(LFYEquaGDNmuber * 1.5),

                                //minInterval: 0.5,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },

                            },
                            yAxis: {
                                max: LFYGDDamage * 1.25,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },
                            },
                            series: [{
                                name: '累积损伤点（按天）',
                                type: 'scatter',
                                label: {
                                    emphasis: {
                                        show: true,
                                        position: 'left',
                                        textStyle: {
                                            color: 'blue',
                                            fontSize: 16
                                        }
                                    }
                                },
                                data: WFTLFYdata,
                                markLine: {
                                    silent: true,
                                    lineStyle: {
                                        normal: {
                                            color: '#01fef9'                   // 这儿设置安全基线颜色
                                        }
                                    },
                                    data: [{
                                        yAxis: LFYGDDamage
                                    }],
                                    label: {
                                        normal: {
                                            formatter: 'SD',
											align: 'center'         // 这儿设置安全基线
                                        }
                                    },
                                }


                            }, {
                                name: '趋势拟合线',
                                type: 'line',
                                showSymbol: false,
                                data: LFYRegression.points,
                                markPoint: {
                                    itemStyle: {
                                        normal: {
                                            color: 'transparent'
                                        }
                                    },
                                    label: {
                                        normal: {
                                            show: false,
                                            position: 'right',
                                            formatter: LFYRegression.expression,
                                            textStyle: {
                                                color: '#333',
                                                fontSize: 14
                                            }
                                        }
                                    },
                                    data: [{
                                        coord: LFYRegression.points[LFYRegression.points.length - 1]
                                    }]
                                }

                            }]
                        };
                        LFYChart.setOption(LFYoption);

                        RFYoption = {

                            title: {
                                text: '右前Y',
                                subtext: startdate + "to" + enddate,

                                left: 'center'
                            },
                            legend: {
                                bottom: 5
                            },
									grid: {
                                    left: '0%',
                                    right: '4%',
                                   
                                    containLabel: true
                                    },
                            tooltip: {
                                trigger: 'axis',
                                formatter: function (params) { // params 为一个数组，数组的每个元素 包含了 该折线图的点 所有的参数信息，比如 value(数值)、seriesName（系列名）、dataIndex（数据项的序号）
                                    let dateIndex = 0; // 当前指示点的 日期序号
                                    let tipList = params.map((seg) => {
                                        let { value, seriesName, dataIndex } = seg;
                                        dateIndex = dataIndex;
                                        yvalue = value[1];
                                        return `${seriesName}：${yvalue}`
                                    })
                                    tipList.unshift(`${RFYdatetime[dateIndex]}`)
                                    return tipList.join('<br/>')
                                },

                                axisPointer: {
                                    type: 'cross'
                                }
                            },
                            xAxis: {
                                max: Math.ceil(RFYEquaGDNmuber * 1.5),

                                //minInterval: 0.5,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },

                            },
                            yAxis: {
                                max: RFYGDDamage * 1.25,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },
                            },
                            series: [{
                                name: '累积损伤点（按天）',
                                type: 'scatter',
                                label: {
                                    emphasis: {
                                        show: true,
                                        position: 'left',
                                        textStyle: {
                                            color: 'blue',
                                            fontSize: 16
                                        }
                                    }
                                },
                                data: WFTRFYdata,
                                markLine: {
                                    silent: true,
                                    lineStyle: {
                                        normal: {
                                            color: '#01fef9'                   // 这儿设置安全基线颜色
                                        }
                                    },
                                    data: [{
                                        yAxis: RFYGDDamage
                                    }],
                                    label: {
                                        normal: {
                                             formatter: 'SD',
											align: 'center'         // 这儿设置安全基线
                                        }
                                    },
                                }


                            }, {
                                name: '趋势拟合线',
                                type: 'line',
                                showSymbol: false,
                                data: RFYRegression.points,
                                markPoint: {
                                    itemStyle: {
                                        normal: {
                                            color: 'transparent'
                                        }
                                    },
                                    label: {
                                        normal: {
                                            show: false,
                                            position: 'right',
                                            formatter: RFYRegression.expression,
                                            textStyle: {
                                                color: '#333',
                                                fontSize: 14
                                            }
                                        }
                                    },
                                    data: [{
                                        coord: RFYRegression.points[RFYRegression.points.length - 1]
                                    }]
                                }

                            }]
                        };
                        RFYChart.setOption(RFYoption);

                        LRYoption = {

                            title: {
                                text: '左后Y',
                                subtext: startdate + "to" + enddate,

                                left: 'center'
                            },
                            legend: {
                                bottom: 5
                            },
									grid: {
                                    left: '0%',
                                    right: '4%',
                                   
                                    containLabel: true
                                    },
                            tooltip: {
                                trigger: 'axis',
                                formatter: function (params) { // params 为一个数组，数组的每个元素 包含了 该折线图的点 所有的参数信息，比如 value(数值)、seriesName（系列名）、dataIndex（数据项的序号）
                                    let dateIndex = 0; // 当前指示点的 日期序号
                                    let tipList = params.map((seg) => {
                                        let { value, seriesName, dataIndex } = seg;
                                        dateIndex = dataIndex;
                                        yvalue = value[1];
                                        return `${seriesName}：${yvalue}`
                                    })
                                    tipList.unshift(`${LRYdatetime[dateIndex]}`)
                                    return tipList.join('<br/>')
                                },

                                axisPointer: {
                                    type: 'cross'
                                }
                            },
                            xAxis: {
                                max: Math.ceil(LRYEquaGDNmuber * 1.5),

                                //minInterval: 0.5,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },

                            },
                            yAxis: {
                                max: LRYGDDamage * 1.25,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },
                            },
                            series: [{
                                name: '累积损伤点（按天）',
                                type: 'scatter',
                                label: {
                                    emphasis: {
                                        show: true,
                                        position: 'left',
                                        textStyle: {
                                            color: 'blue',
                                            fontSize: 16
                                        }
                                    }
                                },
                                data: WFTLRYdata,
                                markLine: {
                                    silent: true,
                                    lineStyle: {
                                        normal: {
                                            color: '#01fef9'                   // 这儿设置安全基线颜色
                                        }
                                    },
                                    data: [{
                                        yAxis: LRYGDDamage
                                    }],
                                    label: {
                                        normal: {
                                             formatter: 'SD',
											align: 'center'        // 这儿设置安全基线
                                        }
                                    },
                                }


                            }, {
                                name: '趋势拟合线',
                                type: 'line',
                                showSymbol: false,
                                data: LRYRegression.points,
                                markPoint: {
                                    itemStyle: {
                                        normal: {
                                            color: 'transparent'
                                        }
                                    },
                                    label: {
                                        normal: {
                                            show: false,
                                            position: 'right',
                                            formatter: LRYRegression.expression,
                                            textStyle: {
                                                color: '#333',
                                                fontSize: 14
                                            }
                                        }
                                    },
                                    data: [{
                                        coord: LRYRegression.points[LRYRegression.points.length - 1]
                                    }]
                                }

                            }]
                        };
                        LRYChart.setOption(LRYoption);

                        LFZoption = {

                            title: {
                                text: '左前Z',
                                subtext: startdate + "to" + enddate,

                                left: 'center'
                            },
                            legend: {
                                bottom: 5
                            },
									grid: {
                                    left: '0%',
                                    right: '4%',
                                   
                                    containLabel: true
                                    },
                            tooltip: {
                                trigger: 'axis',
                                formatter: function (params) { // params 为一个数组，数组的每个元素 包含了 该折线图的点 所有的参数信息，比如 value(数值)、seriesName（系列名）、dataIndex（数据项的序号）
                                    let dateIndex = 0; // 当前指示点的 日期序号
                                    let tipList = params.map((seg) => {
                                        let { value, seriesName, dataIndex } = seg;
                                        dateIndex = dataIndex;
                                        yvalue = value[1];
                                        return `${seriesName}：${yvalue}`
                                    })
                                    tipList.unshift(`${LFZdatetime[dateIndex]}`)
                                    return tipList.join('<br/>')
                                },

                                axisPointer: {
                                    type: 'cross'
                                }
                            },
                            xAxis: {
                                max: Math.ceil(LFZEquaGDNmuber * 1.5),

                                //minInterval: 0.5,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },

                            },
                            yAxis: {
                                max: LFZGDDamage * 1.25,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },
                            },
                            series: [{
                                name: '累积损伤点（按天）',
                                type: 'scatter',
                                label: {
                                    emphasis: {
                                        show: true,
                                        position: 'left',
                                        textStyle: {
                                            color: 'blue',
                                            fontSize: 16
                                        }
                                    }
                                },
                                data: WFTLFZdata,
                                markLine: {
                                    silent: true,
                                    lineStyle: {
                                        normal: {
                                            color: '#01fef9'                   // 这儿设置安全基线颜色
                                        }
                                    },
                                    data: [{
                                        yAxis: LFZGDDamage
                                    }],
                                    label: {
                                        normal: {
                                             formatter: 'SD',
											align: 'center'         // 这儿设置安全基线
                                        }
                                    },
                                }


                            }, {
                                name: '趋势拟合线',
                                type: 'line',
                                showSymbol: false,
                                data: LFZRegression.points,
                                markPoint: {
                                    itemStyle: {
                                        normal: {
                                            color: 'transparent'
                                        }
                                    },
                                    label: {
                                        normal: {
                                            show: false,
                                            position: 'right',
                                            formatter: LFZRegression.expression,
                                            textStyle: {
                                                color: '#333',
                                                fontSize: 14
                                            }
                                        }
                                    },
                                    data: [{
                                        coord: LFZRegression.points[LFZRegression.points.length - 1]
                                    }]
                                }

                            }]
                        };
                        LFZChart.setOption(LFZoption);

                        RFZoption = {

                            title: {
                                text: '右前Z',
                                subtext: startdate + "to" + enddate,

                                left: 'center'
                            },
                            legend: {
                                bottom: 5
                            },
									grid: {
                                    left: '0%',
                                    right: '4%',
                                   
                                    containLabel: true
                                    },
                            tooltip: {
                                trigger: 'axis',
                                formatter: function (params) { // params 为一个数组，数组的每个元素 包含了 该折线图的点 所有的参数信息，比如 value(数值)、seriesName（系列名）、dataIndex（数据项的序号）
                                    let dateIndex = 0; // 当前指示点的 日期序号
                                    let tipList = params.map((seg) => {
                                        let { value, seriesName, dataIndex } = seg;
                                        dateIndex = dataIndex;
                                        yvalue = value[1];
                                        return `${seriesName}：${yvalue}`
                                    })
                                    tipList.unshift(`${RFZdatetime[dateIndex]}`)
                                    return tipList.join('<br/>')
                                },

                                axisPointer: {
                                    type: 'cross'
                                }
                            },
                            xAxis: {
                                max: Math.ceil(RFZEquaGDNmuber * 1.5),

                                //minInterval: 0.5,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },

                            },
                            yAxis: {
                                max: RFZGDDamage * 1.25,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },
                            },
                            series: [{
                                name: '累积损伤点（按天）',
                                type: 'scatter',
                                label: {
                                    emphasis: {
                                        show: true,
                                        position: 'left',
                                        textStyle: {
                                            color: 'blue',
                                            fontSize: 16
                                        }
                                    }
                                },
                                data: WFTRFZdata,
                                markLine: {
                                    silent: true,
                                    lineStyle: {
                                        normal: {
                                            color: '#01fef9'                   // 这儿设置安全基线颜色
                                        }
                                    },
                                    data: [{
                                        yAxis: RFZGDDamage
                                    }],
                                    label: {
                                        normal: {
                                             formatter: 'SD',
											align: 'center'        // 这儿设置安全基线
                                        }
                                    },
                                }


                            }, {
                                name: '趋势拟合线',
                                type: 'line',
                                showSymbol: false,
                                data: RFZRegression.points,
                                markPoint: {
                                    itemStyle: {
                                        normal: {
                                            color: 'transparent'
                                        }
                                    },
                                    label: {
                                        normal: {
                                            show: false,
                                            position: 'right',
                                            formatter: RFZRegression.expression,
                                            textStyle: {
                                                color: '#333',
                                                fontSize: 14
                                            }
                                        }
                                    },
                                    data: [{
                                        coord: RFZRegression.points[RFZRegression.points.length - 1]
                                    }]
                                }

                            }]
                        };
                        RFZChart.setOption(RFZoption);

                        LRZoption = {

                            title: {
                                text: '左后Z',
                                subtext: startdate + "to" + enddate,

                                left: 'center'
                            },
                            legend: {
                                bottom: 5
                            },
									grid: {
                                    left: '0%',
                                    right: '4%',
                                   
                                    containLabel: true
                                    },
                            tooltip: {
                                trigger: 'axis',
                                formatter: function (params) { // params 为一个数组，数组的每个元素 包含了 该折线图的点 所有的参数信息，比如 value(数值)、seriesName（系列名）、dataIndex（数据项的序号）
                                    let dateIndex = 0; // 当前指示点的 日期序号
                                    let tipList = params.map((seg) => {
                                        let { value, seriesName, dataIndex } = seg;
                                        dateIndex = dataIndex;
                                        yvalue = value[1];
                                        return `${seriesName}：${yvalue}`
                                    })
                                    tipList.unshift(`${LRZdatetime[dateIndex]}`)
                                    return tipList.join('<br/>')
                                },

                                axisPointer: {
                                    type: 'cross'
                                }
                            },
                            xAxis: {
                                max: Math.ceil(LRZEquaGDNmuber * 1.5),

                                //minInterval: 0.5,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },

                            },
                            yAxis: {
                                max: LRZGDDamage * 1.25,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },
                            },
                            series: [{
                                name: '累积损伤点（按天）',
                                type: 'scatter',
                                label: {
                                    emphasis: {
                                        show: true,
                                        position: 'left',
                                        textStyle: {
                                            color: 'blue',
                                            fontSize: 16
                                        }
                                    }
                                },
                                data: WFTLRZdata,
                                markLine: {
                                    silent: true,
                                    lineStyle: {
                                        normal: {
                                            color: '#01fef9'                   // 这儿设置安全基线颜色
                                        }
                                    },
                                    data: [{
                                        yAxis: LRZGDDamage
                                    }],
                                    label: {
                                        normal: {
                                             formatter: 'SD',
											align: 'center'        // 这儿设置安全基线
                                        }
                                    },
                                }


                            }, {
                                name: '趋势拟合线',
                                type: 'line',
                                showSymbol: false,
                                data: LRZRegression.points,
                                markPoint: {
                                    itemStyle: {
                                        normal: {
                                            color: 'transparent'
                                        }
                                    },
                                    label: {
                                        normal: {
                                            show: false,
                                            position: 'right',
                                            formatter: LRZRegression.expression,
                                            textStyle: {
                                                color: '#333',
                                                fontSize: 14
                                            }
                                        }
                                    },
                                    data: [{
                                        coord: LRZRegression.points[LRZRegression.points.length - 1]
                                    }]
                                }

                            }]
                        };
                        LRZChart.setOption(LRZoption);

                        RRZoption = {

                            title: {
                                text: '右后Z',
                                subtext: startdate + "to" + enddate,

                                left: 'center'
                            },
                            legend: {
                                bottom: 5
                            },
									grid: {
                                    left: '0%',
                                    right: '4%',
                                   
                                    containLabel: true
                                    },
                            tooltip: {
                                trigger: 'axis',
                                formatter: function (params) { // params 为一个数组，数组的每个元素 包含了 该折线图的点 所有的参数信息，比如 value(数值)、seriesName（系列名）、dataIndex（数据项的序号）
                                    let dateIndex = 0; // 当前指示点的 日期序号
                                    let tipList = params.map((seg) => {
                                        let { value, seriesName, dataIndex } = seg;
                                        dateIndex = dataIndex;
                                        yvalue = value[1];
                                        return `${seriesName}：${yvalue}`
                                    })
                                    tipList.unshift(`${RRZdatetime[dateIndex]}`)
                                    return tipList.join('<br/>')
                                },

                                axisPointer: {
                                    type: 'cross'
                                }
                            },
                            xAxis: {
                                max: Math.ceil(RRZEquaGDNmuber * 1.5),

                                //minInterval: 0.5,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },

                            },
                            yAxis: {
                                max: RRZGDDamage * 1.25,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },
                            },
                            series: [{
                                name: '累积损伤点（按天）',
                                type: 'scatter',
                                label: {
                                    emphasis: {
                                        show: true,
                                        position: 'left',
                                        textStyle: {
                                            color: 'blue',
                                            fontSize: 16
                                        }
                                    }
                                },
                                data: WFTRRZdata,
                                markLine: {
                                    silent: true,
                                    lineStyle: {
                                        normal: {
                                            color: '#01fef9'                   // 这儿设置安全基线颜色
                                        }
                                    },
                                    data: [{
                                        yAxis: RRZGDDamage
                                    }],
                                    label: {
                                        normal: {
                                             formatter: 'SD',
											align: 'center'        // 这儿设置安全基线
                                        }
                                    },
                                }


                            }, {
                                name: '趋势拟合线',
                                type: 'line',
                                showSymbol: false,
                                data: RRZRegression.points,
                                markPoint: {
                                    itemStyle: {
                                        normal: {
                                            color: 'transparent'
                                        }
                                    },
                                    label: {
                                        normal: {
                                            show: false,
                                            position: 'right',
                                            formatter: RRZRegression.expression,
                                            textStyle: {
                                                color: '#333',
                                                fontSize: 14
                                            }
                                        }
                                    },
                                    data: [{
                                        coord: RRZRegression.points[RRZRegression.points.length - 1]
                                    }]
                                }

                            }]
                        };
                        RRZChart.setOption(RRZoption);


                        RRYoption = {

                            title: {
                                text: '右后Y',
                                subtext: startdate + "to" + enddate,

                                left: 'center'
                            },
                            legend: {
                                bottom: 5
                            },
									grid: {
                                    left: '0%',
                                    right: '4%',
                                   
                                    containLabel: true
                                    },
                            tooltip: {
                                trigger: 'axis',
                                formatter: function (params) { // params 为一个数组，数组的每个元素 包含了 该折线图的点 所有的参数信息，比如 value(数值)、seriesName（系列名）、dataIndex（数据项的序号）
                                    let dateIndex = 0; // 当前指示点的 日期序号
                                    let tipList = params.map((seg) => {
                                        let { value, seriesName, dataIndex } = seg;
                                        dateIndex = dataIndex;
                                        yvalue = value[1];
                                        return `${seriesName}：${yvalue}`
                                    })
                                    tipList.unshift(`${RRYdatetime[dateIndex]}`)
                                    return tipList.join('<br/>')
                                },

                                axisPointer: {
                                    type: 'cross'
                                }
                            },
                            xAxis: {
                                max: Math.ceil(RRYEquaGDNmuber * 1.5),

                                //minInterval: 0.5,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },

                            },
                            yAxis: {
                                max: RRYGDDamage * 1.25,
                                splitLine: {
                                    lineStyle: {
                                        type: 'dashed'
                                    }
                                },
                            },
                            series: [{
                                name: '累积损伤点（按天）',
                                type: 'scatter',
                                label: {
                                    emphasis: {
                                        show: true,
                                        position: 'left',
                                        textStyle: {
                                            color: 'blue',
                                            fontSize: 16
                                        }
                                    }
                                },
                                data: WFTRRYdata,
                                markLine: {
                                    silent: true,
                                    lineStyle: {
                                        normal: {
                                            color: '#01fef9'                   // 这儿设置安全基线颜色
                                        }
                                    },
                                    data: [{
                                        yAxis: RRYGDDamage
                                    }],
                                    label: {
                                        normal: {
                                            formatter: 'SD',
											align: 'center'         // 这儿设置安全基线
                                        }
                                    },
                                }


                            }, {
                                name: '趋势拟合线',
                                type: 'line',
                                showSymbol: false,
                                data: RRYRegression.points,
                                markPoint: {
                                    itemStyle: {
                                        normal: {
                                            color: 'transparent'
                                        }
                                    },
                                    label: {
                                        normal: {
                                            show: false,
                                            position: 'right',
                                            formatter: RRYRegression.expression,
                                            textStyle: {
                                                color: '#333',
                                                fontSize: 14
                                            }
                                        }
                                    },
                                    data: [{
                                        coord: RRYRegression.points[RRYRegression.points.length - 1]
                                    }]
                                }

                            }]
                        };
                        RRYChart.setOption(RRYoption);

                        layer.close(index);
                    }
                   
                    else {
                        layer.close(index);
                        layer.msg("此车辆没有数据");
                    }
                }
                , error: function (e) {
                    layer.msg(e);
                }

            });
        }
        else {
          
            layer.msg("请先选择日期！！！");
        }
    });


})