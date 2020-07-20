using Prism;
using Prism.Ioc;
using PNN.Prism.ViewModels;
using PNN.Prism.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PNN.Common.Services;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PNN.Prism
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mjg2OTY1QDMxMzgyZTMyMmUzMFhvVWo3U2F1R3J4TUQwK1FuZGNWSUw2Y0hZcE9YazQwUjlKV0U1Y0pRcnM9");

            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/InitialPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<InitialPage, InitialPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<PubsPage, PubsPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<PubPage, PubPageViewModel>();
            containerRegistry.RegisterForNavigation<ModifyUserPage, ModifyUserPageViewModel>();
            containerRegistry.RegisterForNavigation<ParksPage, ParksPageViewModel>();
            containerRegistry.RegisterForNavigation<ParkPage, ParkPageViewModel>();
            containerRegistry.RegisterForNavigation<MapPage, MapPageViewModel>();
            containerRegistry.RegisterForNavigation<CnpMasterDetailPage, CnpMasterDetailPageViewModel>();
        }
    }
}
