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
    public class PubPageViewModel : ViewModelBase
    {
        private bool _isRunning;
        private bool _isEnabled;
        private string _comment;
        private ContentResponse _content;
        private UserResponse _user;
        private DelegateCommand _commentCommand;
        private readonly IApiService _apiServices;
        


        public PubPageViewModel(INavigationService navigationService,
                                IApiService apiServices ) : base(navigationService)
        {
            Title = "Publicacion";
            _apiServices = apiServices;
            _user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
        }

        public ContentResponse Content 
        { 
            get => _content;
            set => SetProperty(ref _content, value);  
        }

        public string Comment 
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
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

        public DelegateCommand CommentCommand => _commentCommand ?? (_commentCommand = new DelegateCommand(Comentar));

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("Pub")) 
            {
                Content = parameters.GetValue<ContentResponse>("Pub");
            }


            //CommentCommand
        }

        private async void Comentar()
        {

            IsRunning = true;
            IsEnabled = false;

            var isValid = await ValidateComment();
            if (!isValid)
            {
                return;
            }

            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var comment = new CommentRequest
            {
                Description = Comment,
                Date = DateTime.Now,
                zone = default, //lo coloco solo para pasar la validacion del modelo
                Content = Content.Id,                  
                User =_user.Id
            };

            var url = App.Current.Resources["UrlAPI"].ToString();
            var response = await _apiServices.AddComment(
                url,
                "/api",
                "/Comment",
                comment,
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

        }

        public async Task<bool> ValidateComment()
        {
            if (Comment?.Trim() == null) 
            {

                await App.Current.MainPage.DisplayAlert("Alerta", 
                    "Debe ingresar un comentario valido.", 
                    "Aceptar");

                return false;
            }

            if (_user.Email == "visit@hotmail.com")
            {
               await App.Current.MainPage.DisplayAlert("Alerta", 
                   "Para modificar sus datos debe iniciar Sesion Con una Cuenta diferente a la de invitado.", 
                   "Aceptar");

                return false;
            }

            return true;
        }
        
    }
}
