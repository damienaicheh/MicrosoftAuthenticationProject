using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace MicrosoftDemoProject
{
    public class ViewModel : ViewModelBase
    {
        private readonly IMicrosoftAuthService microsoftAuthService;
        private bool isLoading;
        private User user;

        public ViewModel(IMicrosoftAuthService microsoftAuthService)
        {
            this.microsoftAuthService = microsoftAuthService;
            SignInCommand = new RelayCommand(async () => await SignInAsync());
            SignOutCommand = new RelayCommand(async () => await SignOutAsync());
        }

        public ICommand SignInCommand { get; }
        public ICommand SignOutCommand { get; }

        public bool IsLoading
        {
            get { return isLoading; }
            set { Set(ref isLoading, value); }
        }

        public User User
        {
            get { return user; }
            set { Set(ref user, value); }
        }

        public async Task SignInAsync()
        {
            try
            {
                this.IsLoading = true;
                this.User = await this.microsoftAuthService.OnSignInAsync();
                this.IsLoading = false;
            }
            catch (Exception ex)
            {
                // Manage the exception as you need, you can display an error message using a popup.
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }            
        }

        public async Task SignOutAsync()
        {
            try
            {
                this.IsLoading = true;
                await this.microsoftAuthService.OnSignOutAsync();
                // Remove the user on the view just for demo purpose
                this.User = null;
                this.IsLoading = false;
            }
            catch (Exception ex)
            {
                // Manage the exception as you need, you can display an error message using a popup.
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
    }
}
