using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleDataAccess.Dao.FileDao
{
    internal class FileRequirementsDao : FileClient, IRequirementsDao
    {
        private readonly string _requirementFilePath;

        public FileRequirementsDao(string folderPath) : base(folderPath)
        {
            _requirementFilePath = Path.Combine(folderPath, FolderConstants.RequirementsFileName);
        }

        public Requirements GetRequirements()
        {
            using (var textReader = GetTextReader(_requirementFilePath))
            {
                try
                {
                    var dictionary = new Dictionary<int, Requirements.DailyRequirement>();
                    string line;

                    while ((line = textReader.ReadLine()) != null)
                    {
                        var splited = line.Split(' ');
                        var dayId = int.Parse(splited[0]);
                        var dailyRequirements = splited.Skip(1).Select(double.Parse).ToList();
                        dictionary.Add(dayId, new Requirements.DailyRequirement(dailyRequirements));
                    }

                    return new Requirements(dictionary);
                }
                catch
                {
                    throw new Exception($"Unable to parse file: {_requirementFilePath}");
                }
            }
        }

        public void SaveRequirements(Requirements requirements)
        {
            using (var textWriter = GetTextWriter(_requirementFilePath))
            {
                foreach (var dailyRequirement in requirements.DaysToRequirements)
                {
                    var dayId = dailyRequirement.Key;
                    var hourToWorkers = dailyRequirement.Value.HourToWorkers;
                    var hoursString = string.Join(" ", hourToWorkers);
                    textWriter.WriteLine($"{dayId} {hoursString}");
                }
            }
        }
    }
}