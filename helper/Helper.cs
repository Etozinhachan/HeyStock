using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using heystock.models;
using heystock.data;

namespace heystock.helper;

class Helper
{
    #region PasswordEncyption

        public static (string hash, string salt) HashPassword(string password)
        {
            // Generate a random salt
            byte[] saltBytes = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }

            string salt = Convert.ToBase64String(saltBytes);

            // Combine the password and salt, then hash
            using (var sha256 = SHA256.Create())
            {
                byte[] combinedBytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", "");

                return (hash, salt);
            }
        }

        public static string HashPassword(string password, string salt)
        {
            // Combine the password and salt, then hash
            using (var sha256 = SHA256.Create())
            {
                byte[] combinedBytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                string hash = BitConverter.ToString(hashBytes).Replace("-", "");

                return hash;
            }

        }

        #endregion

        #region JWT Generation
        public static SecurityToken createJWT(User user, IConfiguration _configuration, DbDataContext _context)
        {
            bool isAdmin = user.isAdmin;
            var userId = user.id;
            var configKey = _configuration["JwtSettings:Key"]!;
            var TokenLifeTime = TimeSpan.FromMinutes(30);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configKey);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, user.UserName),
                new(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()),
                new("UserID", userId.ToString()!),
                new("admin", isAdmin.ToString())
            };

            // foreach thingie!!!!


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TokenLifeTime),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            /*
                        var claims = new[] {
                                    new Claim(JwtRegisteredClaimNames.Sub, userDTO.UserName),
                                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                                    new Claim("UserId", userId.ToString()!),
                                    new Claim("UserName", userDTO.UserName)/*,
                                    new Claim("Email", user.Email)
                                };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configKey));
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["JwtSettings:Issuer"],
                            _configuration["JwtSettings:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddMinutes(10),
                            signingCredentials: signIn);
                        */
            return token;
        }
        #endregion
}