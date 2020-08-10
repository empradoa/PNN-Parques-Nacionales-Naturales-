using Newtonsoft.Json;
using PNN.Common.Helpers;
using PNN.Common.Models;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PNN.Prism.ViewModels
{
    public class CommentItemViewModel:CommentResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _likepubCommand;
        private DelegateCommand _likezoneCommand;

        public CommentItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand LikePubCommand => _likepubCommand ?? (_likepubCommand = new DelegateCommand(LikePub));

        
        public DelegateCommand LikeZoneCommand => _likezoneCommand ?? (_likezoneCommand = new DelegateCommand(LikeZone));

        private async void LikePub()
        {
            var rcts = JsonConvert.DeserializeObject<List<Reactions>>(Settings.Reactions);
            var user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);
                       
            var pubid = JsonConvert.DeserializeObject<int>(Settings.PubId);

            var comment = this;

           

            var rct = rcts == null ? null : rcts.FirstOrDefault(r => r.CommentId == comment.Id 
                                                                    && r.ContentId== pubid
                                                                    && r.UserId == user.Id);

            if (rct != null)
            {
                 await App.Current.MainPage.DisplayAlert("parque", "Ya has seleccionado que te gusta anteriormente.", "Aceptar");
                    return;
                
            }
            else
            {
                rct = new Reactions
                {
                    Id = rcts == null ? 1 : rcts.Last().Id + 1,
                    CommentId = comment.Id,
                    ContentId = pubid,
                    Tipo = 2,
                    UserId = user.Id
                };
                comment.Like++;
            }

            if (rcts == null)
                rcts = new List<Reactions>();

            rcts.Add(rct);
            Settings.Reactions = JsonConvert.SerializeObject(rcts);

           // await PubPageViewModel.GetInstance().UpdateContentAsync();
        }


        private async void LikeZone()
        {
            var rcts = JsonConvert.DeserializeObject<List<Reactions>>(Settings.Reactions);
            var user = JsonConvert.DeserializeObject<UserResponse>(Settings.User);

            var zoneid = JsonConvert.DeserializeObject<int>(Settings.ZoneId);


            var comment = this;



            var rct = rcts == null ? null : rcts.FirstOrDefault(r => r.CommentId == comment.Id
                                                                    && r.ZoneId == zoneid
                                                                    && r.UserId == user.Id);

            if (rct != null)
            {
                await App.Current.MainPage.DisplayAlert("parque", "Ya has seleccionado que te gusta anteriormente.", "Aceptar");
                return;

            }
            else
            {
                rct = new Reactions
                {
                    Id = rcts == null ? 1 : rcts.Last().Id + 1,
                    CommentId = comment.Id,
                    ZoneId = zoneid,
                    Tipo = 2,
                    UserId = user.Id
                };
                comment.Like++;
            }

            if (rcts == null)
                rcts = new List<Reactions>();

            rcts.Add(rct);
            Settings.Reactions = JsonConvert.SerializeObject(rcts);

           // await PubPageViewModel.GetInstance().UpdateContentAsync();
        }

       
    }
}
