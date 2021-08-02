using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParkyApi.Data;
using ParkyApi.Models;
using ParkyApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ParkyApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ParkyApiContext _db;
        private readonly AppSettings _appSettings;

        public UserRepository(ParkyApiContext db, IOptions<AppSettings> appsettings)
        {
            _appSettings = appsettings.Value;
            _db = db;
        }
        public User Authenticate(string Username, string Password)
        {
            var user = _db.Users.SingleOrDefault(x => x.Username == Username && x.Password == Password);

            //when user is not found
            if (user==null)
            {
                return null;
            }

            //If user was found we will generate a jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim (ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = "";
            return user;
        }

        public bool IsUniqueUser(string Username)
        {
            throw new NotImplementedException();
        }
         
        public User Register(string Username, string Password)
        {
            throw new NotImplementedException();
        }
    }
}
