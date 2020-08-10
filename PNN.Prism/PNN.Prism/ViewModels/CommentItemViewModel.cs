using Newtonsoft.Json;
using PNN.Common.Helpers;
using PNN.Common.Models;
using PNN.Common.Services;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNN.Prism.ViewModels
{
    public class CommentItemViewModel:CommentResponse
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiServices;
        private DelegateCommand _likepubCommand;
        private DelegateCommand _likezoneCommand;

        public CommentItemViewModel(INavigationService navigationService,
                                    IApiService apiServices )
        {
            _navigationService = navigationService;
            _apiServices = apiServices;
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
                rcts.Remove(rct);
                comment.Like--;

                ActCmm(0, pubid, comment.Id,-1);
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
                ActCmm(0, pubid, comment.Id,1);

                if (rcts == null)
                    rcts = new List<Reactions>();

                rcts.Add(rct);
            }

            
            Settings.Reactions = JsonConvert.SerializeObject(rcts);

            

           await PubsPageViewModel.GetInstance().UpdateContentAsync();

            var Ps = JsonConvert.DeserializeObject<PublicationsResponse>(Settings.Pubs);
            var contentId = JsonConvert.DeserializeObject<int>(Settings.PubId);

            var content = Ps.Contents.ToList().Find(c => c.Id == contentId);

            PubPageViewModel.GetInstance().Content = content;

            PubPageViewModel.GetInstance().LoadComments();

            PubPageViewModel.GetInstance().Content = content;
            PubPageViewModel.GetInstance().LoadComments();
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
                rcts.Remove(rct);
                comment.Like--;
                await ActCmm(zoneid, 0, comment.Id, -1);
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
                await ActCmm(zoneid, 0, comment.Id,1);

                if (rcts == null)
                    rcts = new List<Reactions>();

                rcts.Add(rct);
            }

            
            Settings.Reactions = JsonConvert.SerializeObject(rcts);

            

            await PubsPageViewModel.GetInstance().UpdateContentAsync();

            var Ps = JsonConvert.DeserializeObject<PublicationsResponse>(Settings.Pubs);
            var parkId = JsonConvert.DeserializeObject<int>(Settings.ParkId);
            var zoneId = JsonConvert.DeserializeObject<int>(Settings.ZoneId);

            var park = Ps.Parks.ToList().Find(p => p.Id == parkId);
            var zone = park.Zones.ToList().Find(z => z.Id == zoneId);

            ZonePageViewModel.GetInstance().Zone = zone;

            ZonePageViewModel.GetInstance().LoadComments();
         
        }

        private async Task ActCmm(int z, int p , int c, int l)
        {
            var url = App.Current.Resources["UrlAPI"].ToString();
            var token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);


            var request = new LikeRequest
            {
                ZoneId    = z,
                ContentId = p,
                CommentId = c,
                Like = l
            };

            var response = new Response<object>();

            if (z != 0)
            { 
             response = await _apiServices.PutAsync(
                    url,
                    "/api",
                    "/Zone/LikeCommentAsync",
                    z,
                    request,
                    "bearer",
                    token.Token);
            }

            if (p != 0)
            {
                response = await _apiServices.PutAsync(
                       url,
                       "/api",
                       "/Content/LikeCommentAsync",
                       p,
                       request,
                       "bearer",
                       token.Token);
            }


            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert("Error", response.Message, "Aceptar");
                return;
            }
        }
    }
}
