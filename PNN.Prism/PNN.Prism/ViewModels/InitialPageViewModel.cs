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

        public InitialPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Login";
            IsEnabled = true;
        }

        public DelegateCommand LoginCommand => _loginCommand ?? (_loginCommand = new DelegateCommand(Login));
        public DelegateCommand InvCommand => _invCommand ?? (_invCommand = new DelegateCommand(Invitado));

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        private async void Login()
        {            
            await App.Current.MainPage.DisplayAlert("Ok", "Fuck Yeah!!!", "Aceptar");
        }

        private async void Invitado()
        {
            await App.Current.MainPage.DisplayAlert("Ok", "Fuck Yeah!!!", "Aceptar");
        }
    }
}
