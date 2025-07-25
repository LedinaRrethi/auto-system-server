﻿using Domain.Contracts;
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
            try
            {
                await _auth.RegisterAsync(dto);
                return Ok(new { message = "Registration successful. You will be able to log in after approval." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });

            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
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
                var msg = ex.Message.ToLowerInvariant();

                if (msg.Contains("pending") || msg.Contains("rejected"))
                    return StatusCode(StatusCodes.Status403Forbidden, new { error = ex.Message });

                if (msg.Contains("incorrect") || msg.Contains("invalid") || msg.Contains("does not exist"))
                    return Unauthorized(new { error = ex.Message });

                return BadRequest(new { error = "Login failed." });
            }

        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                //leximi i refresh otken nga cookie
                var refreshToken = Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(refreshToken))
                {
                    return Unauthorized(new { error = "Missing refresh token" });
                }
                refreshToken = System.Net.WebUtility.UrlDecode(refreshToken);


                var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                var result = await _auth.RefreshTokenAsync(refreshToken, ip);

                //vendos refresh otken e ri ne cookie
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

                await _auth.LogoutAsync(refreshToken);

                Response.Cookies.Append("refreshToken", "", new CookieOptions
                {
                    HttpOnly = true,
                    Secure = false, // true në prodhim
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(-1),
                    Path = "/"
                });

                //Response.Cookies.Delete("refreshToken");
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
                //Secure = true,
                //SameSite = SameSiteMode.None,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                //per deploy duhet secure=false , SamSite=lax

                Expires = expires,
                Path="/"
            };

            //Response.Cookies.Append("refreshToken", token, cookieOptions);
            Response.Cookies.Append("refreshToken", Uri.EscapeDataString(token), cookieOptions);


        }



    }
}
