using System.Linq;
using System.Threading.Tasks;
using PNN.web.Data;
using PNN.web.Data.Entities;
using PNN.Web.Data.Entities;
using PNN.Web.Helpers;

namespace PNN.Web.Data
{
    public class SeedDb
    {
        private readonly DataContext _dataContext;
        private readonly IUserHelper _userHelper;

        //Clase que permite alimentar la bd
        public SeedDb(
            DataContext context,
            IUserHelper userHelper)
        {
            //context es la instancia de la BD en el startup
            _dataContext = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            await _dataContext.Database.EnsureCreatedAsync();
            await CheckRoles();
            await CheckLocationsAsync();
            var manager = await CheckUserAsync("Eider", "Prado", "empradoa@gmail.com", "3506342747", "Calle Luna Calle Sol", "Admin", "empradoa_1");
            var customer = await CheckUserAsync("Manuel", "Avendaño", "eiderprado@hotmail.com", "3506342747", "Calle Luna Calle Sol","Customer", "eiderprado_1");
            var visit = await CheckUserAsync("Visit", "Visitante", "visit@hotmail.com", "350 634 2747", "Calle Luna Calle Sol","Visit", "visit_0");
            await CheckZoneTypesAsync();
            await CheckContentTypesAsync();
            await CheckOwnerAsync(customer);
            await CheckManagerAsync(manager);
            await CheckVisitAsync(visit);
            await CheckParksAsync();
        }

        private async Task CheckRoles()
        {
            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Customer");
            await _userHelper.CheckRoleAsync("Visit");
        }

        private async Task<User> CheckUserAsync(string firstName, string lastName, string email, string phone, string address, string role, string alias)
        {
            var user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    CellPhone = phone,
                    Address = address,
                    Alias = alias
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, role);
            }

            return user;
        }

        private async Task CheckZoneTypesAsync()
        {
            if (!_dataContext.ZoneTypes.Any())
            {
                _dataContext.ZoneTypes.Add(new ZoneType { Name = "Rio" });
                _dataContext.ZoneTypes.Add(new ZoneType { Name = "Bosque" });
                _dataContext.ZoneTypes.Add(new ZoneType { Name = "Vereda" });
                _dataContext.ZoneTypes.Add(new ZoneType { Name = "Laguna" });
                await _dataContext.SaveChangesAsync();
            }
        }

        private async Task CheckLocationsAsync()
        {
            if (!_dataContext.Locations.Any())
            {
                _dataContext.Locations.Add(new Location { Latitude = 79.364664, Longitude = -14.23565 });
                _dataContext.Locations.Add(new Location { Latitude = 69.364664, Longitude = 24.23565 });
                await _dataContext.SaveChangesAsync();
            }
        }
        private async Task CheckContentTypesAsync()
        {
            if (!_dataContext.ContentTypes.Any())
            {
                _dataContext.ContentTypes.Add(new ContentType { Name = "Testimonio" });
                _dataContext.ContentTypes.Add(new ContentType { Name = "Relato" });
                _dataContext.ContentTypes.Add(new ContentType { Name = "Historia" });
                _dataContext.ContentTypes.Add(new ContentType { Name = "Denuncia" });
                await _dataContext.SaveChangesAsync();
            }
        }
        private async Task CheckOwnerAsync(User user)
        {
            if (!_dataContext.Owners.Any())
            {
                _dataContext.Owners.Add(new Owner { User = user });
                await _dataContext.SaveChangesAsync();
            }
        }

        private async Task CheckManagerAsync(User user)
        {
            if (!_dataContext.Managers.Any())
            {
                _dataContext.Managers.Add(new Manager { User = user });
                await _dataContext.SaveChangesAsync();
            }
        }
        private async Task CheckVisitAsync(User user)
        {
            if (!_dataContext.Managers.Any())
            {
                _dataContext.Managers.Add(new Manager { User = user });
                await _dataContext.SaveChangesAsync();
            }
        }
        private async Task CheckParksAsync()
        {
            var manager = _dataContext.Managers.FirstOrDefault();
            var location = _dataContext.Locations.FirstOrDefault();
            if (!_dataContext.Parks.Any())
            {
                AddPark("Prefiero no registrar el parque", "", "2020", "", "", "", "", "", "", "", manager, location);
                await _dataContext.SaveChangesAsync();
            }
        }
        private void AddPark(string name, string description, string creation, string been, string extension, string height, string temperature, string flora, string wildlife, string communities, Manager manager, Location location)
        {
            _dataContext.Parks.Add(new Park
            {
                Name = name,
                Description = description,
                Creation = creation,
                Been = been,
                Extension = extension,
                Height = height,
                Temperature = temperature,
                Flora = flora,
                Wildlife = wildlife,
                Communities = communities,
                Like = 0,
                DisLike = 0,
                Manager = manager,
                Location = location
            });
        }
    }

}
