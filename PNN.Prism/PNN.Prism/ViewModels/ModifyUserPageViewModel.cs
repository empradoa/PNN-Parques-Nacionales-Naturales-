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
using System.Threading.Tasks;

namespace PNN.Prism.ViewModels
{
    public class ModifyUserPageViewModel : ViewModelBase
    {
        private bool _isRunning;
        private bool _isEnabled;
        private UserResponse _user;
        private DelegateCommand _saveCommand;
        private DelegateCommand _changePasswordCommand;

        private readonly INavigationService _navigationServices;
        private readonly IApiService _apiServices;

        public  ModifyUserPageViewModel(INavigationService navigationServices,
                                        IApiService apiServices ) : base(navigationServices)
        {
            _navigationServices = navigationServices;
            _apiServices = apiServices;
            Title = "User";
            User = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            IsEnabled = true;
            ValidateUser();

        }


        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(SaveAsync));

        public DelegateCommand ChangePasswordCommand => _changePasswordCommand ?? (_changePasswordCommand = new DelegateCommand(ChangePassword));

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private async void SaveAsync()
        {
            var isValid = await ValidateDataAsync();
            if (!isValid)
            {
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var userRequest = new UserRequest
            {
                Address = User.Address,
                Email = User.Email,
                FirstName = User.FirstName,
                LastName = User.LastName,
                Password = "12345678", // It doesn't matter what is sent here. It is only for the model to be valid
                CellPhone = User.CellPhone
            };

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var url = App.Current.Resources["UrlAPI"].ToString();
            var response = await _apiServices.PutAsync(
                url,
                "/api",
                "/Account",
                userRequest,
                "bearer",
                token.Token);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    response.Message,
                    "Accept");
                return;
            }

            Settings.User = JsonConvert.SerializeObject(User);

            await App.Current.MainPage.DisplayAlert(
                "Ok",
                "User updated sucessfully.",
                "Accept");
        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(User.CellPhone))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must to enter a document.",
                    "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(User.FirstName))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must to enter a first name.",
                    "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(User.LastName))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must to enter a last name.",
                    "Accept");
                return false;
            }

            if (string.IsNullOrEmpty(User.Address))
            {
                await App.Current.MainPage.DisplayAlert(
                    "Error",
                    "You must to enter an address.",
                    "Accept");
                return false;
            }

            return true;
        }

        private async void ChangePassword()
        {
            await _navigationServices.NavigateAsync("ChangePasswordPage");
        }


        public async void ValidateUser()
        {
            if (User.Email == "visit@hotmail.com")
            {


                await App.Current.MainPage.DisplayAlert("Alerta", "Para modificar sus datos debe iniciar Sesion Con una Cuenta diferente a la de invitado.", "Aceptar");

                await _navigationServices.NavigateAsync("/CnpMasterDetailPage/NavigationPage/PubsPage");
            }
        }
      
    }

}

