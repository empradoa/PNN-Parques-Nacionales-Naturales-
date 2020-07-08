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
    public class PubsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private PublicationsResponse _Ps;
        private ObservableCollection<ContentItemViewModel> _pubs;
        public PubsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Publicaciones";
            _navigationService = navigationService;
        }

        public ObservableCollection<ContentItemViewModel> Pubs
        {
            get => _pubs;
            set => SetProperty(ref _pubs, value);

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            //_Params = parameters;

            _Ps = parameters.GetValue<PublicationsResponse>("Publications");

            Pubs = new ObservableCollection<ContentItemViewModel>(_Ps.Contents.Select(c=> new ContentItemViewModel(_navigationService) {
                Id = c.Id,
                Description = c.Description,
                Date = c.Date,
                ImageUrl = c.ImageUrl,
                Like = c.Like,
                ContentType = c.ContentType,
                Park = c.Park,
                Comments = c.Comments
            } ).ToList());


            return;
        }
    }
}
