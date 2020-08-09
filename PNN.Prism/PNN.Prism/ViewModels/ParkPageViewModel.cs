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

namespace PNN.Prism.ViewModels
{
    public class ParkPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private ObservableCollection<ZoneItemViewModel> _zones;
        private ParkResponse _park;
        private DelegateCommand _likeCommand;
        private DelegateCommand _disLikeCommand;
        private readonly IApiService _apiService;
        private bool _isRefreshing;


        public ParkPageViewModel(INavigationService navigationService,
                                 IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Parque";
            IsRefreshing = false;
                       
        }


        public DelegateCommand SelectLikeCommand => _likeCommand ?? (_likeCommand = new DelegateCommand(Like));
        public DelegateCommand SelectDisLikeCommand => _disLikeCommand ?? (_disLikeCommand = new DelegateCommand(DisLike));

        public ParkResponse Park
        {
            get => _park;
            set => SetProperty(ref _park, value);
        }

        public bool IsRefreshing //IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("Park"))
            {
                Park = parameters.GetValue<ParkResponse>("Park");
            }

            Title = $"{Park.Name}";

            LoadZones();

        }

        public ObservableCollection<ZoneItemViewModel> Zones
        {
            get => _zones;
            set => SetProperty(ref _zones, value);

        }

        private void LoadZones()
        {


            Zones = new ObservableCollection<ZoneItemViewModel>(Park.Zones.Select(c => new ZoneItemViewModel(_navigationService)
            {
                Id = c.Id,
                Comments = c.Comments,
                DisLike = c.DisLike,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                Like = c.Like,
                Location = c.Location,
                Nombre = c.Nombre,
                Manager = c.Manager,
                ManagerId = c.ManagerId,
                ZoneType = c.ZoneType
            }).ToList());
        }

        private async void Like()
        {
            var rcts = JsonConvert.DeserializeObject<List<Reactions>>(Settings.Reactions);
            var user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);

            var rct= rcts == null ? null : rcts.FirstOrDefault(r => r.ParkId == Park.Id && r.UserId == user.Id);

            if (rct != null)
            {
                if (rct.Tipo == 2)
                {
                    await App.Current.MainPage.DisplayAlert("parque", "Ya has seleccionado que te gusta anteriormente.", "Aceptar");
                    return;
                }

                if (rct.Tipo == 1)
                {
                    rcts.Remove(rct);
                    rct.Tipo = 2;

                    Park.Like++;
                    Park.DisLike--;
                }

            }
            else 
            {
                rct = new Reactions
                {
                    Id = rcts == null ? 1 : rcts.Last().Id + 1, 
                    ParkId = Park.Id,
                    Tipo = 2,
                    UserId = user.Id
                };
                Park.Like++;
            }

            if (rcts == null)
                rcts = new List<Reactions>();

            rcts.Add(rct);
            Settings.Reactions = JsonConvert.SerializeObject(rcts);

            ActPark();

            await PubsPageViewModel.GetInstance().UpdateContentAsync();

            
            LoadZones();
        }

        private async void DisLike()
        {
            var rcts = JsonConvert.DeserializeObject<List<Reactions>>(Settings.Reactions);
            var user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);

            var rct = rcts == null ? null : rcts.FirstOrDefault(r => r.ParkId == Park.Id && r.UserId == user.Id);

            if (rct != null )
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
                    
                    Park.DisLike++ ;
                    Park.Like--;
                }
            }
            else
            {
                rct = new Reactions
                {
                    Id = rcts == null ? 1 : rcts.Last().Id + 1,
                    ParkId = Park.Id,
                    Tipo = 1,
                    UserId = user.Id
                };
                Park.DisLike++;
            }

            if (rcts == null)
                rcts = new List<Reactions>();

            rcts.Add(rct);
            Settings.Reactions = JsonConvert.SerializeObject(rcts);

            ActPark();

            await PubsPageViewModel.GetInstance().UpdateContentAsync();

            LoadZones();
        }

        private async void ActPark() 
        {
            var url = App.Current.Resources["UrlAPI"].ToString();
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            var user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
            
            var request = new ParkRequest
            {
                Id = Park.Id,
                dislike = Park.DisLike,
                like = Park.Like
            };

            var response = await _apiService.PutAsync(
                    url,
                    "/api",
                    "/Park",
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

    }
}
