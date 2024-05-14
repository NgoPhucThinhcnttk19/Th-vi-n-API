using Microsoft.AspNetCore.Identity;
namespace Library_API_1.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
