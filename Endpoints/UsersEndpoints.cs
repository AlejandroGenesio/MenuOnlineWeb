using MenuOnlineUdemy.DTOs;
using MenuOnlineUdemy.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MenuOnlineUdemy.Endpoints
{
    public static class UsersEndpoints
    {
        public static RouteGroupBuilder MapUsers(this RouteGroupBuilder group)
        {
            group.MapPost("/register", Register);
            group.MapPost("/login", Login);

            return group;
        }

        static async Task<Results<Ok<AuthenticationResponseDTO>, BadRequest<IEnumerable<IdentityError>>>>
            Register(UsersCredentialDTO userCredentialDTO,
            [FromServices] UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            var user = new IdentityUser
            {
                UserName = userCredentialDTO.Email,
                Email = userCredentialDTO.Email
            };

            var result = await userManager.CreateAsync(user, userCredentialDTO.Password);

            if (result.Succeeded)
            {
                var responseCredentials = await CreateToken(userCredentialDTO, configuration);
                return TypedResults.Ok(responseCredentials);
            }
            else
            {
                return TypedResults.BadRequest(result.Errors);
            }
        }

        private async static Task<AuthenticationResponseDTO>
            CreateToken(UsersCredentialDTO userCredentialDTO, IConfiguration configuration)
        {
            var claims = new List<Claim>
            {
                new Claim("email", userCredentialDTO.Email),
                new Claim("any other value", "any other value for any other value claim")
            };

            var keys = AuthKeys.GetKey(configuration);
            var credentials = new SigningCredentials(keys.First(), SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddYears(1);

            var secureToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(secureToken);

            return new AuthenticationResponseDTO
            {
                Token = token,
                Expiration = expiration
            };
        }

        static async Task<Results<Ok<AuthenticationResponseDTO>, BadRequest<string>>>
            Login(
            UsersCredentialDTO usersCredentialDTO, [FromServices] SignInManager<IdentityUser> signInManager,
            [FromServices] UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            var user = await userManager.FindByEmailAsync(usersCredentialDTO.Email);

            if (user == null)
            {
                return TypedResults.BadRequest("Invalid login");
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, usersCredentialDTO.Password, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var authenticationResponse = await CreateToken(usersCredentialDTO, configuration);
                return TypedResults.Ok(authenticationResponse);
            }
            else
            {
                return TypedResults.BadRequest("Invalid login");
            }
        }
    }
}
