using System;
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
    public class ContentsController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly ICombosHelper _combosHelper;
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IImageHelper _imageHelper;

        public ContentsController(DataContext dataContext,
                                IUserHelper userHelper,
                                ICombosHelper combosHelper,
                                IConverterHelper converterHelper,
                                IImageHelper imageHelper)
        {
            _dataContext = dataContext;
            _combosHelper = combosHelper;
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _imageHelper = imageHelper;
        }

        // GET: Contents
        public IActionResult Index()
        {
            return View(_dataContext.Contents
                .Include(u => u.User)
                .Include(u => u.Comments));
        }

        // GET: Contents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _dataContext.Contents
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        // GET: Contents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Description,Date,ImageUrl")] Content content)
        {
            if (ModelState.IsValid)
            {
                _dataContext.Add(content);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(content);
        }

        // GET: Contents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _dataContext.Contents.FindAsync(id);
            if (content == null)
            {
                return NotFound();
            }
            return View(content);
        }

        // POST: Contents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Date,ImageUrl,Like,DisLike")] Content content)
        {
            if (id != content.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dataContext.Update(content);
                    await _dataContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContentExists(content.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(content);
        }

        // GET: Contents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _dataContext.Contents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        // POST: Contents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var content = await _dataContext.Contents.FindAsync(id);
            _dataContext.Contents.Remove(content);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContentExists(int id)
        {
            return _dataContext.Contents.Any(e => e.Id == id);
        }

        public async Task<IActionResult> AddContent(String id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _dataContext.Users.FindAsync(id.ToString());


            var content = await _dataContext.Contents
                                .Include(c => c.User)
                                .Include(ct => ct.ContentType)
                                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            //instanciamos la clase ContentViewModel que creamos en la carpeta models, para el modelo del contenido
            var model = new ContentViewModel
            {
                Date = DateTime.Now,
                UserId = user.Id,
                //Id = content.Id,
                //creamos una clase ICombosHelps para poder traer la lista de ContentTypes
                ContentTypes = _combosHelper.GetComboContentTypes(),
                Parks = _combosHelper.GetComboParks()
            };

            return View(model);
        }

        [HttpPost]
        //sobre cargamos el metodo AddContent para guardar pero le agregamos el modelo ContentViewModel
        public async Task<IActionResult> AddContent(ContentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var path = string.Empty;

                if (model.ImageFile != null)
                {
                    //invocamos el metodo UploadImageAsync de IImageHelper
                    path = await _imageHelper.UploadImageAsync(model.ImageFile);
                }
                //creamos una instancia del objeto content y en el metodo ToContect nos devuelve el objeto Content
                //true porque es un nuevo contenido
                var content = await _converterHelper.ToContentAsync(model, path, true);
                _dataContext.Contents.Add(content);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"Details/{model.UserId}");
            }
            model.ContentTypes = _combosHelper.GetComboContentTypes();
            model.Parks = _combosHelper.GetComboParks();
            return View(model);
        }
    }
}
