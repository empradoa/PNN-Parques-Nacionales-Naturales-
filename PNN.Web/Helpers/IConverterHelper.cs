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
        Task<Comment> ToCommentAsync(CommentViewModel model, bool isNew);

        // convierte de Comment a CommentViewModel
        CommentViewModel ToCommentViewModel(Comment comment);

    }
}