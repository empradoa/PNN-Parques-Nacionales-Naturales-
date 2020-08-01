using Newtonsoft.Json;
using PNN.Common.Helpers;
using PNN.Common.Models;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PNN.Prism.ViewModels
{
     public class ZoneItemViewModel:ZoneResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _zoneCommand;

        public ZoneItemViewModel (INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand ZoneCommand => _zoneCommand ?? (_zoneCommand = new DelegateCommand(Zone));

        private async void Zone()
        {
            var parameters = new NavigationParameters
            {
                {"Zone", this}
            };

            Settings.ZoneId = JsonConvert.SerializeObject(this.Id.ToString());

            await _navigationService.NavigateAsync("ZonePage", parameters);
        }

    }
}
