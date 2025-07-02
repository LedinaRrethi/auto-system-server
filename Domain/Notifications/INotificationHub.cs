using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Notifications
{
    public interface INotificationHub
    {
        Task SendNotification(Auto_Notifications notification, string connectionId);
    }
}
