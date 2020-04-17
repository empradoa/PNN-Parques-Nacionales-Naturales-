using System.Threading.Tasks;
using PNN.Web.Data.Entities;
using PNN.Web.Models;

namespace PNN.Web.Helpers
{
    public interface IConverterHelper
    {
        Task<Content> ToContentAsync(ContentViewModel model, string path);
    }
}