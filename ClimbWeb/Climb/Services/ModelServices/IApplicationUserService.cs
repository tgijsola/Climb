using System.Threading.Tasks;
using Climb.Data;

namespace Climb.Services.ModelServices
{
    public interface IApplicationUserService
    {
        Task<ApplicationUser> GetByEmail(string email);
    }
}