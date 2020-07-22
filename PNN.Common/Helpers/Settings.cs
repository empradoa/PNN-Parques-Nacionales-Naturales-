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
        private const string _isRemembered = "IsRemembered";
        private static readonly bool _boolDefault = false;

        private static readonly string _stringDefault = string.Empty;

        private static ISettings AppSettings => CrossSettings.Current;

        public static string Pubs
        {
            get => AppSettings.GetValueOrDefault( _pubs, _stringDefault );
            set => AppSettings.AddOrUpdateValue(  _pubs, value );
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

        public static bool IsRemembered
        {
            get => AppSettings.GetValueOrDefault(_isRemembered, _boolDefault);
            set => AppSettings.AddOrUpdateValue(_isRemembered, value);
        }

    }
}
