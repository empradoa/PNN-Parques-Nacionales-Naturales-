using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PNN.Common.Helpers
{
    public class Settings
    {

        private const string _user = "User";
        private const string _token = "Token";
        private const string _pubs = "Pubs";
        private const string _reactions = "Reactions";
        private const string _areas = "Areas";
        private const string _parkId = "ParkId";
        private const string _zoneId = "ZoneId";

        private const string _isRemembered = "IsRemembered";
        private static readonly bool _boolDefault = false;

        private const string _inicio = "Inicio";
        private static readonly bool _boolDeflt = false;



        private static readonly string _stringDefault = string.Empty;

        private static ISettings AppSettings => CrossSettings.Current;

        public static string Pubs
        {
            get => AppSettings.GetValueOrDefault( _pubs, _stringDefault );
            set => AppSettings.AddOrUpdateValue(  _pubs, value );
        }

        public static string Reactions
        {
            get => AppSettings.GetValueOrDefault(_reactions, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_reactions, value);
        }

        public static string Token
        {
            get => AppSettings.GetValueOrDefault( _token, _stringDefault );
            set => AppSettings.AddOrUpdateValue(  _token, value );
        }

        public static string User
        {
            get => AppSettings.GetValueOrDefault( _user, _stringDefault);
            set => AppSettings.AddOrUpdateValue(  _user, value);
        }

        public static string Areas
        {
            get => AppSettings.GetValueOrDefault(_areas, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_areas, value);
        }

        public static bool IsRemembered
        {
            get => AppSettings.GetValueOrDefault(_isRemembered, _boolDefault);
            set => AppSettings.AddOrUpdateValue(_isRemembered, value);
        }

        public static bool Inicio
        {
            get => AppSettings.GetValueOrDefault(_inicio, _boolDeflt);
            set => AppSettings.AddOrUpdateValue(_inicio, value);
        }

        public static string ParkId
        {
            get => AppSettings.GetValueOrDefault(_parkId, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_parkId, value);
        }

        public static string ZoneId
        {
            get => AppSettings.GetValueOrDefault(_zoneId, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_zoneId, value);
        }
    }
}
