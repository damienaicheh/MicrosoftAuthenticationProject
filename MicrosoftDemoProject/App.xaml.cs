using GalaSoft.MvvmLight.Ioc;
using Xamarin.Forms;

namespace MicrosoftDemoProject
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Bootstraping
            SimpleIoc.Default.Register<IMicrosoftAuthService, MicrosoftAuthService>();
            SimpleIoc.Default.Register<ViewModel>();

            // Initialize the Public Client Application for Microssoft Authentication.
            SimpleIoc.Default.GetInstance<IMicrosoftAuthService>().Initialize();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
