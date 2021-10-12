

//using Quartz;
//using Quartz.Impl;
//using System;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.Linq;
//using System.Threading.Tasks;

//namespace RLDA_VehicleData_Watch.Models
//{
//    public static class MyQuartzScheduler
//    {
//        public static bool TaskAlreadyRun { get; set; } = false;
//        public static async Task RunMonitor()
//        {
//            //try
//            //{
//            //    // Grab the Scheduler instance from the Factory  
//            //    NameValueCollection props = new NameValueCollection
//            //    {
//            //        { "quartz.serializer.type", "binary" }
//            //    };
//            //    StdSchedulerFactory factory = new StdSchedulerFactory(props);
//            //    IScheduler scheduler = await factory.GetScheduler();


//            //    // 启动任务调度器  
//            //    await scheduler.Start();


//            //    // 定义一个 Job  
//            //    IJobDetail job = JobBuilder.Create<MyQuartz>()
//            //        .WithIdentity("job1", "group1")
//            //        .Build();
//            //    ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
//            //        .WithIdentity("trigger1") // 给任务一个名字  
//            //        .StartAt(DateTime.Now) // 设置任务开始时间  
//            //        .ForJob("job1", "group1") //给任务指定一个分组  
//            //        .WithSimpleSchedule(x => x
//            //        .WithIntervalInSeconds(5)  //循环的时间 1秒1次 
//            //        .RepeatForever())
//            //        .Build();


//            //    // 等待执行任务  
//            //    await scheduler.ScheduleJob(job, trigger);


//            //    // some sleep to show what's happening  
//            //    //await Task.Delay(TimeSpan.FromMilliseconds(2000));  
//            //}
//            //catch (SchedulerException se)
//            //{
//            //    await Console.Error.WriteLineAsync(se.ToString());
//            //}

//            NameValueCollection props = new NameValueCollection
//                {
//                    { "quartz.serializer.type", "binary" }
//                };
//            StdSchedulerFactory factory = new StdSchedulerFactory(props);
//            IScheduler scheduler = await factory.GetScheduler();


//            // 启动任务调度器  
//            await scheduler.Start();


//            // 定义一个 Job  
//            IJobDetail job = JobBuilder.Create<MyQuartz>()
//                .WithIdentity("job2", "group2")
//                .Build();
//            ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
//                .WithIdentity("trigger1") // 给任务一个名字  
//                .StartAt(DateTime.Now) // 设置任务开始时间  
//                .ForJob("job2", "group2") //给任务指定一个分组  
//                .WithSimpleSchedule(x => x
//                .WithIntervalInSeconds(5)  //循环的时间 1秒1次 
//                .RepeatForever())
//                .Build();


//            // 等待执行任务  
//            await scheduler.ScheduleJob(job, trigger);

//            TaskAlreadyRun = true;
//        }
//    }
//}
