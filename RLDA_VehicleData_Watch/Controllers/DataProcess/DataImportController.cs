using IBLL.SH_ADF0979IBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tools;
using Tools.DateTimeOperation;
using Tools.MyConfig;

namespace RLDA_VehicleData_Watch.Controllers.DataProcess
{
    public class DataImportController : Controller
    {
        private readonly ILogger<DataImportController> _logger;
        private readonly IAnalysisData_ACC_IBLL _IAnalysisData_ACC_Service;
        private readonly IAnalysisData_WFT_IBLL _IAnalysisData_WFT_Service;
        private readonly IConfiguration _configuration;
        private readonly MemoryCache _memoryCache;//内存缓存

        private static string inputpath;
        private static string resultpath;

        private static string importenable;
        private static string vehilceimport;

        public DataImportController(IAnalysisData_ACC_IBLL IAnalysisData_ACC_Service, IAnalysisData_WFT_IBLL IAnalysisData_WFT_Service, ILogger<DataImportController> logger, IConfiguration configuration, MemoryCache memoryCache)
        {
            _IAnalysisData_ACC_Service = IAnalysisData_ACC_Service;
            _IAnalysisData_WFT_Service = IAnalysisData_WFT_Service;
            _logger = logger;
            _configuration = configuration;
            _memoryCache= memoryCache;
            importenable = _configuration["DataImport:ImportRequired"];
            vehilceimport = _configuration["DataImport:ImportVehicleID"];
        }
        public async Task<string> DataImport(string startdate, string enddate,string vehicleid)
        {
            var VehicleIDPara = MyConfigforVehicleID.GetVehicleConfiguration(vehicleid);
            if (VehicleIDPara.channels != null)
            {
                if (importenable == "true")//判断是否允许导入数据
                {
                    if (vehilceimport.Contains(vehicleid))//判断是否有允许导入数据的车辆号
                    {
                        if (startdate != null && enddate != null)
                        {
                            int span = DateTimeOperation.DateDiff(startdate, enddate);
                            inputpath = _configuration[vehicleid + ":inputpathimport"];
                            resultpath = _configuration[vehicleid + ":resultpathimport"];
                            string importresult = "";
                            try
                            {
                                for (int i = 0; i < span + 1; i++)
                                {
                                    string date = FileOperator.DatetoName(Convert.ToDateTime(startdate).AddDays(i).ToString("yyyy-MM-dd")).Substring(5);//当前计算的日期文件夹名称
                                    string inputfiletimeinfo = inputpath + date;
                                    string outputfiletimeinfo = resultpath + date;
                                    //判断input子文件夹里是否有导入的flag
                                    if (!FileOperator.DataImportCompleteFlag(inputfiletimeinfo))
                                    {
                                        //判断是否有数据上传完整
                                        if (FileOperator.DataTransferCompleteFlag(inputfiletimeinfo, date))
                                        {
                                            _logger.LogInformation(date + "的input数据开始手动导入" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                            //开始input上传
                                            var t1 = _IAnalysisData_ACC_Service.ReadandMergeACCDataperHalfHour(inputfiletimeinfo, vehicleid, VehicleIDPara);
                                            t1.Wait();//等待t1完成才进行下一步
                                            if (await t1)
                                            {
                                                _logger.LogInformation(vehicleid + "日期为" + date + "的input数据手动导入完成时间" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                                FileOperator.CreateDataImportCompleteFlag(inputfiletimeinfo);
                                                importresult += vehicleid + "日期为" + date + "的input数据手动导入完成" + System.Environment.NewLine;
                                                //判断result子文件里是否有导入的flag
                                                {
                                                //if (!FileOperator.DataImportCompleteFlag(outputfiletimeinfo))
                                                //{
                                                //    //判断result子文件里数据是否上传完整
                                                //    if (FileOperator.DataTransferCompleteFlag(outputfiletimeinfo, date))
                                                //    {
                                                //        //开始result上传
                                                //        _logger.LogInformation(date + "的result数据开始手动导入" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                                //        var t2 = _IAnalysisData_WFT_Service.ReadandMergeWFTDataperHalfHour(outputfiletimeinfo, vehicleid, VehicleIDPara);
                                                //        t2.Wait();
                                                //        if (await t2)
                                                //        {
                                                //            _logger.LogInformation(vehicleid + "日期为" + date + "的result数据手动导入完成时间" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                                //            FileOperator.CreateDataImportCompleteFlag(outputfiletimeinfo);
                                                //            importresult += vehicleid + "日期为" + date + "的result数据手动导入完成" + System.Environment.NewLine;
                                                //        }

                                                //        else
                                                //        {
                                                //            _logger.LogInformation(vehicleid + "日期为" + date + "的result无数据文件可读取");
                                                //            importresult += vehicleid + "日期为" + date + "的result无数据文件可读取" + System.Environment.NewLine;
                                                //        }
                                                //    }
                                                //    else
                                                //    {
                                                //        _logger.LogInformation(vehicleid + "日期为" + date + "的result数据还没有上传完毕");
                                                //        importresult += vehicleid + "日期为" + date + "的result数据还没有上传完毕" + System.Environment.NewLine;
                                                //    }
                                                //}

                                                //else
                                                //{
                                                //    _logger.LogInformation(vehicleid + "日期为" + date + "的result数据已有导入flag，不需要再次导入");
                                                //    importresult += vehicleid + "日期为" + date + "的result数据已有导入flag，不需要再次导入" + System.Environment.NewLine;
                                                //}
                                                }
                                            }
                                            else
                                            {
                                                _logger.LogInformation(vehicleid + "日期为" + date + "的input无数据文件可读取");
                                                importresult += vehicleid + "日期为" + date + "的input无数据文件可读取" + System.Environment.NewLine;
                                            }
                                        }
                                        else
                                        {
                                            _logger.LogInformation(vehicleid + "日期为" + date + "的input数据还没有上传完毕");
                                            importresult += vehicleid + "日期为" + date + "的input数据还没有上传完毕" + System.Environment.NewLine;
                                        }

                                    }
                                    else
                                    {
                                        _logger.LogInformation(vehicleid + "日期为" + date + "的input数据已有导入flag，不需要再次导入");
                                        importresult += vehicleid + "日期为" + date + "的input数据已有导入flag，不需要再次导入" + System.Environment.NewLine;
                                    }

                                    if (!FileOperator.DataImportCompleteFlag(outputfiletimeinfo))
                                    {
                                        //判断result子文件里数据是否上传完整
                                        if (FileOperator.DataTransferCompleteFlag(outputfiletimeinfo, date))
                                        {
                                            //开始result上传
                                            _logger.LogInformation(date + "的result数据开始手动导入" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                            var t2 = _IAnalysisData_WFT_Service.ReadandMergeWFTDataperHalfHour(outputfiletimeinfo, vehicleid, VehicleIDPara);
                                            t2.Wait();
                                            if (await t2)
                                            {
                                                _logger.LogInformation(vehicleid + "日期为" + date + "的result数据手动导入完成时间" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                                FileOperator.CreateDataImportCompleteFlag(outputfiletimeinfo);
                                                importresult += vehicleid + "日期为" + date + "的result数据手动导入完成" + System.Environment.NewLine;
                                            }

                                            else
                                            {
                                                _logger.LogInformation(vehicleid + "日期为" + date + "的result无数据文件可读取");
                                                importresult += vehicleid + "日期为" + date + "的result无数据文件可读取" + System.Environment.NewLine;
                                            }
                                        }
                                        else
                                        {
                                            _logger.LogInformation(vehicleid + "日期为" + date + "的result数据还没有上传完毕");
                                            importresult += vehicleid + "日期为" + date + "的result数据还没有上传完毕" + System.Environment.NewLine;
                                        }
                                    }

                                    else
                                    {
                                        _logger.LogInformation(vehicleid + "日期为" + date + "的result数据已有导入flag，不需要再次导入");
                                        importresult += vehicleid + "日期为" + date + "的result数据已有导入flag，不需要再次导入" + System.Environment.NewLine;
                                    }

                                }
                                _memoryCache.Compact(1.0);//一旦有新的数据导入到数据库中，就执行一次内存重置，防止还用之前的缓存来展示数据
                            }
                            catch (Exception ex)
                            {

                                _logger.LogInformation(ex + "错误发生在" + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
                            }
                            importresult = importresult.TrimEnd(new char[] { '\r', '\n' });
                            return importresult;
                        }
                        else
                        {
                            return vehicleid + "日期选择为空";

                        }
                    }
                    else
                    {
                        return vehicleid + "车辆无数据导入";
                    }
                }
                else
                {
                    return "未开启允许导入设置";
                }
            }
            else
            {
                return "车辆配置表中未填写此车辆号"+ vehicleid;
            }
          

        }



        //数据库中查询出最新数据的日期返回给前端显示
        public string FinishedDate(string vehicleid)
        {
            if (vehilceimport.Contains(vehicleid))
            {
                var isnull = _IAnalysisData_ACC_Service.LoadEntities(a => a.VehicleId == vehicleid).OrderBy(a => a.Datadate).Select(a => a.Datadate).LastOrDefault();
                if (isnull != null)
                {
                    var finisheddate = (DateTime)_IAnalysisData_ACC_Service.LoadEntities(a => a.VehicleId == vehicleid).OrderBy(a => a.Datadate).Select(a => a.Datadate).LastOrDefault();
                    return finisheddate.AddDays(1).ToShortDateString();
                }
                else
                {
                    return "此车辆目前还没有数据";
                }

            }
            else
            {
                return "此车辆不在配置文件中";
            }
        }


    }
}
