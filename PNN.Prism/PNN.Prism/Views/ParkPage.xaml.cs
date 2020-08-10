using Newtonsoft.Json;
using PNN.Common.Helpers;
using PNN.Common.Models;
using PNN.Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
            
            var Park = Areas.FindAll(a=> a.Park == ParkId);

            //var position = new Position(Park.Location.Latitude, Park.Location.Longitude);


            var path = new Polygon
            {
                StrokeWidth = 3,
                StrokeColor = Color.FromHex("#0AC4BA"),
                FillColor = Color.FromHex("#29CC85CA") //#FFFFFFNN FFFFFF= color NN= opacidad % hex
            };


            foreach (var area in Park)
            {
                        path.Geopath.Add(new Position(area.Location.Latitude, area.Location.Longitude));
            }

            MyMap.MapElements.Add(path);

            var clat = ((Park.First().Location.Latitude + Park.Last().Location.Latitude) / 2);
            var clon = ((Park.First().Location.Longitude + Park.Last().Location.Longitude) / 2);

            var center = new Position(clat, clon);
            

            MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(
                   center,
                   Distance.FromKilometers(50)));//distancia visual en km


           /* MyMap.Pins.Add(new Pin
            {     
                Label ="parque natural",
                Position = position,
                Type = PinType.Place
            });
            */
        }
    }
}
