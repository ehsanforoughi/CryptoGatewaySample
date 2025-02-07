using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CryptoGateway.Domain.Entities.User;

namespace CryptoGateway.WebApi.Infrastructure;

public class MyController<T> : Controller where T : Controller
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<User> _userManager;

    public MyController(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public Serilog.ILogger Log => Serilog.Log.ForContext<T>();
    public string CurrentUserId => GetCurrentUser();

    private string GetCurrentUser()
    {
        var email = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
        if (email == null) return string.Empty;

        var user = _userManager.FindByEmailAsync(email).Result;
        if (user == null) return string.Empty;

        var currentUserId = user.UserExternalId.Value;
        return currentUserId;
    }
}