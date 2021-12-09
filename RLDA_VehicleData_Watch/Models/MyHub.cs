using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RLDA_VehicleData_Watch.Models
{
    public class MyHub:Hub
    {
        public static  string user;
        public async Task SendMessage()
        {
            
            await Clients.All.SendAsync("ReceiveMessage");
        }

        public override async Task OnConnectedAsync()
        {

             user = Context.User.Identity.Name;
            //将同一个人的连接ID绑定到同一个分组，推送时就推送给这个分组
            await Groups.AddToGroupAsync(Context.ConnectionId, user);
        }
    }
}
