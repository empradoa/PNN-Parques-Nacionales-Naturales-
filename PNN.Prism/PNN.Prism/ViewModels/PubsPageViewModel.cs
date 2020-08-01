﻿using Newtonsoft.Json;
using PNN.Common.Helpers;
using PNN.Common.Models;
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
        private PublicationsResponse _Ps;
        private ObservableCollection<ContentItemViewModel> _pubs;

        public PubsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Publicaciones";
            _navigationService = navigationService;
            LoadPubs();

            Encuesta();
        }

        
        public ObservableCollection<ContentItemViewModel> Pubs
        {
            get => _pubs;
            set => SetProperty(ref _pubs, value);

        }


        private void LoadPubs()
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
                Park = c.Park,
                UserAlias = c.UserAlias

            }).ToList());
        }


        public async void Encuesta()
        {
            await Task.Delay(90000); //1000 es 1 seg

            await App.Current.MainPage.DisplayAlert("Bienvenido", "Por Favor Ayudanos a conocer Tu Experiencia ConParks.", "Aceptar");

            await _navigationService.NavigateAsync($"/CnpMasterDetailPage/NavigationPage/NotificationPage");
           
                 
        }
    }
}
