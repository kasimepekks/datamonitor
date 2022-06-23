using Coravel.Invocable;
using IBLL.SH_ADF0979IBLL;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Tools;
using Tools.MyConfig;

namespace RLDA_VehicleData_Watch.Models
{
    //这里每次定时都会重新构造一次
    public class MyInvocableforAutoDataImport : IInvocable
    {
        private readonly IConfiguration _configuration;
        private readonly IAnalysisData_ACC_IBLL _IAnalysisData_ACC_Service;
        private readonly IAnalysisData_WFT_IBLL _IAnalysisData_WFT_Service;
        private readonly ILogger<MyInvocableforAutoDataImport> _logger;
        private static  VehicleIDPara VehicleIDPara;
        private readonly string inputpath;
        private readonly string resultpath;
        private readonly string _vehicleID;
        private readonly string _DataImportvehicleID;//配置文件里的可以导入数据的车辆
        private static Dictionary<string, string> pathdictionary = new Dictionary<string, string>();//存储每个车辆的读取文件的源路径
        private static Dictionary<string, List<string>> importeddirectory = new Dictionary<string, List<string>>();//存储每个车辆已导入的文件名

        private static Dictionary<string, VehicleIDPara> VehicleIDParadictionary = new Dictionary<string, VehicleIDPara>();

        public MyInvocableforAutoDataImport( IConfiguration configuration, IAnalysisData_ACC_IBLL IAnalysisData_ACC_Service, IAnalysisData_WFT_IBLL IAnalysisData_WFT_Service, ILogger<MyInvocableforAutoDataImport> logger,string vehicleID)
        {
            _configuration = configuration;
            _vehicleID = vehicleID;
            _logger = logger;
            _IAnalysisData_ACC_Service = IAnalysisData_ACC_Service;
            _IAnalysisData_WFT_Service = IAnalysisData_WFT_Service;
            inputpath = _configuration[_vehicleID + ":inputpathimport"];
            resultpath = _configuration[_vehicleID + ":resultpathimport"];

            //根据传进来的车辆号来初始化每个车辆号的各自的参数结构体
            VehicleIDPara=MyConfigforVehicleID.GetVehicleConfiguration(_vehicleID);

            _DataImportvehicleID = _configuration["DataImport:ImportVehicleID"];

            //把各自车辆的结构体参数存放到字典中进行保存，下次直接使用字典里的值即可
            if (VehicleIDParadictionary.Count != 0)
            {
                if (!VehicleIDParadictionary.ContainsKey(_vehicleID))
                {
                    VehicleIDParadictionary.Add(_vehicleID, VehicleIDPara);
                   
                }
            }
            else
            {
                VehicleIDParadictionary.Add(_vehicleID, VehicleIDPara);
                
            }

            if (pathdictionary.Count != 0)
            {
                if (!pathdictionary.ContainsKey(_vehicleID + "inputpath"))
                {
                    pathdictionary.Add(_vehicleID + "inputpath", inputpath);
                    pathdictionary.Add(_vehicleID + "outputpath", resultpath);
                }
            }
            else
            {
                pathdictionary.Add(_vehicleID + "inputpath", inputpath);
                pathdictionary.Add(_vehicleID + "outputpath", resultpath);
            }
            //先添加一个空字符串
            if (importeddirectory.Count != 0)
            {
                if (!importeddirectory.ContainsKey(_vehicleID + "importedinputpath"))
                {
                    List<string> a=new List<string>();
                    a.Add("");
                    importeddirectory.Add(_vehicleID + "importedinputpath", a);
                    importeddirectory.Add(_vehicleID + "importedoutputpath", a);
                }
            }
            else
            {
                List<string> a = new List<string>();
                a.Add("");
                importeddirectory.Add(_vehicleID + "importedinputpath", a);
                importeddirectory.Add(_vehicleID + "importedoutputpath", a);
            }

        }


        public async Task Invoke()
        {
            if (VehicleIDPara.channels != null)
            {
                if (_DataImportvehicleID != null)
                {
                    if (_DataImportvehicleID.Contains(_vehicleID))//先判断是否需要监控配置文件里需要监控的车辆，如配置文件里没有则不需要进行监控了
                    {

                        await Task.Run(async () =>
                        {
                            //获取所有的子文件，就是日期文件夹
                            var inputlist = FileOperator.GetSubDirectories(pathdictionary[_vehicleID + "inputpath"]);
                            //遍历所有子文件夹
                            foreach (var item in inputlist)
                            {
                                //先判断内存字典里是否有记录这一文件夹下的数据已经导入了，如果有就不需要再在这个文件夹下搜索flag，这样可以进一步提高搜索速度
                                if (!importeddirectory[_vehicleID + "importedinputpath"].Contains(item.Name))
                                {
                                    //判断子文件夹里是否有导入的flag，有则判断下一个子文件夹
                                    if (FileOperator.DataImportCompleteFlag(item))
                                    {
                                        _logger.LogInformation(_vehicleID + "日期为" + item.Name + "的input数据已有导入flag，不需要再次导入");
                                        //如果已经有flag了，就把这个文件夹名记录到内存字典中，下次再重新搜索时，就不需要在这个文件夹下重新判断是否还有flag，如果不做这步的话，每次定时导入数据时都需要每个文件夹下重新搜索一下有没有flag，影响性能
                                        importeddirectory[_vehicleID + "importedinputpath"].Add(item.Name);
                                        continue;
                                    }
                                    //没有则判断是否文件上传完毕
                                    else
                                    {

                                        if (FileOperator.DataTransferCompleteFlag(item))
                                        {
                                            var t1 = _IAnalysisData_ACC_Service.ReadandMergeACCDataperHalfHour(item.FullName, _vehicleID, VehicleIDParadictionary[_vehicleID]);
                                            t1.Wait();
                                            if (await t1)//把每个csv文件计算统计值并存入数据库中
                                            {
                                                _logger.LogInformation(_vehicleID + "日期为" + item.Name + "的input数据自动导入完成时间" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                                //创建导入完成的flag文件
                                                FileOperator.CreateDataImportCompleteFlag(item.FullName);
                                                importeddirectory[_vehicleID + "importedinputpath"].Add(item.Name);
                                            }
                                        }
                                        else
                                        {
                                            _logger.LogInformation(_vehicleID + "日期为" + item.Name + "的input数据还没有上传完毕");
                                        }
                                    }
                                }

                            }
                            var resultlist = FileOperator.GetSubDirectories(pathdictionary[_vehicleID + "outputpath"]);
                            foreach (var item in resultlist)
                            {
                                if (!importeddirectory[_vehicleID + "importedoutputpath"].Contains(item.Name))
                                {
                                    //判断子文件夹里是否有导入的flag，有则判断下一个子文件夹
                                    if (FileOperator.DataImportCompleteFlag(item))
                                    {
                                        _logger.LogInformation(_vehicleID + "日期为" + item.Name + "的result数据已有导入flag，不需要再次导入");
                                        importeddirectory[_vehicleID + "importedoutputpath"].Add(item.Name);
                                        continue;
                                    }
                                    else
                                    {
                                        if (FileOperator.DataTransferCompleteFlag(item))
                                        {
                                            var t2 = _IAnalysisData_WFT_Service.ReadandMergeWFTDataperHalfHour(item.FullName, _vehicleID, VehicleIDParadictionary[_vehicleID]);
                                            t2.Wait();
                                            if (await t2)//把每个csv文件计算统计值并存入数据库中
                                            {
                                                _logger.LogInformation(_vehicleID + "日期为" + item.Name + "的result数据自动导入完成时间" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                                FileOperator.CreateDataImportCompleteFlag(item.FullName);
                                                importeddirectory[_vehicleID + "importedoutputpath"].Add(item.Name);
                                            }
                                        }
                                        else
                                        {
                                            _logger.LogInformation(_vehicleID + "日期为" + item.Name + "的result数据还没有上传完毕");
                                        }
                                    }

                                }


                            }

                        });

                    }
                    else
                    {
                        _logger.LogInformation("配置表中数据导入项中未填写此车辆号" + _vehicleID);
                    }
                }
                else
                {
                    _logger.LogInformation("配置表中数据导入项中为空");
                }
            }
            else
            {
                _logger.LogInformation("车辆配置表中未填写此车辆号" + _vehicleID);
            }

           
            //return Task.CompletedTask;
        }
    }
}
