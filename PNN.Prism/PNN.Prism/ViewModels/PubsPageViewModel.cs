﻿using PNN.Common.Models;
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
        private UserResponse _User;
        private ObservableCollection<ContentResponse> _pubs;
        public PubsPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Publicaciones";
        }

        public ObservableCollection<ContentResponse> Pubs
        {
            get => _pubs;
            set => SetProperty(ref _pubs, value);

        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("User")) 
            {
                _User = parameters.GetValue<UserResponse>("User");

                Pubs = new ObservableCollection<ContentResponse>(_User.Contents);
            }
        }
    }
}