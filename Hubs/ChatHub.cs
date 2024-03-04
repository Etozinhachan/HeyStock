using heystock.helper;
using heystock.Interfaces;
using heystock.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace heystock.hubs;


public class ChatHub : Hub
{


    public async Task SendMessage(string user, string message, IUserRepository userRepository)
    {
        try
        {
            // Perform the operation
            if (IsUserAuthorized(user))
            {
                /* userRepository.AddUser(new User{
                    UserName = "Eto_chan2345",
                    passHash = "4560291",
                }); */

                //var userDb = userRepository.getUser("Eto_chan2345")!;

                await Clients.All.SendAsync("ReceiveMessage", user, /*$"{message} \n {userDb.UserName} \n {userDb.passHash}"*/message);
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

    public async Task incrementCounter(string user, int currentNumber)
    {
        try
        {
            // Perform the operation
            if (IsUserAuthorized(user))
            {
                /* userRepository.AddUser(new User{
                    UserName = "Eto_chan2345",
                    passHash = "4560291",
                }); */

                

                await Clients.All.SendAsync("ReceiveCounterMessage", user, currentNumber + 1);
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