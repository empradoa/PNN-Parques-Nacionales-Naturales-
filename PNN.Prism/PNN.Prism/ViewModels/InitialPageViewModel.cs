using PNN.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PNN.Prism.ViewModels
{
    public class InitialPageViewModel : ViewModelBase
    {
        private bool _isEnabled;
        private DelegateCommand _loginCommand;
        private DelegateCommand _invCommand;
        private readonly INavigationService _navigationService;

        public InitialPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            Title = "Home";
            IsEnabled = true;
        }

        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(Initial));
        public DelegateCommand InvCommand => _invCommand ?? (_invCommand = new DelegateCommand(Invitado));

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private async void Initial()
        {
            await _navigationService.NavigateAsync("LoginPage");
        }

        private async void Invitado()
        {
                        
            var parameters = new NavigationParameters
            {
                {"User",new UserResponse
                    {
                        FirstName = "Invitado",
                        LastName = "Aprobed",
                        Address = "av nunca calle siempre",
                        Email = "Invitado@cnp.org",
                        CellPhone = "312 000 11 22",
                        Contents = new List<ContentResponse>()

                    }
                }
            };

            await _navigationService.NavigateAsync("PubsPage",parameters);
        }
    }
}
