using PNN.Common.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PNN.Prism.ViewModels
{
    public class CnpMasterDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        //private ObservableCollection<MenuItemViewModel> _menus;

        public CnpMasterDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            LoadMenus();
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        private void LoadMenus()
        {
            var menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "ic_home",
                    PageName = "PubsPage",
                    Title = "Publicaciones"
                },

                new Menu
                {
                    Icon = "ic_landscape",
                    PageName = "ParksPage",
                    Title = "Parques"
                },

                new Menu
                {
                    Icon = "ic_pin_drop",
                    PageName = "MapPage",
                    Title = "Mapa"
                },

                new Menu
                {
                    Icon = "ic_person_pin",
                    PageName = "ModifyUserPage",
                    Title = "Modificar Datos"
                },

                new Menu
                {
                    Icon = "ic_content_paste",
                    PageName = "NotificationPage",
                    Title = "Encuesta"
                },

                new Menu
                {
                    Icon = "ic_exit_to_app",
                    PageName ="InitialPage",
                    Title = "Cerrar Sesion"
                }
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title

                }).ToList());
        }

    }
}
