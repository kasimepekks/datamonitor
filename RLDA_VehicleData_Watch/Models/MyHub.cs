using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RLDA_VehicleData_Watch.Models
{
    public class MyHub:Hub
    {
        public static  List<string> user=new List<string>();
        private readonly ILogger<MyHub> _logger;
        public MyHub(ILogger<MyHub> logger)
        {
            _logger = logger;
        }
       
        public async Task SendMessage()
        {
            
            await Clients.All.SendAsync("ReceiveMessage");
        }

        public override  Task OnConnectedAsync()
        {
            if (user != null)
            {
                if (!user.Contains(Context.User.Identity.Name))
                {
                    user.Add(Context.User.Identity.Name);
                }
            }
            else
            {
                user.Add(Context.User.Identity.Name);
            }
            

            //将同一个人的连接ID绑定到同一个分组，推送时就推送给这个分组
            Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
           
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            
            _logger.LogInformation("此Signalr链接ID已断开：{0}", this.Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
