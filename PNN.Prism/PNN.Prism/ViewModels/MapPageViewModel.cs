using PNN.Common.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PNN.Prism.ViewModels
{
    public class MapPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationServices;

        public MapPageViewModel(INavigationService navigationServices) : base(navigationServices)
        {
            _navigationServices = navigationServices;
            Title = "Mapa";

            ubicacion();
        }

        private async void ubicacion()
        {

            if (Settings.NLoc) 
            {
                await App.Current.MainPage.DisplayAlert("Recuerda", "Debes Activar La Ubicacion para que el Mapa se vea correctamente.", "Aceptar");
                Settings.NLoc = false;
            }
        }
    }
}
