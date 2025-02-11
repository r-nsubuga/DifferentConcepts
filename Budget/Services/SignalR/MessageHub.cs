using Microsoft.AspNetCore.SignalR;

namespace Budget.Services.SignalR;

public class MessageHub: Hub
{
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
    
    public async Task SubscribeToTopic(string routingKey)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, routingKey);
    }

    public async Task UnsubscribeFromTopic(string routingKey)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, routingKey);
    }
}