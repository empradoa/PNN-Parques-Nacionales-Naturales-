using System.Collections.Generic;
using System.Threading.Tasks;
using PNN.Common.Models;
using PNN.web.Data.Entities;
using PNN.Web.Data.Entities;
using PNN.Web.Models;

namespace PNN.Web.Helpers
{
    public interface IConverterHelper
    {
        //convierte de ContentViewModel a Content
        Task<Content> ToContentAsync(ContentViewModel model, string path, bool isNew);

        // convierte de Content a ContentViewModel
        ContentViewModel ToContentViewModel(Content content);


        // convierte de ToCommentContentAsync a Comment
        Task<Comment> ToCommentToContentAsync(CommentViewModel model, bool isNew);

        // convierte de Comment a CommentViewModel
        CommentViewModel ToCommentToContentViewModel(Comment comment);


        //metodos de converter helper para park
        //convierte de ParkViewModel a Park
        Task<Park> ToParkAsync(ParkViewModel model, string path, bool isNew);

        // convierte de Park a ParkViewModel
        ParkViewModel ToParkViewModel(Park content);

        //convierte de Zone ViewModel to zone
        Task<Zone> ToZoneAsync(ZoneViewModel zone, bool isNew);

        //convierte de Zone to Zone ViewModel
        ZoneViewModel ToZoneViewModel(Zone zone);

        //conversiones para deserializacion y serializacion en los API's
        ICollection<ParkResponse> ToListParkResponse(ICollection<Park> prk);
        ParkResponse ToParkResponse(Park p);
        ICollection<ZoneResponse> ToListZoneResponse(ICollection<Zone> zn);
        ZoneResponse ToZoneResponse(Zone z);
        ZoneTypesResponse ToZoneTyperespone(ZoneType zt);
        ManagerResponse ToManagerResponse(Manager manager);
        User ToUser(UserResponse u);
        UserResponse ToUserResponse(User u);
        ICollection<Content> ToListContent(ICollection<ContentResponse> cont);
        Content ToContent(ContentResponse c);
        ICollection<ContentResponse> ToListContentResponse(ICollection<Content> cont);
        ContentResponse ToContentResponse(Content c);
        ICollection<Comment> ToListComments(ICollection<CommentResponse> cmm);
        Comment ToComment(CommentResponse cmm);
        ICollection<CommentResponse> ToListCommentsResponse(ICollection<Comment> cmm);
        CommentResponse ToCommentResponse(Comment cmm);
        ContentType ToContentType(ContentTypeResponse ct);
        ContentTypeResponse ToContentTypeResponse(ContentType ct);
        ICollection<AreaResponse> ToListAreaResponse(ICollection<Area> loc);
        AreaResponse ToAreaResponse(Area ar);
        LocationResponse ToLocationResponse(Location l);

    }
}