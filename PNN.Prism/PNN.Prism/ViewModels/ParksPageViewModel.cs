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
    public class ParksPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private PublicationsResponse _Ps;
        private ObservableCollection<ContentItemViewModel> _parks;

        public ParksPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
