using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using ShiftScheduleAlgorithm.ShiftAlgorithm.Core;
using ShiftScheduleDataAccess.Dao;
using ShiftScheduleUtilities;
using ShiftScheduleAlgorithm.ShiftAlgorithm.Validation;
using System.Diagnostics;

namespace ShiftScheduleAlgorithm
{
    internal class Program
    {
        private static void Main()
        {
            var settings = ConfigurationManager.AppSettings;

            // We get all the algorithm configurations
            var configurationsFolder = settings["ConfigurationsFolder"];
            var configFiles = GetConfigurationsWithNames(configurationsFolder);

            // We get all data access clients
            var testingDataFolder = settings["TestingDataFolder"];
            var testingDataSets = settings["TestingDataSets"];
            var dataAccessClients = GetDataAccessClients(testingDataFolder, testingDataSets).ToList();

            foreach (var fileNameToConfig in configFiles)
            {
                var configFileName = fileNameToConfig.Key;
                var algorithmConfiguration = fileNameToConfig.Value;

                foreach (var dataAccessClient in dataAccessClients)
                {
                    var persons = dataAccessClient.PersonDao.GetAllPersons();
                    var requirements = dataAccessClient.RequirementsDao.GetRequirements();
                    var algorithmInput = new AlgorithmInput(persons, requirements, null, algorithmConfiguration);
                    var result = AlgorithmExecutor.ExecuteAlgorithm(algorithmInput);
                    result.Specification = configFileName;
                    dataAccessClient.ResultingScheduleDao.SaveResultingSchedule(result);

                    var validator = new Validator(algorithmInput, result);
                    var validationOutput = validator.Validate();
                    var msg = validationOutput.GetMessage();
                    Debug.WriteLine($"Working folder:  {dataAccessClient.WorkingFolder} | Report: {msg}");
                    
                }
            }
        }

        private static IDictionary<string, AlgorithmConfiguration> GetConfigurationsWithNames(string folder)
        {
            return Directory.EnumerateFiles(folder, "*.config").ToDictionary
            (
                Path.GetFileNameWithoutExtension,
                configFile => ConfigurationReader<AlgorithmConfiguration>.ParseFile
                (
                    configFile, configuration => new AlgorithmConfiguration
                    {
                        AlgorithmStrategy = AlgorithmStrategyFactory.ParseStrategy(configuration["AlgorithmStrategy"]),
                        MaxDailyWork = Convert.ToInt32(configuration["MaxDailyWork"]),
                        MaxConsecutiveWorkHours = Convert.ToInt32(configuration["MaxConsecutiveWorkHours"]),
                        WorkerPauseLength = Convert.ToInt32(configuration["WorkerPauseLength"])
                    }
                )
            );
        }

        private static IEnumerable<DataAccessClient> GetDataAccessClients(string folder, string pattern)
        {
            return Directory.EnumerateDirectories(folder, pattern)
                .SelectMany(Directory.EnumerateDirectories)
                .Select(directory => new DataAccessClient(directory));
        }
    }
}