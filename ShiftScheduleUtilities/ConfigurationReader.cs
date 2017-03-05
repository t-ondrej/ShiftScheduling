using System;
using System.Configuration;

namespace ShiftScheduleUtilities
{
    public static class ConfigurationReader<T>
    {
        public class Configuration
        {
            private readonly KeyValueConfigurationCollection _configurationCollection;

            internal Configuration(KeyValueConfigurationCollection configurationCollection)
            {
                _configurationCollection = configurationCollection;
            }

            public string this[string propertyName]
            {
                get
                {
                    var keyValueConfigurationElement = _configurationCollection[propertyName];

                    if (keyValueConfigurationElement == null)
                        throw new NullReferenceException("No such key in the app settings section: " + propertyName);

                    return keyValueConfigurationElement.Value;
                }
            }
        }

        public static T ParseFile(string path, Func<Configuration, T> parser)
        {
            Configuration configuration;

            try
            {
                configuration = CreateConfiguration(path);
            }
            catch
            {
                throw new Exception("Invalid configuration file path: " + path);
            }

            return parser(configuration);
        }

        private static Configuration CreateConfiguration(string configPath)
        {
            var map = new ExeConfigurationFileMap {ExeConfigFilename = configPath};
            var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            var section = (AppSettingsSection) config.GetSection("appSettings");
            return new Configuration(section.Settings);
        }
    }
}