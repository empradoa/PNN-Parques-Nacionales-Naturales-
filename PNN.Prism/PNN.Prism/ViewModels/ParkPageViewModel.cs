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
    public class ParkPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private ObservableCollection<ZoneItemViewModel> _zones;
        private ParkResponse _park;
        

        public ParkPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Parque";
                       
        }

                
        public ParkResponse Park
        {
            get => _park;
            set => SetProperty(ref _park, value);
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

    }
}
