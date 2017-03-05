using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"C:\Users\Patrik Bak\Documents\Visual Studio 2015\Projects\ShiftSchedule\ShiftScheduleGenerator\App.config";

            ExeConfigurationFileMap map = new ExeConfigurationFileMap();
            map.ExeConfigFilename = path;

            var config = ConfigurationManager.OpenMappedExeConfiguration(map, ConfigurationUserLevel.None);
            var section = (AppSettingsSection) config.GetSection("appSettings");
            Console.WriteLine(section.Settings["test"].Value);
        }
    }
}