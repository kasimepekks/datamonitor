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
        public static  string user;
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

            
            user = Context.User.Identity.Name;

            //将同一个人的连接ID绑定到同一个分组，推送时就推送给这个分组
            Groups.AddToGroupAsync(Context.ConnectionId, user);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _logger.LogInformation("靠，有人跑路了：{0}", this.Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}
