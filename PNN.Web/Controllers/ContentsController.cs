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
        public ActionResult Index()
        {
            return View(_dataContext.Contents
                .Include(ct => ct.ContentType)
                .Include(ct => ct.Comments)
                .Include(ct => ct.User)
                .OrderByDescending(ct => ct.Date));
        }


        // GET: Contents
        /* public async Task<IActionResult> Index()
         {

             var content = await _dataContext.Contents
                 .Include(u => u.ContentType)
                 .Include(u => u.User)
                 .ThenInclude(ct => ct.Comments)
                 .FirstOrDefaultAsync(u => u.User.UserName.ToLower().Equals(User.Identity.Name.ToLower()));

             var response = new UserViewModel
             {
                 Id = content.User.Id,
                 FirstName = content.User.FirstName,
                 LastName = content.User.LastName,
                 Address = content.User.Address,
                 Email = content.User.Email,
                 CellPhone = content.User.CellPhone,
                 Contents = content.User.Contents.Select(ct => new ContentViewModel
                 {
                     Date = ct.Date,
                     Id = ct.Id,
                     ImageUrl = ct.ImageFullPath,
                     Description = ct.Description,
                     Like = ct.Like,
                     ContentTypes = _combosHelper.GetComboContentTypes(),
                     Parks = _combosHelper.GetComboParks(),
                     Comments = ct.Comments.Select(cm => new CommentViewModel
                     {
                         Date = cm.Date,
                         Description = cm.Description,
                         Id = cm.Id,
                         Like = cm.Like
                     }).ToList()
                 }).ToList()
             };

             return View(_dataContext.Contents
                 .Include(ct => ct.ContentType)
                 .Include(ct => ct.User)
                 .ThenInclude(ct => ct.Comments)
                 .Include(ct => ct.Comments)
                 .ThenInclude(ct => ct.User));
         }*/

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
        //agregar contenido al contents
        public async Task<IActionResult> AddContent()
        {

            var user = await _dataContext.Users
                .Include(ct => ct.Contents)
                .FirstOrDefaultAsync(u => u.UserName.ToLower().Equals(User.Identity.Name.ToLower()));

            /*var content = await _dataContext.Contents
                                .Include(c => c.User)
                                .Include(ct => ct.ContentType)
                                .FirstOrDefaultAsync();*/
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
                var content = await _converterHelper.ToContentAsync(model, path, true);
                _dataContext.Contents.Add(content);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"Index/{model.UserId}");
            }
            model.ContentTypes = _combosHelper.GetComboContentTypes();
            model.Parks = _combosHelper.GetComboParks();
            return View(model);
        }

        //detalles del usuario tener encuenta la consulta a la bd el Id no es el Id del owner sino del Content que viene de details del owner METODO GET
        public async Task<IActionResult> EditContent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _dataContext.Contents
                .Include(ct => ct.User)
                .Include(ct => ct.ContentType)
                .Include(ct => ct.Park)
                .Include(ct => ct.Location)
                .FirstOrDefaultAsync(ct => ct.Id == id);
            if (content == null)
            {
                return NotFound();
            }
            //invocamos el metodo ToContentViewModel del ConverterHelper para pintar los datos en el formulario editar content
            return View(_converterHelper.ToContentViewModel(content));
        }

        [HttpPost]
        //sobrecargamos el metodo EditContent para armar el POST
        public async Task<IActionResult> EditContent(ContentViewModel model)
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
                var content = await _converterHelper.ToContentAsync(model, path, false);
                _dataContext.Contents.Update(content);
                await _dataContext.SaveChangesAsync();
                return RedirectToAction($"Index/{model.UserId}");
            }
            model.ContentTypes = _combosHelper.GetComboContentTypes();
            model.Parks = _combosHelper.GetComboParks();
            return View(model);
        }


        //metodo para eliminar comentarios
        public async Task<IActionResult> DeleteContent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _dataContext.Contents
                .Include(p => p.User)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id.Value);


            if (content == null)
            {
                return NotFound();
            }


            /*if (content.Comments.Count > 0)
            {
                ModelState.AddModelError(string.Empty, "La publicación no se puede eliminar porque tiene comentarios.");
                return RedirectToAction($"{nameof(Details)}/{content.User.Id}");
            }*/

            _dataContext.Contents.Remove(content);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction($"{nameof(Index)}/{content.Id}");
        }

        [HttpPost]
        //metodo para editar comentarios a las publicaciones o contenidos
        public async Task<IActionResult> AddCommentToContent(int? id, string descripcion)
        {
            if (id == null)
            {
                return RedirectToAction($"Index");
            }
                        

            var content = await _dataContext.Contents
                .Include(c => c.Comments)
                .Include(c => c.User)
                .FirstOrDefaultAsync(ct => ct.Id == id);

            if (descripcion == null)
            {
                return RedirectToAction($"Index");
            }

            var user = await _dataContext.Users
                .Include(ct => ct.Contents)
                .Include(ct => ct.Comments)
                .FirstOrDefaultAsync(u => u.UserName.ToLower().Equals(User.Identity.Name.ToLower()));

            var comment = new Comment
            {
                Date = DateTime.Now,
                Description = descripcion,
                Content = await _dataContext.Contents.FindAsync(id),
                User = await _dataContext.Users.FindAsync(user.Id)
            };

            //var content = await _converterHelper.CommentToContentAsync(comment, true);

            _dataContext.Comments.Add(comment);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction($"Index");
        }

        //ver las publicaciones de los contenidos
        public async Task<IActionResult> DetailsContent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _dataContext.Contents
                .Include(ct => ct.User)
                .Include(ct => ct.Comments)
                .ThenInclude(ct => ct.Content)
                .Include(t => t.ContentType)
                .FirstOrDefaultAsync(o => o.Id == id.Value);
            if (content == null)
            {
                return NotFound();
            }

            return View(content);
        }

        private bool ContentExists(int id)
        {
            return _dataContext.Contents.Any(e => e.Id == id);
        }
    }
}
