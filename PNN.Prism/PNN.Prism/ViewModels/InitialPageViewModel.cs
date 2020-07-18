using Newtonsoft.Json;
using PNN.Common.Helpers;
using PNN.Common.Models;
using PNN.Common.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PNN.Prism.ViewModels
{
    public class InitialPageViewModel : ViewModelBase
    {
        private bool _isEnabled;
        private bool _isRunning;
        private DelegateCommand _loginCommand;
        private DelegateCommand _invCommand;
        private DelegateCommand _regCommand;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;

        public InitialPageViewModel(INavigationService navigationService,
                                    IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Home";
            IsEnabled = true;
        }

        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(Initial));
        public DelegateCommand InvCommand => _invCommand ?? (_invCommand = new DelegateCommand(Invitado));
        public DelegateCommand RegisterCommand => _regCommand ?? (_regCommand = new DelegateCommand(register));

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        private async void Initial()
        {
            await _navigationService.NavigateAsync("/NavigationPage/LoginPage");
        }

        private async void Invitado()
        {
            IsRunning = true;
            IsEnabled = false;

            var url = App.Current.Resources["UrlAPI"].ToString();
            var connection = await _apiService.CheckConnectionAsync(url);

            if (!connection)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", "Verifique su conexion a internet.", "Aceptar");
                return;
            }

            var request = new TokenRequest
            {
                Password = "123456",
                Username = "visit@hotmail.com"
            };

            var response = await _apiService.GetTokenAsync(url, "/Account", "/CreateToken", request);
            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Correo o Contraseña incorrecta.", "Aceptar");
                IsRunning = false;
                IsEnabled = true;

                return;
            }

            var token = response.Result;
            var response2 = await _apiService.GetOwnerByEmailAsync(url, "api", "/Users/GetUserByEmail", "bearer", token.Token, "visit@hotmail.com");

            if (!response2.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Problemas con los datos de Usuario, comuniquese con Soporte.", "Aceptar");
                IsRunning = false;
                IsEnabled = true;
                return;
            }

            var response3 = await _apiService.GetContentsAsync(url, "api", "/Content/GetContentsAsync", "bearer", token.Token);

            if (!response3.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Problemas con los contenidos, comuniquese con Soporte.", "Aceptar");
                IsRunning = false;
                IsEnabled = true;
                return;
            }

            var publics = response3.Result;
            var user = response2.Result;

            Settings.User = JsonConvert.SerializeObject(user);
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.Pubs = JsonConvert.SerializeObject(publics);

            IsRunning = false;
            IsEnabled = true;

            await _navigationService.NavigateAsync("/NavigationPage/PubsPage");
        }

        private async void register()
        {
            var parameters = new NavigationParameters
            {
                {"User", new UserRequest{} }

            };

            await _navigationService.NavigateAsync("/RegisterPage", parameters);
        }
    }
}
