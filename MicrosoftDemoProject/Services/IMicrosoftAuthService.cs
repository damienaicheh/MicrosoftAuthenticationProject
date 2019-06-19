using System;
using System.Threading.Tasks;

namespace MicrosoftDemoProject
{
    public interface IMicrosoftAuthService
    {
        void Initialize();
        Task<User> OnSignInAsync();
        Task OnSignOutAsync();
    }
}
