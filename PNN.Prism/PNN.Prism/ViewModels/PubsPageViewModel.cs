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

namespace PNN.Prism.ViewModels
{
    public class PubsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private PublicationsResponse _Ps;
        private ObservableCollection<ContentItemViewModel> _pubs;
        private UserResponse _user;
        private DelegateCommand _addPropertyCommand; 
        private static PubsPageViewModel _instance;
        private DelegateCommand _refreshCommand;
        private bool _isRefreshing;


        public PubsPageViewModel(INavigationService navigationService,
                                 IApiService apiService) : base(navigationService)
        {
            Title = "Publicaciones";
            _navigationService = navigationService;
            _apiService = apiService;
            _instance = this;
                        
            _user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            
            //Encuesta();

            LoadPubs();
        }

        public DelegateCommand AddPropertyCommand => _addPropertyCommand ?? (_addPropertyCommand = new DelegateCommand(AddProperty));

        public DelegateCommand RefreshCommand => _refreshCommand ?? (_refreshCommand = new DelegateCommand(RefreshContent));

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        private async void AddProperty()
        {
            if (_user.Email == "visit@hotmail.com")
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe Iniciar Sesion con una cuenta diferente a la de invitado", "Aceptar");
                return;
            }

            await _navigationService.NavigateAsync("AddPubPage");

        }

        public ObservableCollection<ContentItemViewModel> Pubs
        {
            get => _pubs;
            set => SetProperty(ref _pubs, value);

        }


        private async void LoadPubs()
        {
                      
            _Ps = JsonConvert.DeserializeObject<PublicationsResponse>(Settings.Pubs);

            Pubs = new ObservableCollection<ContentItemViewModel>(_Ps.Contents.Select(c => new ContentItemViewModel(_navigationService)
            {

                Comments = c.Comments,
                ContentType = c.ContentType,
                Date = c.Date,
                Description = c.Description,
                FullName = c.FullName,
                Id = c.Id,
                ImageUrl = c.ImageUrl,
                Like = c.Like,
                Park = c.Park == "Prefiero no registrar el parque" ? "": c.Park,
                UserAlias = c.UserAlias

            }).OrderByDescending(x => x.Date).ToList());

        }


        public async void Encuesta()
        {
            int t;

            if (Settings.IsRemembered && Settings.Inicio)
                await UpdateContentAsync();

            if (Settings.Inicio)
            {
                t = 420000;  //7 min
                Settings.Inicio = false;
            }
            else
                t = 1200000; // 20 minutos.

            await Task.Delay(t); //1000 es 1 seg

            await App.Current.MainPage.DisplayAlert("Bienvenido", "Por Favor Ayudanos a conocer Tu Experiencia ConParks.", "Aceptar");

            await _navigationService.NavigateAsync($"/CnpMasterDetailPage/NavigationPage/NotificationPage");
           
                 
        }

        public static PubsPageViewModel GetInstance()
        {
            return _instance;
        }

        public async Task UpdateContentAsync()
        {
            var url = App.Current.Resources["UrlAPI"].ToString();
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var response = await _apiService.GetOwnerByEmailAsync(
                url,
                "/api",
                "/Users/GetUserByEmail",
                "bearer",
                token.Token,
                _user.Email);

            if (response.IsSuccess)
            {
                var user = (UserResponse)response.Result;
                Settings.User = JsonConvert.SerializeObject(user);
                _user = user;
            }

            var response2 = await _apiService.GetContentsAsync(url, "api", "/Content/GetContentsAsync", "bearer", token.Token);

            if (response2.IsSuccess)
            {
                var publics = response2.Result;
                Settings.Pubs = JsonConvert.SerializeObject(publics);
                Settings.Areas = JsonConvert.SerializeObject(publics.Areas.OrderBy(a=>a.Park));
                LoadPubs();
            }

        }

        private async void RefreshContent()
        {
            IsRefreshing = true;
            
            await UpdateContentAsync();

            IsRefreshing = false;
        }



    }
}
