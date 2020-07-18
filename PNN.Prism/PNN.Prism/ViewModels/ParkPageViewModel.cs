using PNN.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PNN.Prism.ViewModels
{
    public class ParkPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
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

        }
    }
}
