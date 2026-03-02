using inventory_system_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace inventory_system_api.Controllers.User
{
    [ApiController]
    public class LoginController : BaseController
    {
        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] LoginUser login)
        {
            IActionResult response = null;
            bool isValid = IsAuthenticateUser(login);

            if (isValid)
            {
                var tokenString = GenerateJSONWebToken(login);
                response = OkResponse(new
                {
                    Token = tokenString
                });
            }
            else
            {
                response = Unauthorized(new Response() { StatusCode = 401, Message = "Invlid username or password", });
            }

                return response;
        }

        private object GenerateJSONWebToken(LoginUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("Date", DateTime.Now.ToString())
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private bool IsAuthenticateUser(LoginUser login)
        {
            if (login.UserName.ToLower() == "san" && login.Password == "san")
            {
                return true;
            }

            return false;
        }
    }
}
