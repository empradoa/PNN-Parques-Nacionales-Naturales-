using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PNN.Prism.ViewModels
{
    public class ModifyUserPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationServices;

        public ModifyUserPageViewModel(INavigationService navigationServices) : base(navigationServices)
        {
            _navigationServices = navigationServices;
            Title = "User";
        }
    }
}
