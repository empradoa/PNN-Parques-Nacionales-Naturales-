using System;
using System.Collections.Generic;
using System.Text;

namespace PNN.Common.Models
{    
   //Se creaesta clase para traer desde el APi de Content
   //para traer toda la informaciona mostrar en el aplicativo 
   //y evitar m,ultiples llamados alentando la  app

    public class PublicationsResponse 
    {
        public ICollection<ContentResponse> Contents { get; set; }
        public ICollection<ParkResponse> Parks { get; set; } 
        
    }
}
