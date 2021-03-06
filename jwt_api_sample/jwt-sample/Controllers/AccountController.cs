﻿using jwt_sample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace jwt_sample.Controllers
{
    public class AccountController : ControllerBase
    {
        IConfiguration _config;

        public AccountController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public IActionResult GetAuthenticated([FromBody] UserModel model)
        {
            if(string.IsNullOrEmpty(model.Username)==false
                && string.IsNullOrEmpty(model.Password) == false)
            {
                string tokenStr = GenerateToken(model);
                
                return Ok(new { Token = tokenStr, AppData.ExpiresIn });
            }

            return Unauthorized();
        }

        private string GenerateToken(UserModel user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            if (user.Dob.HasValue)
                claims.Add(new Claim(JwtRegisteredClaimNames.Birthdate, user.Dob.Value.ToShortDateString()));

            var tokenObj = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              claims,
#if DEBUG
              expires: DateTime.UtcNow.AddMinutes(AppData.ExpiresIn),
#else
              expires: DateTime.UtcNow.AddDays(AppData.ExpiresIn),
#endif
              signingCredentials: creds);


            var tokenStr = new JwtSecurityTokenHandler().WriteToken(tokenObj);
            return tokenStr;
        }
    }
}
