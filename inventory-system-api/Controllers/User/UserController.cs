using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.Models;
using inventory_system_api.Application.Models.System;
using inventory_system_api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace inventory_system_api.Controllers
{
    [ApiController]
    public class UserController : BaseController
    {
        private IConfiguration _config;
        private IUserRepository _repo;

        public UserController(IConfiguration config, IUserRepository repo)
        {
            _config = config;
            _repo = repo;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUser login)
        {
            IActionResult response = null;

            // Authenticate the user
            User user = await _repo.VerifyAndGetUserDetails(login.UserName, login.Password); //IsAuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = OkResponse(new
                {
                    Token = tokenString
                });
            }
            else
            {
                response = Unauthorized(new Response() { StatusCode = 401, Message = "Invalid username or password", });
            }

            return response;
        }

        [HttpPut]
        public async Task<IActionResult> Put(User entity)
        {
            if (await _repo.ValidatePassword(entity.ID, entity.Password))
            {
                return BadRequest(new Response() { StatusCode = 400, Message = "Password does not match to the existing one", });
            }
            var res = await _repo.AddEdit(entity);
            return OkResponse();
        }

        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordModel entity)
        {
            
            await _repo.UpdatePassword(entity);
            return OkResponse();
        }

        [HttpPost]
        public async Task<IActionResult> Post(User entity)
        {
            int res = await _repo.AddEdit(entity);
            return OkResponse(new { ID = res });
        }

        private object GenerateJSONWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("Role", user.Role),
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

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var list = await _repo.Get();
            return OkResponse(list);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var entity = await _repo.Get(id);
            return OkResponse(entity);
        }

        
    }
}
