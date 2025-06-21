using Domain.Contracts;
using Domain.JWT;
using DTO.UserDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthDomain _auth;

        public AuthController(IAuthDomain auth)
        {
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await _auth.RegisterAsync(dto);
                return Ok(new { message = "Registration successful. You will be able to log in after approval." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                var result = await _auth.LoginAsync(dto, ip);

                SetRefreshTokenCookie(result.RefreshToken, result.ExpiresAt);

                return Ok(new
                {
                    token = result.Token,
                    refreshToken = result.RefreshToken,
                    expiresAt = result.ExpiresAt
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(refreshToken))
                {
                    return Unauthorized(new { error = "Missing refresh token" });
                }
                refreshToken = System.Net.WebUtility.UrlDecode(refreshToken);


                var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                var result = await _auth.RefreshTokenAsync(refreshToken, ip);

                SetRefreshTokenCookie(result.RefreshToken, result.ExpiresAt);

                return Ok(new
                {
                    token = result.Token,
                    refreshToken = result.RefreshToken,
                    expiresAt = result.ExpiresAt
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            Console.WriteLine($"[Controller] RefreshToken from cookie: {refreshToken}");


            if (!string.IsNullOrEmpty(refreshToken))
            {
                refreshToken = Uri.UnescapeDataString(refreshToken);

                Console.WriteLine("LogoutAsync i thirrir me refresh token" + refreshToken);

                await _auth.LogoutAsync(refreshToken);
                Response.Cookies.Delete("refreshToken");
            }
            else
            {
                Console.WriteLine("RefreshToken null – kontrollo withCredentials në frontend!");

            }

            return Ok(new { message = "Logged out successfully" });
        }

        private void SetRefreshTokenCookie(string token, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,

                Expires = expires
            };

            //Response.Cookies.Append("refreshToken", token, cookieOptions);
            Response.Cookies.Append("refreshToken", Uri.EscapeDataString(token), cookieOptions);


        }



    }
}
