using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PNN.Web.Data.Entities;
using PNN.web.Data;
using Microsoft.AspNetCore.Authorization;
using PNN.Web.Models;
using PNN.Web.Helpers;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace PNN.Web.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class OwnersController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;
        private readonly ICombosHelper _combosHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IImageHelper _imageHelper;

        public OwnersController(
            DataContext dataContext,
            IUserHelper userHelper,
            ICombosHelper combosHelper,
            IConverterHelper converterHelper,
            IImageHelper imageHelper)
        {
            _dataContext = dataContext;
            _userHelper = userHelper;
            _combosHelper = combosHelper;
            _converterHelper = converterHelper;
            this._imageHelper = imageHelper;
        }

        // GET: Owners
        public IActionResult Index()
        {
            //se hace una especie de consulta sql donde con Include toma forma de Join para relacionar la tabla owners con User
            //select * from owner inner join User
            return View(_dataContext.Owners
                .Include(o => o.User)
                .Include(o => o.Contents));
        }

        //detalles del usuario tener encuenta la consulta a la bd
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _dataContext.Owners
                .Include(o => o.User)
                //.Include(o => o.Comments)
                .Include(o => o.Contents)
                .ThenInclude(ct => ct.ContentType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // GET: Owners/Create
        public IActionResult Create()
        {
            return View();
        }

        // Creamos el metodo para crear usuario que ereda de la AddUserViewModel del Owner
        // AddUserViewModel se crea en la caperta Models
        // Inyectamos el IUserHelper para llamar los metodos de usuarios o owners
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    Address = model.Address,
                    Email = model.Username,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    CellPhone = model.CellPhone,
                    UserName = model.Username
                };
                //implementamos el metodo de la IUserHelper para agregar usuario de las viewModel
                var response = await _userHelper.AddUserAsync(user, model.Password);

                //si el usuario cumple con la validación
                if (response.Succeeded)
                {
                    //agragamos el usuario por email
                    var userInDB = await _userHelper.GetUserByEmailAsync(model.Username);
                    //agregamos el rol de usuario (Customer)
                    await _userHelper.AddUserToRoleAsync(userInDB, "Customer");

                    //como en la tabla de owner no tenemos nada agregamos la datos que necesitamos 
                    var owner = new Owner
                    {
                        Contents = new List<Content>(),
                        //Comments = new List<Comment>(),
                        User = userInDB
                    };

                    //agregamos el owner
                    _dataContext.Owners.Add(owner);

                    try
                    {
                        await _dataContext.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.ToString());
                    }                   
                }

                ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                
            }
            return View(model);
        }

        // GET: Owners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _dataContext.Owners.FindAsync(id);
            if (owner == null)
            {
                return NotFound();
            }
            return View(owner);
        }

        // POST: Owners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] Owner owner)
        {
            if (id != owner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dataContext.Update(owner);
                    await _dataContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OwnerExists(owner.Id))
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
            return View(owner);
        }

        // GET: Owners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _dataContext.Owners
                .FirstOrDefaultAsync(m => m.Id == id);
            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // POST: Owners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var owner = await _dataContext.Owners.FindAsync(id);
            _dataContext.Owners.Remove(owner);
            await _dataContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OwnerExists(int id)
        {
            return _dataContext.Owners.Any(e => e.Id == id);
        }

        //detalles del usuario tener encuenta la consulta a la bd
        public async Task<IActionResult> AddContent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _dataContext.Owners.FindAsync(id.Value);
            if (owner == null)
            {
                return NotFound();
            }

            //instanciamos la clase ContentViewModel que creamos en la carpeta models, para el modelo del contenido
            var model = new ContentViewModel
            {
                Date = DateTime.Today,
                OwnerId = owner.Id,
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
                return RedirectToAction($"Details/{model.OwnerId}");
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
                .Include(ct => ct.Owner)
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
                return RedirectToAction($"Details/{model.OwnerId}");
            }
            model.ContentTypes = _combosHelper.GetComboContentTypes();
            model.Parks = _combosHelper.GetComboParks();
            return View(model);
        }

        //editamos el contentenido o publicación del owner
        public async Task<IActionResult> DetailsContent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var content = await _dataContext.Contents
                .Include(ct => ct.Owner)
                .ThenInclude(o => o.User)
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

        //metodo para agregar comentarios a las publicaciones o contenidos
        public async Task<IActionResult> AddCommentToContent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }            

            var content = await _dataContext.Contents.FindAsync(id.Value);
            //var user = await _dataContext.Users.FindAsync(id.Value);
            if (content == null)
            {
                return NotFound();
            }

            //nos traemos los datos del usuario logeado
            //var owner = await _dataContext.Owners.FirstOrDefaultAsync(o => o.User.UserName.ToLower().Equals(User.Identity.Name.ToLower()));

            var user = await _dataContext.Users
                .Include(ct => ct.Comments)
                .FirstOrDefaultAsync(u => u.UserName.ToLower().Equals(User.Identity.Name.ToLower()));

            var model = new CommentViewModel
            {
                Date = DateTime.Now,
                ContentId = content.Id,
                UserId = user.Id
            };

            return View(model);
        }

        //sobre cargamos el metodo AddCommentToContent pero en el HttpPost
        [HttpPost]
        public async Task<IActionResult> AddCommentToContent(CommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                //var comment = await _dataContext.Comments.FindAsync(model.Id);
                if (ModelState.IsValid)
                {
                    var comment = await _converterHelper.ToCommentAsync(model, true);
                    //comment.Owner = await _dataContext.Owners.FindAsync(model.OwnerId);
                    _dataContext.Comments.Add(comment);
                    await _dataContext.SaveChangesAsync();
                    return RedirectToAction($"{nameof(DetailsContent)}/{model.ContentId}");
                }


                /*bool isNew = true;
                var comment = await _dataContext.Comments.FindAsync(model.Id);
                if (comment != null)
                {
                    comment.Id = isNew ? 0 : model.Id;
                    comment.Owner = await _dataContext.Owners.FindAsync(model.OwnerId);
                    comment.Content = await _dataContext.Contents.FindAsync(model.ContentId);
                    comment.Description = model.Description;
                    _dataContext.Comments.Update(comment);
                    await _dataContext.SaveChangesAsync();
                    return RedirectToAction($"{nameof(DetailsContent)}/{model.ContentId}");
                }
                */
            }

            return View(model);
        }

    }
}
