using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Principal;

namespace AuthSample {

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase {

        [HttpGet("secret")]
        public ActionResult<string> GetRestrictedResource() {
            return "This message is top secret!";
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<string> AuthenticateUser([FromBody] Credentials creds) {
            if (UserVault.ContainsCredentials(creds.UserName, creds.Password)) {
                var key = SecurityService.GetSecurityKey();
                var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var identity = new ClaimsIdentity(new GenericIdentity(creds.UserName, "username"));

                var handler = new JwtSecurityTokenHandler();
                var token = handler.CreateToken(new SecurityTokenDescriptor() {
                    Issuer = SecurityService.GetIssuer(),
                    Audience = SecurityService.GetAudience(),
                    SigningCredentials = signingCredentials,
                    Subject = identity,
                    Expires = DateTime.Now.AddMinutes(10),
                    NotBefore = DateTime.Now
                });
                return handler.WriteToken(token);
            } else {
                return StatusCode(401);
            }
        }

        private IEnumerable<Claim> GetClaims() {
            return new List<Claim>() {
    new Claim("secret_access", "true"),
    new Claim("excellent_code", "true")
  };
        }
    }
}
