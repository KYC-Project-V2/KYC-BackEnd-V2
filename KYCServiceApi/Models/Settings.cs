using Model;
using Newtonsoft.Json;

namespace KYCServiceApi.Models
{
    public static class Settings
    {
        public static AppSettings appSettings
        {
            get
            {
                if (_appSettings == null)
                {
                    _appSettings = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText("appsettings.json"));
                }
                return _appSettings;
            }
        }
        private static AppSettings _appSettings;

    }
}
