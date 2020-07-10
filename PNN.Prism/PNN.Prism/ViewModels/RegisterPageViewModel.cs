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
    public class RegisterPageViewModel : ViewModelBase
    {
        private bool _isEnabled;
        private bool _isRunning;
        private string _address;
        private string _cellPhone;
        private string _email;
        private string _firstName;
        private string _lastName;
        private string _passverified;
        private string _password;
        private readonly IApiService _apiService;
        private readonly INavigationService _navigationService;
        private DelegateCommand _regCommand;
        private UserRequest _user;
        

        public RegisterPageViewModel(INavigationService navigationService,
                                        IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Registro";

        }

        public DelegateCommand RegistCommand => _regCommand ?? (_regCommand = new DelegateCommand(Register));

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

        public string Passverified
        {
            get => _passverified;
            set => SetProperty(ref _passverified, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string FirstName
        {
            get => _firstName;
            set => SetProperty(ref _firstName, value);
        }

        public string LastName
        {
            get => _lastName;
            set => SetProperty(ref _lastName, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        public string CellPhone
        {
            get => _cellPhone;
            set => SetProperty(ref _cellPhone, value);
        }

        public UserRequest User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("User"))
            {
                User = parameters.GetValue<UserRequest>("User");
            }

        }

        private async void Register()
        {
            if (string.IsNullOrEmpty(User.FirstName))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Digite un Nombre.", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(User.LastName))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Digite un Apellido.", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(User.CellPhone))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Digite un Numero Celular.", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(User.Email))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Digite un Email.", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Digite un Pasword.", "Aceptar");
                return;
            }

            if (string.IsNullOrEmpty(Passverified))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Digite La verificacion de Pasword.", "Aceptar");
                return;
            }


            if (Password != Passverified) 
            {
                await App.Current.MainPage.DisplayAlert("Error", "Los Passwords No Coinciden.", "Aceptar");
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var request = new UserRequest
            {
                Address = Address,
                CellPhone = CellPhone,
                Email = Email,
                FirstName = FirstName,
                LastName = LastName,
                Password = Password,
                
                
            };

            var url = App.Current.Resources["UrlAPI"].ToString();
            var response = await _apiService.RegisterUserAsync(
                url,
                "/api",
                "/Account/RegisterUser",
                request);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Accept");
                return;
            }

            await App.Current.MainPage.DisplayAlert("Ok", response.Message, "Accept");
            await _navigationService.GoBackAsync();

            //await _navigationService.NavigateAsync("LoginPage");
        }
    }
}
