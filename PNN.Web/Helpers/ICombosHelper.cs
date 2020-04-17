using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PNN.Web.Helpers
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboContentTypes(); 
        IEnumerable<SelectListItem> GetComboParks();
    }
}