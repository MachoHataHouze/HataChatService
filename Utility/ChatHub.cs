using Microsoft.AspNetCore.SignalR;

namespace HataChatSerives.Utility;

public class ChatHub : Hub
{
    public async Task SendMessage(int senderId, int receiverId, string message, string timestamp)
    {
        await Clients.All.SendAsync("ReceiveMessage", senderId, receiverId, message, timestamp);
    }
}