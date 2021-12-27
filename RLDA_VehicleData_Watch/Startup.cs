using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BLL.SH_ADF0979BLL;
using Coravel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
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
using Newtonsoft.Json.Serialization;
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDbContext<datawatchContext>(options => options.UseMySql(connectionstring, serverVersion));
            services.AddControllersWithViews();
            //����
            //services.AddCors(option => option.AddPolicy("cors",
            //     policy => policy.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(_ => true)));

            services.AddSignalR(options =>
            {
                options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);//�ͻ��˷������������󵽷����������Ĭ��30�룬�ĳ�4���ӣ���ҳ���������connection.keepAliveIntervalInMilliseconds = 12e4;��2����
                options.KeepAliveInterval = TimeSpan.FromSeconds(15);//����˷������������󵽿ͻ��˼����Ĭ��15�룬�ĳ�2���ӣ���ҳ���������connection.serverTimeoutInMilliseconds = 24e4;��4����
            });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(30);
                options.Cookie.HttpOnly = true;
            });
            services.AddTransient<MyInvocable>();
            services.AddScheduler();

        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<MyAutofacModule>();
         

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            string MonitorRequired = Configuration["DataMonitor:MonitorRequired"];

            int result;

            bool stringtoint=int.TryParse(Configuration["DataMonitor:TimeGap"],out result);
           
            string[] MonitorVehicle = Configuration["DataMonitor:MonitoringVehicleID"].Split(";");
            if (MonitorRequired == "true")
            {
                if (stringtoint)
                {
                    var provider = app.ApplicationServices;
                    if (MonitorRequired == "true")
                    {
                        foreach (var i in MonitorVehicle)
                        {
                            provider.UseScheduler(scheduler =>
                            {
                                scheduler.ScheduleWithParams<MyInvocable>(i)
                                .EverySeconds(result)

                                .PreventOverlapping(i);
                            }).OnError((ex) =>
                               throw ex);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("�����ļ�TimeGap�������󣬲������ö�ʱ���񣡣���");
                }
            }
            else
            {
                Console.WriteLine("�����ļ�MonitorRequired�������󣬲������ö�ʱ���񣡣���");
            }
         


            //app.UseCors("cors");
            //app.UseCookiePolicy();
            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            //һ���ͻ��˷��ʴ�HTML�ľ�̬�ļ���ַ��ʹ���Զ�����м�����ж��Ƿ���sessionȨ�ޣ��оͼ�����û�оͷ���404
            app.UseWhen(
                          c => c.Request.Path.Value.Contains("Html"),
                          _ => _.UseMiddleware<AuthorizeStaticFilesMiddleware>());

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
