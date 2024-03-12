﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MovieMint.DTO;
using MovieMint.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MovieMint.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<AccountController> _logger;

        private readonly IConfiguration _configuration;

        private readonly UserManager<ApiUser> _userManager;

        private readonly SignInManager<ApiUser> _signInManager;

        public AccountController(
            ApplicationDbContext context,
            ILogger<AccountController> logger,
            IConfiguration configuration,
            UserManager<ApiUser> userManager,
            SignInManager<ApiUser> signInManager)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterDTO input)
        {
            var newUser = new ApiUser();
            newUser.UserName = input.UserName;
            newUser.Email = input.Email;
            newUser.PhoneNumber = input.PhoneNumber;

            var result = await _userManager.CreateAsync(
                        newUser, input.Password);
            if (result.Succeeded)
            {
                _logger.LogInformation(
                    $"User {newUser.UserName} with {newUser.Email} has been created.");
                return StatusCode(201,
                    $"User '{newUser.UserName}' has been created.");
            }
            else
                throw new Exception(
                    string.Format("Error: {0}", string.Join(" ",
                        result.Errors.Select(e => e.Description))));
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginDTO input)
        {
            var user = await _userManager.FindByNameAsync(input.UserName);
            if (user == null
                        || !await _userManager.CheckPasswordAsync(
                                user, input.Password))
                throw new Exception("Invalid login attempt.");
            else
            {
                var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(
                            _configuration["JWT:SigningKey"])),
                    SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>();
                claims.Add(new Claim(
                    ClaimTypes.Name, user.UserName));

                claims.AddRange(
                    (await _userManager.GetRolesAsync(user))
                        .Select(r => new Claim(ClaimTypes.Role, r)));

                var jwtObject = new JwtSecurityToken(
                    issuer: _configuration["JWT:Issuer"],
                    audience: _configuration["JWT:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddSeconds(300),
                    signingCredentials: signingCredentials);

                var jwtString = new JwtSecurityTokenHandler()
                    .WriteToken(jwtObject);

                return StatusCode(
                    StatusCodes.Status200OK,
                    jwtString);
            }
        }
    }
}
