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
    public partial class ParkPage : ContentPage
    {
        private readonly IGeolocatorService _geolocatorService;
        private readonly IApiService _apiService;

        public ParkPage(IGeolocatorService geolocatorService,
                        IApiService apiService)
        {
            InitializeComponent();
            _geolocatorService = geolocatorService;
            _apiService = apiService;
            ShowParkAsync();
            
        }

        private void ShowParkAsync()
        {
            var Areas= JsonConvert.DeserializeObject<List<AreaResponse>>(Settings.Areas);

            var ParkId = int.Parse(JsonConvert.DeserializeObject<string>(Settings.ParkId)); 
            
            var Park = Areas.Find(a=> a.Park == ParkId);

            var position = new Position(Park.Location.Latitude, Park.Location.Longitude);

            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                   position,
                   Distance.FromKilometers(10)));//distancia visual en km

            MyMap.Pins.Add(new Pin
            {     
                Label ="parque natural",
                Position = position,
                Type = PinType.Place
            });

        }
    }
}
