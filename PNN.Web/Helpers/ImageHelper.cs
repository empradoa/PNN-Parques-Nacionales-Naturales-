using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace PNN.Web.Helpers
{
    public class ImageHelper : IImageHelper
    {
        //metodo reutilizable para cargar una imagen a la pagina web a la cargue Contents
        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            //agregamos una imagen que no se repita en la carpeta Contents de wwwroot
            var guid = Guid.NewGuid().ToString();
            var file = $"{guid}.jpg";
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot\\images\\Contents",
                file);

            //metodos permiten el cargue de la imagen al servidor
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"~/images/Contents/{file}";
        }
    }
}
