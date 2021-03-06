﻿using Newtonsoft.Json;
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
        private bool _isRefreshing;
        private string _comment;
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiServices;
        private PublicationsResponse _Ps;
        private ZoneResponse _zone;
        private UserResponse _user;
        private DelegateCommand _commentCommand;
        private DelegateCommand _likeCommand;
        private DelegateCommand _disLikeCommand;
        private DelegateCommand _refreshCommand;
        private ObservableCollection<CommentItemViewModel> _comments;
        private static ZonePageViewModel _instance;


        public ZonePageViewModel(INavigationService navigationService,
                                IApiService apiServices) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Zona";
            _apiServices = apiServices;
            _user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            _instance = this;
        }

        public DelegateCommand CommentCommand => _commentCommand ?? (_commentCommand = new DelegateCommand(Comentar));

        public DelegateCommand SelectLikeCommand => _likeCommand ?? (_likeCommand = new DelegateCommand(Like));
        public DelegateCommand SelectDisLikeCommand => _disLikeCommand ?? (_disLikeCommand = new DelegateCommand(DisLike));

        public DelegateCommand RefreshCommand => _refreshCommand ?? (_refreshCommand = new DelegateCommand(RefreshZone));

        public ZoneResponse Zone
        {
            get => _zone;
            set => SetProperty(ref _zone, value);
        }

        public ObservableCollection<CommentItemViewModel> Comments
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

        public bool IsRefreshing //IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
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

        public void LoadComments()
        {
            Comments = new ObservableCollection<CommentItemViewModel>(_zone.Comments.Select(c => new CommentItemViewModel(_navigationService, _apiServices)
            {
                Id = c.Id,
                Description = c.Description,
                Date = c.Date,
                Like = c.Like,
                FullName = c.FullName,
                User = c.User
            }).ToList().OrderByDescending(x => x.Date));
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

        private async void Like()
        {
            var rcts = JsonConvert.DeserializeObject<List<Reactions>>(Settings.Reactions);
            var user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);

            var rct = rcts == null ? null : rcts.FirstOrDefault(r => r.ZoneId == Zone.Id && r.UserId == user.Id);

            if (rct != null)
            {
                if (rct.Tipo == 2)
                {
                    await App.Current.MainPage.DisplayAlert("Zona", "Ya has seleccionado que te gusta anteriormente.", "Aceptar");
                    return;
                }

                if (rct.Tipo == 1)
                {
                    rcts.Remove(rct);
                    rct.Tipo = 2;

                    Zone.Like++;
                    Zone.DisLike--;
                }

            }
            else
            {
                rct = new Reactions
                {
                    Id = rcts == null ? 1 : rcts.Last().Id + 1,
                    ZoneId = Zone.Id,
                    Tipo = 2,
                    UserId = user.Id
                };
                Zone.Like++;
            }

            rcts.Add(rct);
            Settings.Reactions = JsonConvert.SerializeObject(rcts);

            ActPark();

            RefreshZone();
        }

        private async void DisLike()
        {
            var rcts = JsonConvert.DeserializeObject<List<Reactions>>(Settings.Reactions);
            var user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);

            var rct = rcts==null? null :rcts.FirstOrDefault(r => r.ZoneId == Zone.Id && r.UserId == user.Id);

            if (rct != null)
            {
                if (rct.Tipo == 1)
                {
                    await App.Current.MainPage.DisplayAlert("parque", "Ya has seleccionado que no te gusta anteriormente.", "Aceptar");
                    return;
                }

                if (rct.Tipo == 2)
                {
                    rcts.Remove(rct);
                    rct.Tipo = 1;

                    Zone.DisLike++;
                    Zone.Like--;
                }
            }
            else
            {
                rct = new Reactions
                {
                    Id = rcts== null ? 1 : rcts.Last().Id + 1,
                    ZoneId = Zone.Id,
                    Tipo = 1,
                    UserId = user.Id
                };
                Zone.DisLike++;
            }

            if (rcts == null)
                rcts = new List<Reactions>();

            rcts.Add(rct);
            Settings.Reactions = JsonConvert.SerializeObject(rcts);

            ActPark();

            RefreshZone();

        }

        private async void ActPark()
        {
            var url = App.Current.Resources["UrlAPI"].ToString();
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            

            var request = new ZoneRequest
            {
                Id = Zone.Id,
                dislike = Zone.DisLike,
                like = Zone.Like
            };

            var response = await _apiServices.PutAsync(
                    url,
                    "/api",
                    "/Zone",
                    request.Id,
                    request,
                    "bearer",
                    token.Token);

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
        }

        private async void RefreshZone()
        {
            IsRefreshing = true;

            await PubsPageViewModel.GetInstance().UpdateContentAsync();
            _Ps = JsonConvert.DeserializeObject<PublicationsResponse>(Settings.Pubs);
            var parkid = JsonConvert.DeserializeObject<int>(Settings.ParkId);
            var park = _Ps.Parks.FirstOrDefault(p => p.Id == parkid);
            Zone = park.Zones.FirstOrDefault(z => z.Id == _zone.Id);

            IsRefreshing = false;
        }

        public static ZonePageViewModel GetInstance()
        {
            return _instance;
        }
    }
}
