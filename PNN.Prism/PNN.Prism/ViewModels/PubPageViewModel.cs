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
    public class PubPageViewModel : ViewModelBase
    {
        private bool _isRunning;
        private bool _isEnabled; 
        private bool _isUser; 
        private bool _isRefreshing;
        private bool _prkShow;
        private string _comment;
        private ContentResponse _content;
        private UserResponse _user;
        private ObservableCollection<CommentResponse> _comments;
        private DelegateCommand _commentCommand;
        private DelegateCommand _editPubCommand;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiServices;
        


        public PubPageViewModel(INavigationService navigationService,
                                IApiService apiServices ) : base(navigationService)
        {
            Title = "Publicacion";
            _navigationService = navigationService;
            _apiServices = apiServices;
            _user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            IsUser = false;
            PrkShow = true;
        }

        public DelegateCommand EditPubCommand => _editPubCommand ?? (_editPubCommand = new DelegateCommand(EditPub));

  
        public ContentResponse Content 
        { 
            get => _content;
            set => SetProperty(ref _content, value);  
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

        public bool PrkShow
        {
            get => _prkShow;
            set => SetProperty(ref _prkShow, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public bool IsUser
        {
            get => _isUser;
            set => SetProperty(ref _isUser, value);
        }


        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public DelegateCommand CommentCommand => _commentCommand ?? (_commentCommand = new DelegateCommand(Comentar));

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("Pub")) 
            {
                Content = parameters.GetValue<ContentResponse>("Pub");
            }


            if (Content.FullName == _user.FullName ||
                _user.Email.ToLower() == "empradoa@gmail.com".ToLower())
            {
                IsUser = true;
            }

            if (Content.Park == "Prefiero no registrar el parque") 
            {
                PrkShow = false;
            }

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

            int id;

            if (_content.Comments.Count() != 0)
            {
                id = ((_content.Comments.Last().Id) + 1);
            }
            else
                id = 1;

            var c = new CommentResponse{
                Id = id,
                Description = Comment,
                Date = DateTime.Now,
                Like = 0,
                FullName = _user.FullName,
                User = _user.Id
            };

            Comment = "";
                      
            _content.Comments.Add(c);
            LoadComments();

            await App.Current.MainPage.DisplayAlert(
                    "Comentario",
                    response.Message,
                    "Aceptar");
        }

        private void LoadComments()
        {
            Comments = new ObservableCollection<CommentResponse>(_content.Comments.OrderByDescending(x => x.Date));
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

        private async void EditPub()
        {
            var parameters = new NavigationParameters 
            {
                {"Content",Content}
            };
                        
                await _navigationService.NavigateAsync("AddPubPage", parameters);
            
        }

    }
}
