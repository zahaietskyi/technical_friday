using Common.Models;
using Microsoft.AspNetCore.SignalR;

namespace Web.Server.Hubs;

public class ChatHub : Hub
{
    public void Send(MessageDto message)
    {
        message.DateTimeMessage = DateTime.UtcNow;
        Clients.All.SendAsync("Get", message);
    }
}