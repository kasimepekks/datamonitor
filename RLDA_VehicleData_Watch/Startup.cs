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
            //跨域
            //services.AddCors(option => option.AddPolicy("cors",
            //     policy => policy.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(_ => true)));

            services.AddSignalR(options =>
            {
                options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);//客户端发保持连接请求到服务端最长间隔，默认30秒，改成4分钟，网页需跟着设置connection.keepAliveIntervalInMilliseconds = 12e4;即2分钟
                options.KeepAliveInterval = TimeSpan.FromSeconds(15);//服务端发保持连接请求到客户端间隔，默认15秒，改成2分钟，网页需跟着设置connection.serverTimeoutInMilliseconds = 24e4;即4分钟
            });
            //services.AddSignalR();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(30);
                //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                //options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.HttpOnly = true;
            });

            //services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "DataProtection"));

            //services.AddTransient<MyInvocable>();
            //services.AddScoped<AutoCalCoravelJob>();
            services.AddScheduler();
            //services.AddQueue();



        }

        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<MyAutofacModule>();
         

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           // string MonitorRequired = Configuration["DataMonitor:MonitorRequired"];
           //string[] MonitorVehicle = Configuration["DataMonitor:MonitoringVehicleID"].Split(";");

           // var provider = app.ApplicationServices;
           // if (MonitorRequired == "true")
           // {
           //     foreach(var i in MonitorVehicle)
           //     {
           //         provider.UseScheduler(scheduler =>
           //         {
           //             scheduler.ScheduleWithParams<MyInvocable>(i)
           //             .EveryTenSeconds()

           //             .PreventOverlapping(i);


           //         }).OnError((ex) =>
           //            throw ex);
           //     }
                
            
           // }


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
            //一旦客户端访问带HTML的静态文件地址则使用自定义的中间件，判断是否有session权限，有就继续，没有就返回404
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
