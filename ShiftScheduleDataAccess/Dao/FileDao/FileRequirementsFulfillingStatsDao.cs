using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ShiftScheduleLibrary.Entities;
using ShiftScheduleUtilities;

namespace ShiftScheduleDataAccess.Dao.FileDao
{
    internal class FileRequirementsFulfillingStatsDao : FileClient, IRequirementsFulfillingStatsDao
    {
        private readonly string _statisticsFilePath;

        public FileRequirementsFulfillingStatsDao(string folderPath) : base(folderPath)
        {
            string fileName = $"{FolderConstants.RequirementsFulfillingStatsFileName}.{FolderConstants.FileExtensions}";
            _statisticsFilePath = Path.Combine(folderPath, fileName);
        }

        public RequirementsFulfillingStats GetRequirements()
        {
            using (var textReader = GetTextReader(_statisticsFilePath))
            {
                try
                {
                    var personsToStats = new Dictionary<int, RequirementsFulfillingStats.PersonStats>();
                    string line;

                    while ((line = textReader.ReadLine()) != null)
                    {
                        var dictionary = new Dictionary<int, double>();
                        var splited = line.Split(' ');
                        var personId = int.Parse(splited[0]);

                        splited.Skip(1).ForEach(periodToStatsString =>
                        {
                            var splitedStats = periodToStatsString.Split('=');
                            var pediodId = int.Parse(splitedStats[0]);
                            var statsValue = double.Parse(splitedStats[1]);
                            dictionary.Add(pediodId, statsValue);
                        });

                        personsToStats.Add(personId, new RequirementsFulfillingStats.PersonStats(dictionary));
                    }

                    return new RequirementsFulfillingStats(personsToStats);
                }
                catch
                {
                    throw new Exception($"Unable to parse file: {_statisticsFilePath}");
                }
            }
        }

        public void SaveRequirements(RequirementsFulfillingStats requirements)
        {
            using (var textWriter = GetTextWriter(_statisticsFilePath))
            {
                foreach (var personStats in requirements.PersonsStats)
                {
                    var personId = personStats.Key;
                    var statsStrings = personStats.Value.PeriodToFulfilling.Select(pair => $"{pair.Key}={pair.Value}");
                    var statsString = string.Join(" ", statsStrings);
                    textWriter.WriteLine($"{personId} {statsString}");
                }
            }
        }
    }
}