using System.IdentityModel.Tokens.Jwt;
using heystock.data;
using heystock.helper;
using heystock.Interfaces;
using heystock.models;
using Microsoft.AspNetCore.SignalR;

namespace heystock.hubs;

class LoginHub : Hub
{
        public async Task register_user(string username, string password, IUserRepository userRepository, IConfiguration config, DbDataContext context){
        try
        {
            // Perform the operation
            /* if (IsUserAuthorized(username))
            { */

                (string hash, string salt) = Helper.HashPassword(password);
                
                userRepository.AddUser(new User{
                   UserName = username,
                   passHash = hash,
                   salt = salt,
                   isAdmin = false
                });

                User user = userRepository.getUser(username)!;

                var jwt = Helper.createJWT(user, config, context);


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