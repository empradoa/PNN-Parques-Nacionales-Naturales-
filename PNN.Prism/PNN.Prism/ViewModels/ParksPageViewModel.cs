using Newtonsoft.Json;
using PNN.Common.Helpers;
using PNN.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PNN.Prism.ViewModels
{
    public class ParksPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private PublicationsResponse _Ps;
        private ObservableCollection<ParksItemViewModel> _parks;

        public ParksPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Parques";
            LoadParks();
        }

        public ObservableCollection<ParksItemViewModel> Parks
        {
            get => _parks;
            set => SetProperty(ref _parks, value);

        }

        private void LoadParks()
        {
            _Ps = JsonConvert.DeserializeObject<PublicationsResponse>(Settings.Pubs);

            Parks = new ObservableCollection<ParksItemViewModel>(_Ps.Parks.Select(c => new ParksItemViewModel(_navigationService)
            {
                Been = c.Been,
                Creation = c.Creation,
                Communities = c.Communities,
                Contents = c.Contents,
                Description = c.Description,
                DisLike = c.DisLike,
                Extension = c.Extension,
                Flora = c.Flora,
                Height = c.Height,
                Id = c.Id,
                ImageUrl = c.ImageUrl,
                Like = c.Like,
                Location = c.Location,
                Manager = c.Manager,
                Name = c.Name,
                Temperature = c.Temperature,
                Wildlife = c.Wildlife,
                Zones = c.Zones
                
            }).ToList());
        }
    }
}
