
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Threading.Tasks;

namespace RLDA_VehicleData_Watch.Models
{
    /// <summary>
    /// 自定义中间件，用于静态文件的鉴权授权
    /// </summary>
    public class AuthorizeStaticFilesMiddleware
    {
        private readonly RequestDelegate _next;
        

        public AuthorizeStaticFilesMiddleware(RequestDelegate next)
        {
            _next = next;
           
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Session.GetString("UserID")=="Admin"|| context.Session.GetString("UserID") == "User")
            {

                await _next(context);

            }

          
            else
            {
                context.Response.Redirect("/Home/Error");
            }
        }

    }
}
