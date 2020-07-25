using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PNN.Common.Models;
using PNN.web.Data;
using PNN.web.Data.Entities;
using PNN.Web.Data.Entities;
using PNN.Web.Models;

namespace PNN.Web.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _dataContext;
        private readonly ICombosHelper _combosHelper;

        public ConverterHelper(
            DataContext dataContext,
            ICombosHelper combosHelper)
        {
            _dataContext = dataContext;
            _combosHelper = combosHelper;
        }
        //convierte un objeto de la clase ContentViewModel en la clase Content
        public async Task<Content> ToContentAsync(ContentViewModel model, string path, bool isNew)
        {
            var content = new Content
            {
                Comments = model.Comments,
                Id = isNew ? 0 : model.Id,
                Description = model.Description,
                Date = DateTime.Now,
                ImageUrl = path,
                Like = model.Like,
                DisLike = model.DisLike,
                User = await _dataContext.Users.FindAsync(model.UserId),
                Park = await _dataContext.Parks.FindAsync(model.ParkId),
                ContentType = await _dataContext.ContentTypes.FindAsync(model.ContentTypeId)
                //Location = await _dataContext.Locations.FindAsync(model.LocationId)
            };
            return content;
        }

        //editar publicaciones
        public ContentViewModel ToContentViewModel(Content content)
        {
            return new ContentViewModel
            {
                Description = content.Description,
                Date = content.Date,
                ImageUrl = content.ImageUrl,
                Like = content.Like,
                DisLike = content.DisLike,
                Comments = content.Comments,
                User = content.User,
                ContentType = content.ContentType,
                Park = content.Park,
                //Location = content.Location,
                Id = content.Id,
                UserId = content.User.Id,
                ContentTypeId = content.ContentType.Id,
                ContentTypes = _combosHelper.GetComboContentTypes(),
                ParkId = content.Park.Id,
                Parks = _combosHelper.GetComboParks()
                //LocationId = content.Location.Id               
            };
        }
        //metodo para agregar comentario desde contenido o publicación de CommentViewModel a Comment
        public async Task<Comment> ToCommentToContentAsync(CommentViewModel model, bool isNew)
        {

            return new Comment
            {
                Date = DateTime.Now,
                Description = model.Description,
                Id = isNew ? 0 : model.Id,
                Content = await _dataContext.Contents.FindAsync(model.ContentId),
                User = await _dataContext.Users.FindAsync(model.UserId)
            };
        }

        //metodo para agregar comentario desde contenido o publicación de Comment a CommentViewModel
        public CommentViewModel ToCommentToContentViewModel(Comment comment)
        {

            return new CommentViewModel
            {
                Date = comment.Date,
                Description = comment.Description,
                Id = comment.Id,
                ContentId = comment.Content.Id,
                UserId = comment.User.Id
            };
        }

        //metodo para agregar comentario desde contenido o publicación de CommentViewModel a Comment
        public async Task<Comment> CommentToContentAsync(Comment comment, bool isNew)
        {

            return new Comment
            {
                Date = DateTime.Now,
                Description = comment.Description,
                Id = isNew ? 0 : comment.Id,
                Content = await _dataContext.Contents.FindAsync(comment.Content),
                User = await _dataContext.Users.FindAsync(comment.User)
            };
        }

        //convierte un objeto de la clase ContentViewModel en la clase Content
        public async Task<Park> ToParkAsync(ParkViewModel model, string path, bool isNew)
        {
            var park = new Park
            {
                Name = model.Name,
                Id = isNew ? 0 : model.Id,
                Description = model.Description,
                Creation = model.Creation,
                ImageUrl = path,
                Been = model.Been,
                Extension = model.Extension,
                Height = model.Height,
                Temperature = model.Temperature,
                Flora = model.Flora,
                Wildlife = model.Wildlife,
                Communities = model.Communities,
                Like = 0,
                DisLike = 0,
                Manager = await _dataContext.Managers.FindAsync(model.ManagerId)
                //Location = await _dataContext.Locations.FindAsync(model.LocationId)
            };

            if (model.latitud != null && model.longuitud != null)
            {
                park.Locations = new List<Area> { new Area {
                                 Location = new Location{
                                    Latitude  = double.Parse(model.latitud,CultureInfo.InvariantCulture),
                                    Longitude = double.Parse(model.longuitud,CultureInfo.InvariantCulture)
                                                      },
                                 Park = park }
                            };
            }

            return park;
        }

        //eiditar park
        public ParkViewModel ToParkViewModel(Park park)
        {
            return new ParkViewModel
            {
                Name = park.Name,
                Id = park.Id,
                Description = park.Description,
                Creation = park.Creation,
                ImageUrl = park.ImageUrl,
                Been = park.Been,
                Extension = park.Extension,
                Height = park.Height,
                Temperature = park.Temperature,
                Flora = park.Flora,
                Wildlife = park.Wildlife,
                Communities = park.Communities,
                ManagerId = park.Manager.Id,
                //LocationId = content.Location.Id   
                latitud = park.Locations?.FirstOrDefault().Location.Latitude.ToString(),
                longuitud = park.Locations?.FirstOrDefault().Location.Longitude.ToString()
            };
        }


        public Zone ToZoneAsync(ZoneViewModel zone, bool isNew)
        {
            var z = new Zone
            {
                Id = isNew ? 0 : zone.Id,
                Nombre = zone.Nombre,
                ImageUrl = zone.ImageUrl,
                Description = zone.Description,
                Like = zone.Like,
                DisLike = zone.DisLike,
                ZoneType = zone.ZoneType,
                Park = zone.Park,
                Manager = zone.Manager,
                Comments = zone.Comments
            };


            if (zone.latitud != null && zone.longuitud != null)
            {
                z.Locations = new List<Area> { new Area {
                                 Location = new Location{ 
                                    Latitude  = double.Parse(zone.latitud,CultureInfo.InvariantCulture),
                                    Longitude = double.Parse(zone.longuitud,CultureInfo.InvariantCulture)
                                                      },
                                 Zone = z }
                            };
            }

            return z;
        }

        public ZoneViewModel ToZoneViewModel(Zone zone)
        {
            return new ZoneViewModel {
                Id          = zone.Id,
                Nombre      = zone.Nombre,
                ImageUrl    = zone.ImageUrl,
                Description = zone.Description,
                Like        = zone.Like,
                DisLike     = zone.DisLike,
                ZoneType    = zone.ZoneType,
                Locations   = zone.Locations,
                Park        = zone.Park,
                Manager     = zone.Manager,
                Comments    = zone.Comments,
                latitud     = zone.Locations?.FirstOrDefault().Location.Latitude.ToString(),
                longuitud   = zone.Locations?.FirstOrDefault().Location.Longitude.ToString()
            };
        }



        //  conversiones de serializacion y deserializacion de los Api's

        public ICollection<ParkResponse> ToListParkResponse(ICollection<Park> prk)
        {
            List<ParkResponse> Parks = new List<ParkResponse>();

            if (prk != null && prk.Count != 0)
            {

                foreach (var p in prk)
                {
                    Parks.Add(ToParkResponse(p));
                }

            }

            return Parks;
        }

        public ParkResponse ToParkResponse(Park p)
        {
            ParkResponse prk = new ParkResponse();

            if (p != null)
            {
                prk = new ParkResponse
                {
                    Id   = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Creation = p.Creation,
                    ImageUrl = p.ImageUrl,
                    Been = p.Been,
                    Extension = p.Extension,
                    Height = p.Height,
                    Temperature = p.Temperature,
                    Flora = p.Flora,
                    Wildlife = p.Wildlife,
                    Communities = p.Communities,
                    Like = p.Like,
                    DisLike = p.DisLike,
                    Manager = ToManagerResponse(p.Manager),
                    Location = ToListAreaResponse(p.Locations),
                    Contents = ToListContentResponse(p.Contents),
                    Zones = ToListZoneResponse(p.Zones)
                };
            }

            return prk;
        }   

        public ICollection<ZoneResponse> ToListZoneResponse(ICollection<Zone> zn)
        {
            var zones = new List<ZoneResponse>();

            if (zn != null && zn.Count != 0)
            {

                foreach (var z in zn)
                {
                    zones.Add(ToZoneResponse(z));
                }

            }

            return zones;
        }

        public ZoneResponse ToZoneResponse(Zone z)
        {
            return (z == null ? new ZoneResponse
                                {
                                    Id = z.Id,
                                    Nombre = z.Nombre,
                                    ImageUrl = z.ImageUrl,
                                    Description = z.Description,
                                    Like = z.Like,
                                    DisLike = z.DisLike,
                                    ZoneType = ToZoneTyperespone(z.ZoneType),
                                    Location = ToListAreaResponse(z.Locations),
                                    Manager = ToManagerResponse(z.Manager),
                                    Comments = ToListCommentsResponse(z.Comments)
                                }
                                : new ZoneResponse { });
        }

        public ZoneTypesResponse ToZoneTyperespone(ZoneType zt)
        {
            return (zt != null ? new ZoneTypesResponse
                                {
                                    Id = zt.Id,
                                    Name = zt.Name
                                }
                                : new ZoneTypesResponse { });
        }

        public User ToUser(UserResponse u)
        {
            return (u != null ? new User
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Address = u.Address,
                CellPhone = u.CellPhone,
                Email = u.Email,
                Contents = ToListContent(u.Contents)
            }
                                : new User { });
        }

        public UserResponse ToUserResponse(User u)
        {
            return (u != null ? new UserResponse
                                {
                                    Id = u.Id,
                                    FirstName = u.FirstName,
                                    LastName = u.LastName,
                                    Address = u.Address,
                                    CellPhone = u.CellPhone,
                                    Email = u.Email,
                                    Contents = ToListContentResponse(u.Contents)
                                }
                                : new UserResponse { });
        }   

        public ICollection<Content> ToListContent(ICollection<ContentResponse> cont)
        {
            var contents = new List<Content>();

            if (cont != null && cont.Count != 0)
            {
                foreach (var c in cont)
                {
                    contents.Add(ToContent(c));
                }
            }

            return contents;
        }

        public Content ToContent(ContentResponse c)
        {
            return (c != null ? new Content
                                {
                                    Id = c.Id,
                                    Description = c.Description,
                                    Date = c.Date,
                                    ImageUrl = c.ImageUrl,
                                    Like = c.Like,
                                    ContentType = ToContentType(c.ContentType),
                                    Park = _dataContext.Parks
                                                        .Include(p => p.Manager)
                                                        .ThenInclude(p => p.User)
                                                        .Include (p=>  p.Locations)
                                                        .Include(p=> p.Contents)
                                                        .ThenInclude(cnt => cnt.FirstOrDefault().ContentType)
                                                        .Include(p=> p.Zones)
                                                        .ThenInclude(z=> z.FirstOrDefault().ZoneType)
                                                        .FirstOrDefault(p => p.Name.ToLower() == c.Park.ToLower() ),
                                    Comments = ToListComments(c.Comments)

                                }
                                : new Content { });
        }

        public ICollection<ContentResponse> ToListContentResponse(ICollection<Content> cont)
        {
            var contents = new List<ContentResponse>();

            if (cont != null && cont.Count != 0)
            {
                foreach (var c in cont)
                {
                    contents.Add(ToContentResponse(c));
                }
            }

            return contents;
        }

        public ContentResponse ToContentResponse(Content c)
        {
            return (c != null ? new ContentResponse
                                {
                                    Id = c.Id,
                                    Description = c.Description,
                                    Date = c.Date,
                                    ImageUrl = c.ImageUrl,
                                    Like = c.Like,
                                    ContentType = ToContentTypeResponse(c.ContentType),
                                    Park = c.Park.Name,
                                    Comments = ToListCommentsResponse(c.Comments),
                                    FullName = c.User.FullName,
                                    UserAlias = c.User.Alias
                                }
                                : new ContentResponse { });
        }

        public ICollection<Comment> ToListComments(ICollection<CommentResponse> cmm)
        {
            var comments = new List<Comment>();

            if (cmm != null && cmm.Count != 0)
            {
                foreach (var c in cmm)
                {
                    comments.Add(ToComment(c));
                }
            }

            return comments;
        }

        public Comment ToComment(CommentResponse cmm)
        {
            return (cmm != null ? new Comment
                                    {
                                        Id = cmm.Id,
                                        Description = cmm.Description,
                                        Date = cmm.Date,
                                        Like = cmm.Like,
                                        User = _dataContext.Users.FirstOrDefault(u => u.Id == cmm.User)
                                    }
                                  : new Comment { });
        }

        public ICollection<CommentResponse> ToListCommentsResponse(ICollection<Comment> cmm)
        {
            var comments = new List<CommentResponse>();

            if (cmm != null && cmm.Count != 0)
            {
                foreach (var c in cmm)
                {
                    comments.Add(ToCommentResponse(c));
                }
            }

            return comments;
        }

        public CommentResponse ToCommentResponse(Comment cmm)
        {
            return (cmm != null ? new CommentResponse
                                    {
                                        Id = cmm.Id,
                                        Description = cmm.Description,
                                        Date = cmm.Date,
                                        Like = cmm.Like,
                                        FullName = cmm.User?.FullName,
                                        User = cmm.User?.Id
                                    }
                                  : new CommentResponse { });
        }

        public ContentType ToContentType(ContentTypeResponse ct)
        {
            return (ct != null ? new ContentType
                                {
                                    Id = ct.Id,
                                    Name = ct.Name
                                }
                                 : new ContentType { });
        }

        public ContentTypeResponse ToContentTypeResponse(ContentType ct)
        {
            return (ct != null ? new ContentTypeResponse
                                {
                                    Id = ct.Id,
                                    Name = ct.Name
                                }
                                : new ContentTypeResponse { });
        }

        public ICollection<AreaResponse> ToListAreaResponse(ICollection<Area> Ar)
        {
            var Areas = new List<AreaResponse>();

            if (Ar != null && Ar.Count != 0)
            {

                foreach (var a in Ar)
                {
                    Areas.Add(ToAreaResponse(a));
                }

            }

            return Areas;
        }

        public AreaResponse ToAreaResponse(Area ar)
        {
            return (ar != null ? new AreaResponse
                                {
                                    Id = ar.Id,
                                    Location = ToLocationResponse(ar.Location),
                                    Name = ar.Park != null ? ar.Park.Name : ar.Zone.Nombre,
                                    Park = ar.Park != null ? ar.Park.Id : default,
                                    Zone = ar.Zone != null ? ar.Zone.Id : default
                                }
                                : new AreaResponse { });
        }

        public LocationResponse ToLocationResponse(Location l)
        {
            return (l!=null ? new LocationResponse 
                            {
                            Id = l.Id,
                            Latitude = l.Latitude,
                            Longitude = l.Longitude
                            }
                            : new LocationResponse { });
        }

        public ManagerResponse ToManagerResponse(Manager manager)
        {
            return (manager != null ? new ManagerResponse
                                 {
                                     Id = manager.Id,
                                     User = manager.User.Id,
                                     FullName = manager.User.FullName
                                 }
                                 : new ManagerResponse { });
        }

        
    }
}
