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
    public class ParksItemViewModel: ParkResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectParkCommand;

        public ParksItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectParkCommand => _selectParkCommand ?? (_selectParkCommand = new DelegateCommand(DetalleAsync));

        private async void DetalleAsync()
        {
            var parameters = new NavigationParameters
                {
                    {"Park", this}
                };

            Settings.ParkId = JsonConvert.SerializeObject(this.Id.ToString());

            await _navigationService.NavigateAsync("ParkPage", parameters);
 
        }
    }  
}
