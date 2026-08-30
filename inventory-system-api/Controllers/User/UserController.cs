using inventory_system_api.Application.IRepository;
using inventory_system_api.Application.IService;
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
        private IUserService _repo;

        public UserController(IConfiguration config, IUserService repo)
        {
            _config = config;
            _repo = repo;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUser login, CancellationToken cancellationToken)
        {
            IActionResult response;

            // Authenticate the user
            User? user = await _repo.VerifyAndGetUserDetails(login.UserName, login.Password, cancellationToken); //IsAuthenticateUser(login);

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

        public async Task<IActionResult> Put(User entity, CancellationToken cancellationToken)
        {
            if (!await _repo.ValidatePassword(entity.ID, entity.Password, cancellationToken))
            {
                return BadRequest(new Response() { StatusCode = 400, Message = "Password does not match to the existing one", });
            }
            var res = await _repo.AddEdit(entity, UserId, cancellationToken);
            return OkResponse();
        }

        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordModel entity, CancellationToken cancellationToken)
        {
            
            await _repo.UpdatePassword(entity, UserId, cancellationToken);
            return OkResponse();
        }

        [HttpPost]
        public async Task<IActionResult> Post(User entity, CancellationToken cancellationToken)
        {
            int res = await _repo.AddEdit(entity, UserId, cancellationToken);
            return OkResponse(new { ID = res });
        }

        private object GenerateJSONWebToken(User user, int expiresIn =60)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]??""));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.ID.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("Roles", user.Role),
                new Claim("Date", DateTime.Now.ToString())
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(expiresIn),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var list = await _repo.Get(cancellationToken);
            return OkResponse(list);
        }

        [AllowAnonymous]
        [HttpPost("Login/Guest")]
        public IActionResult LoginGuest()
        {
            string userName = "GuestUser" + new Guid().ToString();

           var token = GenerateJSONWebToken(new User() { UserName = userName,Role="GUEST"}, 15);
            return OkResponse(new
            {
                Token = token
            });
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var entity = await _repo.Get(id, cancellationToken);
            return OkResponse(entity!);
        }

        
    }
}
