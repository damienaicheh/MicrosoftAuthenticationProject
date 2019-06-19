using System.ComponentModel;
using GalaSoft.MvvmLight.Ioc;
using Xamarin.Forms;

namespace MicrosoftDemoProject
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = SimpleIoc.Default.GetInstance<ViewModel>();
        }
    }
}
