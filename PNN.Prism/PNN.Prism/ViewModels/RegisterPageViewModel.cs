using PNN.Common.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PNN.Prism.ViewModels
{
    public class RegisterPageViewModel : ViewModelBase
    {
        
        private readonly IApiService _apiService;
        private readonly INavigationService _navigationService;
        private DelegateCommand _regCommand;

        public RegisterPageViewModel(INavigationService navigationService,
                                        IApiService apiService ) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = "Registro";

        }

        public DelegateCommand LoginCommand => _regCommand ?? (_regCommand = new DelegateCommand(Register));

        private async void Register()
        {
            await _navigationService.NavigateAsync("InitialPage");
        }
    }
}
