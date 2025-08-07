using Microsoft.Extensions.Configuration;

namespace Infrastructure.SharedKernel
{
    /// <summary>
    /// AppHelper,for reading appsettings.json or other configuration files.
    /// </summary>
    public class AppHelper
    {
        private static IConfiguration _config;

        public AppHelper(IConfiguration configuration)
        {
            _config = configuration;
        }
        /// <summary>
        /// get session configuration value from appsettings.json
        /// </summary>
        /// <param name="sessions"></param>
        /// <returns></returns>
        public static string ReadAppSetting(string sessions)
        {
            try
            {
                return _config[sessions];

            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        ///  get session configuration value from appsettings.json by sub sections.
        /// </summary>
        /// <param name="sessions"></param>
        /// <returns></returns>
        public static string ReadAppSettings(params string[] sessions)
        {
            try
            {
                if (sessions.Any())
                {
                    return _config[string.Join(":", sessions)];
                }
            }
            catch
            {
                return "";
            }
            return "";
        }
        public static List<IConfigurationSection> ReadAppSettingsSection(params string[] sessions)
        {
            try
            {
                if (sessions.Any())
                {
                    return _config.GetSection(string.Join(":", sessions)).GetChildren().ToList();
                }
            }
            catch
            {
                return default;
            }
            return default;
        }

      
        public static T? ReadAppSettingsSection<T>(params string[] sessions)
        {
            try
            {
                if (sessions.Any())
                {
                    return _config.GetSection(string.Join(":", sessions)).Get<T>();
                }
            }
            catch
            {
                return default;
            }
            return default;
        }


    }
}
