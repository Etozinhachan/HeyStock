using Microsoft.AspNetCore.SignalR;

namespace SignalRChat.Hubs;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        try
        {
            // Perform the operation
            if (IsUserAuthorized(user))
            {
                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
            else
            {
                // Handle unauthorized access
            }
        }
        catch (Exception ex)
        {
            // Handle the exception
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task SendMessageToGroup(string groupName, string user, string message)
    {
        await Clients.Group(groupName).SendAsync("ReceiveMessage", user, message);
    }

    public override async Task OnConnectedAsync()
    {
        // Log the connection
        Console.WriteLine($"Client {Context.ConnectionId} connected");
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        // Log the disconnection
        Console.WriteLine($"Client {Context.ConnectionId} disconnected");
    }

    private bool IsUserAuthorized(string user)
    {
        // Perform authorization check
        // Return true if the user is authorized, false otherwise
        return true;
    }
}