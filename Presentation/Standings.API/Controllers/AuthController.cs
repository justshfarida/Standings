using Microsoft.AspNetCore.Mvc;
using Standings.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Standings.API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService authService;

    public AuthController(IAuthService authService)
    {
        this.authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string usernameOrEmail, string password)
    {
        var data = await authService.LoginAsync(usernameOrEmail, password);
        return StatusCode(data.StatusCode, data);
    }

    [HttpPost("refresh-token-login")]
    public async Task<IActionResult> RefreshTokenLogin(string refreshToken)
    {
        var data = await authService.LoginWithRefreshTokenAsync(refreshToken);
        return StatusCode(data.StatusCode, data);
    }

    [HttpPut("logout")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, User")]
    public async Task<IActionResult> Logout(string usernameOrEmail)
    {
        var data = await authService.LogOut(usernameOrEmail);
        return StatusCode(data.StatusCode, data);
    }

    [HttpPut("reset-password")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin, User")]
    public async Task<IActionResult> ResetPassword(string email, string currentPassword, string newPassword)
    {
        var data = await authService.ResetPassword(email, currentPassword, newPassword);
        return StatusCode(data.StatusCode, data);
    }
}
