using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PNN.web.Data;
using PNN.Web.Data.Entities;
using PNN.Web.Models;

namespace PNN.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _dataContext;
        private readonly ICombosHelper _combosHelper;

        public ConverterHelper(
            DataContext dataContext,
            ICombosHelper combosHelper)
        {
            _dataContext = dataContext;
            _combosHelper = combosHelper;
        }
        //convierte un objeto de la clase ContentViewModel en la clase Content
        public async Task<Content> ToContentAsync(ContentViewModel model, string path, bool isNew)
        {
            var content = new Content
            {
                Description = model.Description,
                Date = DateTime.Now,
                Id = isNew ? 0 : model.Id,
                ImageUrl = path,
                Like = model.Like,
                DisLike = model.DisLike,
                Comments = model.Comments,
                Owner = await _dataContext.Owners.FindAsync(model.OwnerId),
                Park = await _dataContext.Parks.FindAsync(model.ParkId),
                ContentType = await _dataContext.ContentTypes.FindAsync(model.ContentTypeId),
                Location = await _dataContext.Locations.FindAsync(model.LocationId)
            };
            return content;
        }

        public ContentViewModel ToContentViewModel(Content content)
        {
            return new ContentViewModel
            {
                Description = content.Description,
                Date = content.Date,
                ImageUrl = content.ImageUrl,
                Like = content.Like,
                DisLike = content.DisLike,
                Comments = content.Comments,
                Owner = content.Owner,
                ContentType = content.ContentType,
                Park = content.Park,
                //Location = content.Location,
                Id = content.Id,
                OwnerId = content.Owner.Id,
                ContentTypeId = content.ContentType.Id,
                ContentTypes = _combosHelper.GetComboContentTypes(),
                ParkId = content.Park.Id,
                Parks = _combosHelper.GetComboParks(),
                //LocationId = content.Location.Id               
            };
        }
        //metodo para agregar comentario desde contenido o publicación de CommentViewModel a Comment
        public async Task<Comment> ToCommentAsync(CommentViewModel model, bool isNew)
        {

            return new Comment
            {
                Date = DateTime.Now,
                Description = model.Description,
                Id = isNew ? 0 : model.Id,
                Content = await _dataContext.Contents.FindAsync(model.ContentId),
                Owner = await _dataContext.Owners.FindAsync(model.OwnerId)
            };
        }

        //metodo para agregar comentario desde contenido o publicación de Comment a CommentViewModel
        public CommentViewModel ToCommentViewModel(Comment comment)        {
            
            return new CommentViewModel
            {
                Date = comment.Date,
                Description = comment.Description,
                Id = comment.Id,
                ContentId = comment.Content.Id,
                OwnerId = comment.Owner.Id
            };
        }

    }
}
