using PNN.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PNN.Prism.ViewModels
{
    public class PubPageViewModel : ViewModelBase
    {
        private ContentResponse _content;

        public PubPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "Publicacion";
        }

        public ContentResponse Content 
        { 
            get => _content;
            set => SetProperty(ref _content, value);  
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("Pub")) 
            {
                Content = parameters.GetValue<ContentResponse>("Pub");
            }

        }
    }
}
