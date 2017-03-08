using System;
using System.Configuration;
using System.IO;
using ShiftScheduleUtilities;

namespace ShiftScheduleGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var settings = ConfigurationManager.AppSettings;
            var generatedDataFolder = settings["generatedDataFolder"];
            var configurationsFolder = settings["configurationsFolder"];

            foreach (var configFile in Directory.EnumerateFiles(configurationsFolder, "*.config"))
            {
                var generatorConfiguration = ConfigurationReader<GeneratorConfiguration>.ParseFile
                (
                    configFile, configuration => new GeneratorConfiguration
                    (
                        Convert.ToInt32(configuration["ScheduleDaysCount"]),
                        Convert.ToInt32(configuration["WorkingTimeLength"]),
                        Convert.ToInt32(configuration["WorkingTimePerMonthMax"]),
                        Convert.ToInt32(configuration["EmployeeCount"]),
                        configuration["FolderName"],
                        Convert.ToInt32(configuration["NumberOfSets"])
                    )
                );

                var workingFolder = Path.Combine(generatedDataFolder, generatorConfiguration.FolderName);
                Directory.CreateDirectory(workingFolder);
                new Generator(generatorConfiguration, workingFolder).GenerateData();
            }
        }
    }
}