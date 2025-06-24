using Microsoft.AspNetCore.SignalR;

public class GameHub : Hub
{
    public async Task SendPlayerAction(string action, object data)
    {
        await Clients.Others.SendAsync("ReceivePlayerAction", action, data);
    }

    public async Task JoinRoom(string roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).SendAsync("PlayerJoined", Context.ConnectionId);
    }

    public async Task LeaveRoom(string roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        await Clients.Group(roomId).SendAsync("PlayerLeft", Context.ConnectionId);
    }
}