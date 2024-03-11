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

class LoginHub : Hub
{

    private readonly DbDataContext _context;
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public LoginHub(DbDataContext context, IConfiguration config, IUserRepository userRepository)
    {
        _context = context;
        _configuration = config;
        _userRepository = userRepository;
    }

    public async Task register_user(string _userRegistration)
    {
        try
        {

            UserRegistration userRegistration = JsonConvert.DeserializeObject<UserRegistration>(_userRegistration);
            // Perform the operation
            /* if (IsUserAuthorized(username))
            { */

            if (_userRepository.UserExists(userRegistration.UserName))
            {
                await Clients.Client(Context.ConnectionId).SendAsync("LoginError", "A user with that username already exists.");
                return;
            }

            (string hash, string salt) = Helper.HashPassword(userRegistration.passHash);

            Console.WriteLine(userRegistration.UserName);
            Console.WriteLine(userRegistration.passHash);

            _userRepository.AddUser(new User
            {
                UserName = userRegistration.UserName,
                passHash = hash,
                salt = salt,
                isAdmin = false
            });

            User user = _userRepository.getUser(userRegistration.UserName)!;

            var jwt = Helper.createJWT(user, _configuration, _context);


            await Clients.Client(Context.ConnectionId).SendAsync("UserRegistered", new JwtSecurityTokenHandler().WriteToken(jwt).ToString());
            await Clients.Group("Admin").SendAsync("UserRegistered", user);
            await Clients.Others.SendAsync("RefreshUserList");
            /* userRepository.AddUser(new User{
                UserName = "Eto_chan2345",
                passHash = "4560291",
            }); */

            //var userDb = userRepository.getUser("Eto_chan2345")!;

            //await Clients.All.SendAsync("ReceiveMessage", user, /*$"{message} \n {userDb.UserName} \n {userDb.passHash}"*/message);
            /* }
            else
            {
                // Handle unauthorized access
            } */
        }
        catch (Exception ex)
        {
            // Handle the exception
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    public async Task login_user(string _userRegistration)
    {
        try
        {
            // Perform the operation
            /* if (IsUserAuthorized(username))
            { */
            UserRegistration userRegistration = JsonConvert.DeserializeObject<UserRegistration>(_userRegistration);
            Console.WriteLine("Jeorge");
            if (!_userRepository.UserExists(userRegistration.UserName))
            {
                Console.WriteLine("meow");
                await Clients.Client(Context.ConnectionId).SendAsync("LoginError", "No user found with that username.");
                return;
            }

            User searched_user = _userRepository.getUser(userRegistration.UserName)!;

            var salt = searched_user.salt;

            string hash = Helper.HashPassword(userRegistration.passHash, salt);

            if (hash != searched_user.passHash)
            {
                Console.WriteLine("rawr");
                await Clients.Client(Context.ConnectionId).SendAsync("LoginError", "Wrong password.");
                return;
            }
            Console.WriteLine("rahhh");
            var jwt = Helper.createJWT(searched_user, _configuration, _context);


            await Clients.Client(Context.ConnectionId).SendAsync("UserLogin", new JwtSecurityTokenHandler().WriteToken(jwt).ToString());
            /* userRepository.AddUser(new User{
                UserName = "Eto_chan2345",
                passHash = "4560291",
            }); */

            //var userDb = userRepository.getUser("Eto_chan2345")!;

            //await Clients.All.SendAsync("ReceiveMessage", user, /*$"{message} \n {userDb.UserName} \n {userDb.passHash}"*/message);
            /* }
            else
            {
                // Handle unauthorized access
            } */
        }
        catch (Exception ex)
        {
            // Handle the exception
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    [Authorize]
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