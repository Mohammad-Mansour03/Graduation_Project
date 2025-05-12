using HojozatyCode.Pages;
using HojozatyCode.Services;
using System.Threading.Tasks;

namespace HojozatyCode
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            InitializeSupabaseAsync();
        }

        private async void InitializeSupabaseAsync()
        {
            await SupabaseConfig.InitializeAsync();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            return new Window(new MainPage());
        }
    }
}