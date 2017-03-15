using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using ShiftScheduleGenerator.Generation;
using ShiftScheduleUtilities;

namespace ShiftScheduleGenerator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var settings = ConfigurationManager.AppSettings;
            var generatedDataFolder = settings["GeneratedDataFolder"];
            var configurationsFolder = settings["ConfigurationsFolder"];
            var dataSetFolderName = settings["DataSetFolderName"];

            foreach (var configFile in Directory.EnumerateFiles(configurationsFolder, "*.config"))
            {
                var generatorConfiguration = ConfigurationReader<GeneratorConfiguration>.ParseFile
                (
                    configFile, configuration => new GeneratorConfiguration
                    { 
                        ScheduleDaysCount = Convert.ToInt32(configuration["ScheduleDaysCount"]),
                        DayAssignmentDensity = Convert.ToDouble(configuration["DayAssignmentDensity"]),
                        WorkingTimePerMonthMin = Convert.ToInt32(configuration["WorkingTimePerMonthMin"]),
                        WorkingTimePerMonthMax = Convert.ToInt32(configuration["WorkingTimePerMonthMax"]),
                        WorkingTimePerDay =  Convert.ToInt32(configuration["WorkingTimePerDay"]),
                        NumberOfShiftWeightValues = Convert.ToInt32(configuration["NumberOfShiftWeightValues"]),
                        EmployeeCount = Convert.ToInt32(configuration["EmployeeCount"]),
                        NumberOfSets = Convert.ToInt32(configuration["NumberOfSets"]),
                        DifficultyToFulfilRequirements = new Difficulty(configuration["DifficultyToFulfilRequirements"]),
                        ToleranceAssignmentProbability = Convert.ToDouble(configuration["ToleranceAssignmentProbability"]),
                        ToleranceUseProbability = Convert.ToDouble(configuration["ToleranceUseProbability"]),
                    }
                );

                var folderName = Path.GetFileNameWithoutExtension(configFile);
                Debug.Assert(folderName != null, "folderName != null");
                var workingFolder = Path.Combine(generatedDataFolder, folderName);
                Directory.CreateDirectory(workingFolder);
                new Generator(generatorConfiguration, workingFolder, dataSetFolderName).GenerateData();
            }
        }
    }
}