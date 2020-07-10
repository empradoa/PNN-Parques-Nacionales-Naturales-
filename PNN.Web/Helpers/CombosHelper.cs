using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using PNN.web.Data;

namespace PNN.Web.Helpers
{
    //OJO: una vez creemos el metodo CombosHelper y la interfaz ICombosHelper se debe inyectar en el Startup
    public class CombosHelper : ICombosHelper
    {
        private readonly DataContext _dataContext;

        public CombosHelper(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IEnumerable<SelectListItem> GetComboContentTypes()
        {
            //listamos de la tabla contenttypes el datos Name, Id para pintarlos en el combo
            var list = _dataContext.ContentTypes.Select(ct => new SelectListItem
            {
                Text = ct.Name,
                Value = $"{ct.Id}"
            })
                .OrderBy(ct => ct.Value)
                .ToList();

            //insertamos la primera linea 0 para que el usuario sepa lo que va a seleccionar
            list.Insert(1, new SelectListItem
            {
                Text = "Seleccione el tipo de publicación",
                Value = "0"
            });

            return list;
        }
        public IEnumerable<SelectListItem> GetComboParks()
        {
            //listamos de la tabla contenttypes el datos Name, Id para pintarlos en el combo
            var list = _dataContext.Parks.Select(ct => new SelectListItem
            {
                Text = ct.Name,
                Value = $"{ct.Id}"
            })
                .OrderBy(ct => ct.Value)
                .ToList();

            //insertamos la primera linea 0 para que el usuario sepa lo que va a seleccionar
            //list.Insert(1, new SelectListItem
            //{
            //Text = "Puede seleccionar un parque nacional si lo desea",
            //Value = "1"
            //});

            return list;
        }

        public IEnumerable<SelectListItem> GetComboRoles()
        {
            //listamos de la tabla contenttypes el datos Name, Id para pintarlos en el combo
            var list = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "(Seleccione un Rol...)" },
                new SelectListItem { Value = "1", Text = "Admin" },
                new SelectListItem { Value = "2", Text = "Usuario" }
            };

            return list;
        }

    }
}
