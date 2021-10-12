using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BLL.SH_ADF0979BLL;
using Coravel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MysqlforDataWatch;
using Pomelo.AspNetCore.TimedJob;

using RLDA_VehicleData_Watch.Controllers;
using RLDA_VehicleData_Watch.Models;
using Tools.MyAutofacModule;

namespace RLDA_VehicleData_Watch
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }

        public IConfiguration Configuration { get; }
        public IServiceProvider Services { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            var connectionstring = Configuration.GetConnectionString("MySqlConnection");
            var serverVersion = new MySqlServerVersion(new Version(5, 7, 20));
            var FilePath = new FilePath();
            Configuration.Bind("FilePath", FilePath);
            services.AddHttpContextAccessor();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option => 
            option.LoginPath = new PathString("/Home/Login"));
            //services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddDbContext<datawatchContext>(options => options.UseMySql(connectionstring));
            services.AddDbContext<datawatchContext>(options => options.UseMySql(connectionstring, serverVersion));
            services.AddControllersWithViews();
          
          
            services.AddSignalR();
            services.AddSession();
            //services.AddTimedJob();
            //services.AddScoped<DbContext, datawatchContext>();
            //services.AddScoped<IRealTimeI_ACC_IDAL, RealTime_ACC_DAL>();
            //services.AddScoped<IStatistic_ACC_IDAL, Statistic_ACC_DAL>();
            //services.AddScoped<IRealTimeI_WFT_IDAL, RealTime_WFT_DAL>();
            //services.AddScoped<IStatistic_WFT_IDAL, Statistic_WFT_DAL>();
            //services.AddScoped<IAnalysisData_ACC_IDAL, AnalysisData_ACC_DAL>();
            //services.AddScoped<IAnalysisData_WFT_IDAL, AnalysisData_WFT_DAL>();
            //services.AddScoped<ISpeedDistribution_ACC_IDAL, SpeedDistribution_ACC_DAL>();
            //services.AddScoped<IRealTime_ACC_IBLL, RealTime_ACC_BLL>();
            //services.AddScoped<IStatistic_ACC_IBLL, Statistic_ACC_BLL>();
            //services.AddScoped<IRealTime_WFT_IBLL, RealTime_WFT_BLL>();
            //services.AddScoped<IStatistic_WFT_IBLL, Statistic_WFT_BLL>();
            //services.AddScoped<IAnalysisData_ACC_IBLL, AnalysisData_ACC_BLL>();
            //services.AddScoped<IAnalysisData_WFT_IBLL, AnalysisData_WFT_BLL>();
            //services.AddScoped<ISpeedDistribution_ACC_IBLL, SpeedDistribution_ACC_BLL>();
            services.AddTransient<MyInvocable>();
            //services.AddScoped<AutoCalCoravelJob>();
            services.AddScheduler();
            //services.AddQueue();
            // services.AddSingleton<IJobFactory, SingletonJobFactory>();
            // services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();

            //services.AddSingleton<MyQuartz>();
            // services.AddSingleton(
            //      new JobSchedule(jobType: typeof(MyQuartzJob), cronExpression: "0/5 * * * * ?")
            //);

            // services.AddHostedService<QuartzHostedService>();
        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<MyAutofacModule>();
         

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            string MonitorRequired = Configuration["DataMonitor:MonitorRequired"];
            //string DailyisRequired = Configuration["DailyTimeJob:DailyisRequired"];
            //string AutoisRequired = Configuration["AutoTimeJob:AutoisRequired"];
            //int hour= int.Parse(Configuration["DailyTimeJob:hour"]);
            //int minute = int.Parse(Configuration["DailyTimeJob:minute"]);
            //string interval = Configuration["AutoTimeJob:Interval"];
            var provider = app.ApplicationServices;
            if (MonitorRequired == "true") { 
                provider.UseScheduler(scheduler =>
                {
                    scheduler.ScheduleWithParams<MyInvocable>("ADF0979")
                    .EveryTenSeconds()
                    
                    .PreventOverlapping("ADF0979");


                }).OnError((ex) =>
                   throw ex);
                provider.UseScheduler(scheduler =>
                {
                    scheduler.ScheduleWithParams<MyInvocable>("E21SIV161")
                    .EveryFiveSeconds()

                    .PreventOverlapping("ADF0979");


                }).OnError((ex) =>
                   throw ex);
            }

            //if (AutoisRequired == "true")
            //{
            //    switch (interval)
            //    {
            //        case "1":
            //            provider.UseScheduler(scheduler =>
            //            {
            //                scheduler.Schedule<AutoCalCoravelJob>()
            //               .EveryMinute()
            //                .PreventOverlapping("AutoCalCoravelJob");

            //            }).OnError((ex) =>

            //            throw ex
            //            );
            //            break;

            //        case "5":
            //            provider.UseScheduler(scheduler =>
            //            {
            //                scheduler.Schedule<AutoCalCoravelJob>()
            //               .EveryFiveMinutes()
            //                .PreventOverlapping("AutoCalCoravelJob");

            //            }).OnError((ex) =>

            //            throw ex
            //            );
            //            break;
            //        case "10":
            //            provider.UseScheduler(scheduler =>
            //            {
            //                scheduler.Schedule<AutoCalCoravelJob>()
            //               .EveryTenMinutes()
            //                .PreventOverlapping("AutoCalCoravelJob");

            //            }).OnError((ex) =>

            //            throw ex
            //            );
            //            break;
            //        case "15":
            //            provider.UseScheduler(scheduler =>
            //            {
            //                scheduler.Schedule<AutoCalCoravelJob>()
            //               .EveryFifteenMinutes()
            //                .PreventOverlapping("AutoCalCoravelJob");

            //            }).OnError((ex) =>

            //            throw ex
            //            );
            //            break;
            //        case "30":
            //            provider.UseScheduler(scheduler =>
            //            {
            //                scheduler.Schedule<AutoCalCoravelJob>()
            //               .EveryThirtyMinutes()
            //                .PreventOverlapping("AutoCalCoravelJob");

            //            }).OnError((ex) =>

            //            throw ex
            //            );
            //            break;

            //    }


            //}
            //string MonitorRequired = Configuration["DataMonitor:MonitorRequired"];
            //if (MonitorRequired == "true")
            //{
            //    app.UseTimedJob();
            //}

            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

          
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MyHub>("/MyHub");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Login}/{id?}");
               

            });
           
        }
    }
}
