using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Shared.Core.Interfaces.Services.Identity;

namespace Shared.Test.Mocks;

public class CurrentUser : ICurrentUser
{
    public string Name => "FirstUser";
    public Guid GetUserId() => Guid.NewGuid();

    public string GetUserEmail() => "test@test.com";

    public bool IsAuthenticated() => true;

    public bool IsInRole(string role) => role switch
    {
        "Staff" => true,
        _ => false
    };

    public IEnumerable<Claim> GetUserClaims() => new List<Claim>
    {
        new(ClaimTypes.Email, "test@test.com"),
        new(ClaimTypes.Surname, "Family"),
        new(ClaimTypes.Name, Name)
    };

    public HttpContext GetHttpContext() => new DefaultHttpContext();
}