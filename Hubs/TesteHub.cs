using System.IdentityModel.Tokens.Jwt;
using heystock.data;
using heystock.helper;
using heystock.Interfaces;
using heystock.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using testingStuff.models;

namespace heystock.hubs;

[Authorize]
class TesteHub : Hub
{

    private readonly DbDataContext _context;
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public TesteHub(DbDataContext context, IConfiguration config, IUserRepository userRepository)
    {
        _context = context;
        _configuration = config;
        _userRepository = userRepository;
    }

    public async Task refresh_users(){
        var users = _userRepository.getUsers();
        ICollection<SafeUser> safeUsers = [];
        foreach (var user in users)
        {
            safeUsers.Add(new SafeUser{
                id = user.id,
                UserName = user.UserName,
                isAdmin = user.isAdmin
            });
        }
        await Clients.Client(Context.ConnectionId).SendAsync("RefreshedUserList", safeUsers);
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

}