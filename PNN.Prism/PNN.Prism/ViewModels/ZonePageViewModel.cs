using PNN.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PNN.Prism.ViewModels
{
    public class ZonePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private ZoneResponse _zone;

        public ZonePageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Zona";
        }

        public ZoneResponse Zone
        {
            get => _zone;
            set => SetProperty(ref _zone, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("Zone"))
            {
                Zone = parameters.GetValue<ZoneResponse>("Zone");
            }

            Title = $"{Zone.Nombre}";

        }
    }
}
