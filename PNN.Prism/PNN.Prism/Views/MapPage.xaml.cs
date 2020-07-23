using Newtonsoft.Json;
using PNN.Common.Helpers;
using PNN.Common.Models;
using PNN.Common.Services;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;


namespace PNN.Prism.Views
{
    public partial class MapPage : ContentPage
    {
        private readonly IGeolocatorService _geolocatorService;
        private readonly IApiService _apiService;

        public MapPage(IGeolocatorService geolocatorService,
                       IApiService apiService )
        {
            InitializeComponent();
            _geolocatorService = geolocatorService;
            _apiService = apiService;
            MoveMapToCurrentPositionAsync();
            ShowAreasAsync();
        }

        private async void MoveMapToCurrentPositionAsync()
        {
            await _geolocatorService.GetLocationAsync();
            if (_geolocatorService.Latitude != 0 && _geolocatorService.Longitude != 0)
            {
                var position = new Position(
                    _geolocatorService.Latitude,
                    _geolocatorService.Longitude);
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                    position,
                    Distance.FromKilometers(50)));
            }
        }

        private async void ShowAreasAsync()
        {
            
            var areas = JsonConvert.DeserializeObject <List<AreaResponse>>(Settings.Areas);
           
            foreach (var area in areas)
            {                
                MyMap.Pins.Add(new Pin
                {
                    Label = area.Name,
                    Position = new Position(area.Location.Latitude, area.Location.Longitude),
                    Type = PinType.Place
                });

                
                /* MyMap.MapElements.Add(new Polygon 
                {  
                     
                    
                });*/
                
            }

        }

    }
}
