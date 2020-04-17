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

        public ConverterHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        //convierte un objeto de la clase ContentViewModel en la clase Content
        public async Task<Content> ToContentAsync(ContentViewModel model, string path)
        {
            return new Content
            {
                Description = model.Description,
                Date = model.DateLocal,
                ImageUrl = path,
                Like = model.Like,
                DisLike = model.DisLike,
                Comments = model.Comments,
                Owner = await _dataContext.Owners.FindAsync(model.OwnerId),
                ContentType = await _dataContext.ContentTypes.FindAsync(model.ContentTypeId),
                Location = await _dataContext.Locations.FindAsync(model.LocationId)
            };
        }
    }
}
