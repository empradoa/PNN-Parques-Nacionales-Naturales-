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
    public class ContentItemViewModel: ContentResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectPubCommand;

        public ContentItemViewModel(INavigationService navigationService) 
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectPubCommand => _selectPubCommand ?? (_selectPubCommand = new DelegateCommand(DetalleAsync));

        private async void DetalleAsync()
        {
            var parameters = new NavigationParameters
            {
                {"Pub", this}
            };

            Settings.PubId = JsonConvert.SerializeObject(this.Id.ToString());

            await _navigationService.NavigateAsync("PubPage", parameters);
 
        }
    }
}
