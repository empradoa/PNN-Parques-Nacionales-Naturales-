using Prism;
using Prism.Ioc;
using PNN.Prism.ViewModels;
using PNN.Prism.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PNN.Common.Services;
using PNN.Common.Models;
using PNN.Common.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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

            TokenResponse token = new TokenResponse();
           
            if(Settings.Token != "Token")
            token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            Settings.Inicio = true;
            Settings.NLoc = true;



            if (Settings.Reactions == "Reactions")
            {
                var a = new List<Reactions>();
                Settings.Reactions = JsonConvert.SerializeObject(a);
            }

            if (Settings.IsRemembered && token?.Expiration > DateTime.Now)
            {
               await NavigationService.NavigateAsync("/CnpMasterDetailPage/NavigationPage/PubsPage");
            }
            else
            {
                await NavigationService.NavigateAsync("NavigationPage/InitialPage");
            }

        }

       
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.Register<IGeolocatorService, GeolocatorService>();
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
            containerRegistry.RegisterForNavigation<ChangePasswordPage, ChangePasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<RememberPasswordPage, RememberPasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<ZonePage, ZonePageViewModel>();
            containerRegistry.RegisterForNavigation<NotificationPage, NotificationPageViewModel>();

            containerRegistry.RegisterForNavigation<AddPubPage, AddPubPageViewModel>();
        }
    }
}
