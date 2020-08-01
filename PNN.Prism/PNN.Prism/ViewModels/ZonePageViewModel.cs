using Newtonsoft.Json;
using PNN.Common.Helpers;
using PNN.Common.Models;
using PNN.Common.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PNN.Prism.ViewModels
{
    public class ZonePageViewModel : ViewModelBase
    {
        private bool _isRunning;
        private bool _isEnabled;
        private string _comment;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiServices;
        private ZoneResponse _zone;
        private UserResponse _user;
        private DelegateCommand _commentCommand;
        private ObservableCollection<CommentResponse> _comments;

        public ZonePageViewModel(INavigationService navigationService,
                                IApiService apiServices) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Zona";
            _apiServices = apiServices;
            _user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
        }

        public DelegateCommand CommentCommand => _commentCommand ?? (_commentCommand = new DelegateCommand(Comentar));

        public ZoneResponse Zone
        {
            get => _zone;
            set => SetProperty(ref _zone, value);
        }

        public ObservableCollection<CommentResponse> Comments
        {
            get => _comments;
            set => SetProperty(ref _comments, value);

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
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("Zone"))
            {
                Zone = parameters.GetValue<ZoneResponse>("Zone");
            }

            Title = $"{Zone.Nombre}";
            LoadComments();
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
                zone = Zone.Id, //lo coloco solo para pasar la validacion del modelo
                Content = default,
                User = _user.Id
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

            int id;

            if(_zone.Comments.Count() != 0 )
            {
                id = ((_zone.Comments.Last().Id) + 1); 
            } 
            else 
                id=1;

            var c = new CommentResponse
            {
                Id = id,
                Description = Comment,
                Date = DateTime.Now,
                Like = 0,
                FullName = _user.FullName,
                User = _user.Id
            };

            Comment = "";
            _zone.Comments.Add(c);
            LoadComments();

            await App.Current.MainPage.DisplayAlert(
                    "Comentario",
                    response.Message,
                    "Aceptar");
        }

        private void LoadComments()
        {
            Comments = new ObservableCollection<CommentResponse>(_zone.Comments);
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
