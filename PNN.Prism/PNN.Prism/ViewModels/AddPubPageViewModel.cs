using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using PNN.Common.Helpers;
using PNN.Common.Models;
using PNN.Common.Services;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PNN.Prism.ViewModels
{
    public class AddPubPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private ContentResponse _content;
        private ImageSource _imageSource;
        private bool _isRunning;
        private bool _isEnabled;
        private bool _isEdit;
        private MediaFile _file;
        private DelegateCommand _changeImageCommand; 
        private DelegateCommand _saveCommand;
        private ObservableCollection<ContentTypeResponse> _contentTypes;
        private ContentTypeResponse _contentType;
        private ObservableCollection<ParkResponse> _parks;
        private ParkResponse _park;


        public AddPubPageViewModel(INavigationService navigationService,
                                   IApiService apiService) : base(navigationService)
        {
            IsEnabled = true;
            _navigationService = navigationService;
            _apiService = apiService;
        }

        public DelegateCommand ChangeImageCommand => _changeImageCommand ?? (_changeImageCommand = new DelegateCommand(Changeimage));
        
        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(SaveAsync));

        
        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEdit
        {
            get => _isEdit;
            set => SetProperty(ref _isEdit, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public ContentResponse Content
        {
            get => _content;
            set => SetProperty(ref _content, value);
        }

        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public ObservableCollection<ContentTypeResponse> ContentTypes
        {
            get => _contentTypes;
            set => SetProperty(ref _contentTypes, value);
        }

        public ContentTypeResponse ContentType
        {
            get => _contentType;
            set => SetProperty(ref _contentType, value);
        }

        public ObservableCollection<ParkResponse> Parks
        {
            get => _parks;
            set => SetProperty(ref _parks, value);
        }

        public ParkResponse Park
        {
            get => _park;
            set => SetProperty(ref _park, value);
        }


        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("Content"))
            {
                Content = parameters.GetValue<ContentResponse>("Content");
                ImageSource = Content.ImageShow;
                IsEdit = true;
                Title = "Editar Publicacion";
            }
            else
            {
                Content = new ContentResponse {};
                ImageSource = "noimg";
                IsEdit = false;
                Title = "Agregar Publicacion";
            }


            LoadContentTypesAsync();
            LoadParks();
        }

        
        private async void LoadContentTypesAsync()
        {
            var url = App.Current.Resources["UrlAPI"].ToString();
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            var response = await _apiService.GetListAsync<ContentTypeResponse>(url, "/api", "/ContentTypes", "bearer", token.Token);

            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Error Al obtener lista  de Tipso", "Aceptar");
                return;
            }

            var contentTypes = (List<ContentTypeResponse>)response.Result;
            ContentTypes = new ObservableCollection<ContentTypeResponse>(contentTypes);

            if (!string.IsNullOrEmpty(Content.Description))
            {
                ContentType = ContentTypes.FirstOrDefault(pt => pt.Name == Content.ContentType.Name);
            }
        }

    

        private void LoadParks()
        {
           var _Pubs = JsonConvert.DeserializeObject<PublicationsResponse>(Settings.Pubs);

            Parks = new ObservableCollection<ParkResponse>(_Pubs.Parks);
        }

        private async void Changeimage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                "Seleccione Origen:",
                "Cancelar",
                null,
                "Galeria",
                "Camara");

            if (source == "Cancelar")
            {
                _file = null;
                return;
            }

            if (source == "Camara")
            {
                _file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                _file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (_file != null)
            {
                ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = _file.GetStream();
                    return stream;
                });
            }

        }

        private async void SaveAsync()
        {
            var isValid = await ValidateDataAsync();
            if (!isValid)
            {
                return;
            }

            IsRunning = true;
            IsEnabled = false;

            var url = App.Current.Resources["UrlAPI"].ToString();
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            var user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);

            var contentRequest = new ContentRequest
            {
                Id = Content.Id,
                Description= Content.Description,
                Date = IsEdit ? Content.Date : DateTime.Today,
                ContentType = ContentType.Id,
                Park = Park.Id,
                UserId = user.Id
                
            };

            Response<object> response;
            if (IsEdit)
            {
                response = await _apiService.PutAsync(
                    url,
                    "/api",
                    "/Content",
                    contentRequest.Id,
                    contentRequest,
                    "bearer",
                    token.Token);
            }
            else
            {
                response = await _apiService.PostAsync(
                    url,
                    "/api",
                    "/Content",
                    contentRequest,
                    "bearer",
                    token.Token);
            }

            byte[] imageArray = null;
            if (_file != null)
            {
                imageArray = FilesHelper.ReadFully(_file.GetStream());
                if (Content.Id == 0)
                {
                    var response2 = await _apiService.GetLastContentByUserId(
                        url,
                        "/api",
                        "/Content/GetLastContentByUserId",
                        "bearer",
                        token.Token,
                        user.Id);
                    if (response2.IsSuccess)
                    {
                        var content = (ContentResponse)response2.Result;
                        Content.Id = content.Id;
                    }
                }

                if (Content.Id != 0)
                {
                    var imageRequest = new ImageRequest
                    {
                        ContentId = Content.Id,
                        ImageArray = imageArray
                    };

                    var response3 = await _apiService.PostAsync(url, "/api", "/Content/AddImageToContent", imageRequest, "bearer", token.Token);
                    if (!response3.IsSuccess)
                    {
                        IsRunning = false;
                        IsEnabled = true;
                        await App.Current.MainPage.DisplayAlert("Error", response3.Message, "Aceptar");
                    }
                }
            }
              
            if (!response.IsSuccess)
            {
                IsRunning = false;
                IsEnabled = true;
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }

            await PubsPageViewModel.GetInstance().UpdateContentAsync();

            IsRunning = false;
            IsEnabled = true;

            await App.Current.MainPage.DisplayAlert(
                "Publicacion",
                IsEdit ? "Se han Cambiado Los Datos Exitosamente.": "La Publicacion Se ha Creado exitosamente.",
                "Aceptar");

            await _navigationService.GoBackToRootAsync();
        }

        private async Task<bool> ValidateDataAsync()
        {
            if (string.IsNullOrEmpty(Content.Description))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe agregar una Descripcion", "Aceptar");
                return false;
            }

            if (string.IsNullOrEmpty(Content.Date.ToString()))
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe Agregar una Fecha", "Aceptar");
                return false;
            }
                        
            if (Park == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe Seleccionar un Parque", "Aceptar");
                return false;
            }

            if (ContentType == null)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Debe Seleccioanr un Tipo", "Aceptar");
                return false;
            }

            return true;
        }

    }
}
