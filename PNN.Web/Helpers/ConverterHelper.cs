using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
