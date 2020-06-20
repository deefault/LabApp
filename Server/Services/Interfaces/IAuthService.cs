using LabApp.Server.Data.Models;

namespace LabApp.Server.Services.Interfaces
{
    public interface IAuthService
    {
        /// <returns>jwt token</returns>
        string Authenticate(UserIdentity user);
    }
}