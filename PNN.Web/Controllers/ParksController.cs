using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PNN.web.Data;
using PNN.Web.Data.Entities;
using PNN.Web.Helpers;
using PNN.Web.Models;

namespace PNN.Web.Controllers
{
    public class ParksController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IConverterHelper _converterHelper;
        private readonly IImageHelper _imageHelper;

        public ParksController(
                            DataContext dataContext,
                            IConverterHelper converterHelper,
                            IImageHelper imageHelper)
        {
            _dataContext = dataContext;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
        }

        // GET: Parks
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Parks.ToListAsync());
        }

        // GET: Parks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var park = await _dataContext.Parks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (park == null)
            {
                return NotFound();
            }

            return View(park);
        }

        //crear parque
        public async Task<IActionResult> Create()
        {

            var user = await _dataContext.Users
                .Include(ct => ct.Contents)
                .FirstOrDefaultAsync(u => u.UserName.ToLower().Equals(User.Identity.Name.ToLower()));

            if (user == null)
            {
                return NotFound();
            }

            var manager = await _dataContext.Managers
                .Include(m => m.Parks)
                .FirstOrDefaultAsync(m => m.User.UserName.ToLower().Equals(User.Identity.Name.ToLower()));

            if (manager == null)
            {
                return NotFound();
            }

            //instanciamos la clase ParkViewModel que creamos en la carpeta models, para el modelo del parke
            var model = new ParkViewModel
            {
                ManagerId = manager.Id
            };

            return View(model);
        }

        [HttpPost]
        //sobre cargamos el metodo create para guardar pero le agregamos el modelo ParkViewModel
        public async Task<IActionResult> Create(ParkViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null)
                {
                    //invocamos el metodo UploadImageAsync de IImageHelper
                    path = await _imageHelper.UploadImageAsync(model.ImageFile);
                }
                //creamos una instancia del objeto content y en el metodo ToContect nos devuelve el objeto Content
                //true porque es un nuevo contenido
                var park = await _converterHelper.ToParkAsync(model, path, true);
                _dataContext.Parks.Add(park);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"Index");
            }
            return View(model);
        }

        //detalles del usuario tener encuenta la consulta a la bd el Id no es el Id del owner sino del Content que viene de details del owner METODO GET
        public async Task<IActionResult> EditPark(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var park = await _dataContext.Parks
                .Include(ct => ct.Manager)
                .ThenInclude(ct => ct.User)
                .Include(ct => ct.Manager)
                .Include(ct => ct.Zones)
                .FirstOrDefaultAsync(ct => ct.Id == id);
            if (park == null)
            {
                return NotFound();
            }
            //invocamos el metodo ToContentViewModel del ConverterHelper para pintar los datos en el formulario editar content
            return View(_converterHelper.ToParkViewModel(park));
        }

        [HttpPost]
        //sobrecargamos el metodo EditContent para armar el POST
        public async Task<IActionResult> EditPark(ParkViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = model.ImageUrl;

                if (model.ImageFile != null)
                {
                    //invocamos el metodo UploadImageAsync de IImageHelper
                    path = await _imageHelper.UploadImageAsync(model.ImageFile);
                }
                //creamos una instancia del objeto content y en el metodo ToContect nos devuelve el objeto Content
                //false porque no es un nuevo contenido
                var park = await _converterHelper.ToParkAsync(model, path, false);
                _dataContext.Parks.Update(park);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"Index");
            }
            return View(model);
        }


        // GET: Parks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var park = await _dataContext.Parks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (park == null)
            {
                return NotFound();
            }

            return View(park);
        }

        // POST: Parks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var park = await _dataContext.Parks.FindAsync(id);
            _dataContext.Parks.Remove(park);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParkExists(int id)
        {
            return _dataContext.Parks.Any(e => e.Id == id);
        }
    }
}
