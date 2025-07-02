using Entities.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Notifications
{
    public class NotificationHub : Hub<INotificationHub>
    {

        public static Dictionary<string, List<string>> ConnectedUsers = new();
        public async Task SendNotification(Auto_Notifications notification, string connectionId)
        {

            await Clients.Client(connectionId).SendNotification(notification, connectionId);
        }
        public string GetConnectionId(string userId)
        {
            lock (ConnectedUsers)
            {
                if (userId != null)
                {
                    if (!ConnectedUsers.ContainsKey(userId))
                        ConnectedUsers[userId] = new();
                    ConnectedUsers[userId].Add(Context.ConnectionId);

                    Console.WriteLine($" Connected: {userId} => {Context.ConnectionId}");

                }
            }
            return Context.ConnectionId;
        }

    }
}
