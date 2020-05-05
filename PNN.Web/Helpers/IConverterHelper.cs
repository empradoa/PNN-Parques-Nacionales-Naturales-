using System.Threading.Tasks;
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
    }
}