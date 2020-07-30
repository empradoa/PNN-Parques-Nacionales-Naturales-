using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PNN.Prism.ViewModels
{
    public class NotificationPageViewModel : ViewModelBase
    {
        public NotificationPageViewModel(INavigationService navigationServices) : base(navigationServices)
        {
            Title = "Encuesta";
        }
    }
}
