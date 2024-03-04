using System.IdentityModel.Tokens.Jwt;
using heystock.data;
using heystock.helper;
using heystock.Interfaces;
using heystock.models;
using Microsoft.AspNetCore.SignalR;

namespace heystock.hubs;

class LoginHub : Hub
{

    private readonly DbDataContext _context;
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public LoginHub(DbDataContext context, IConfiguration config, IUserRepository userRepository){
        _context = context;
        _configuration = config;
        _userRepository = userRepository;
    }

    public async Task register_user(string username, string password){
        try
        {
            // Perform the operation
            /* if (IsUserAuthorized(username))
            { */

                (string hash, string salt) = Helper.HashPassword(password);
                
                Console.WriteLine(username);
                Console.WriteLine(password);

                _userRepository.AddUser(new User{
                   UserName = username,
                   passHash = hash,
                   salt = salt,
                   isAdmin = false
                });

                User user = _userRepository.getUser(username)!;

                var jwt = Helper.createJWT(user, _configuration, _context);


                await Clients.Client(Context.ConnectionId).SendAsync("RegisteredMessage", user, new JwtSecurityTokenHandler().WriteToken(jwt).ToString());
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

}