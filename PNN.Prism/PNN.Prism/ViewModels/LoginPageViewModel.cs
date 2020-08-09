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
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private string _password;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isRemember;
        private DelegateCommand _loginCommand;
        private DelegateCommand _forgotPasswordCommand;

        public LoginPageViewModel(INavigationService navigationService,
                                  IApiService apiService  ): base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Login";
            IsEnabled = true;
            IsRemember = true;
        }

        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(Login));

        public DelegateCommand ForgotPasswordCommand => _forgotPasswordCommand ?? (_forgotPasswordCommand = new DelegateCommand(ForgotPassword));


        public string Email { get; set; }
        public string Password 
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public bool IsRemember 
        {
            get => _isRemember;
            set => SetProperty(ref _isRemember, value);
        }


        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning,value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(Email)) 
            {
                await App.Current.MainPage.DisplayAlert("Error","Digite un Email.","Aceptar");
                    return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Digite un Pasword.", "Aceptar");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var url = App.Current.Resources["UrlAPI"].ToString();
            var connection = await _apiService.CheckConnectionAsync(url);

            if (!connection) 
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error","Verifique su conexion a internet.","Aceptar");
                return;
            }

            var request = new TokenRequest {
                Password = Password,
                Username = Email
            };

            var response = await _apiService.GetTokenAsync(url,"/Account","/CreateToken",request);

            if (!response.IsSuccess) 
            {
                await App.Current.MainPage.DisplayAlert("Error", "Correo o Contraseña incorrecta.","Aceptar");
                Password = String.Empty;
                IsRunning = false;
                IsEnabled = true;

                return;
            }

            var token = response.Result;
            var response2 = await _apiService.GetOwnerByEmailAsync(url,"api","/Users/GetUserByEmail","bearer",token.Token,Email);

            if (!response2.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Problemas con los datos de Usuario, comuniquese con Soporte.", "Aceptar");
                Password = String.Empty;
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
            var user =  response2.Result;

            Settings.User = JsonConvert.SerializeObject(user);
            Settings.Token = JsonConvert.SerializeObject(token);
            Settings.Pubs = JsonConvert.SerializeObject(publics);
            Settings.Areas = JsonConvert.SerializeObject(publics.Areas.OrderBy(a => a.Park));
            Settings.IsRemembered = IsRemember;

           

            IsRunning = false;
            IsEnabled = true;

            
            await _navigationService.NavigateAsync("/CnpMasterDetailPage/NavigationPage/PubsPage");
        }

        private async void ForgotPassword()
        {
            await _navigationService.NavigateAsync("RememberPasswordPage");
        }

    }
}
