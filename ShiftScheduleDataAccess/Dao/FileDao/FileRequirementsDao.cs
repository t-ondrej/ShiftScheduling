using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ShiftScheduleLibrary.Entities;
using ShiftScheduleUtilities;

namespace ShiftScheduleDataAccess.Dao.FileDao
{
    internal class FileRequirementsDao : FileClient, IRequirementsDao
    {
        private readonly string _requirementFilePath;

        public FileRequirementsDao(string folderPath) : base(folderPath)
        {
            string fileName = $"{FolderConstants.RequirementsFileName}.{FolderConstants.FileExtensions}";
            _requirementFilePath = Path.Combine(folderPath, fileName);
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
                        var dailyDictionary = splited.Skip(1).ToDictionary((s, i) => i, (s, i) => double.Parse(s));
                        dictionary.Add(dayId, new Requirements.DailyRequirement(dailyDictionary));
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
                    var maxHour = hourToWorkers.Keys.Max();
                    var stringBuilder = new StringBuilder();

                    for (var i = 0; i <= maxHour; i++)
                    {
                        stringBuilder.Append(hourToWorkers.ContainsKey(i) ? $"{hourToWorkers[i]} " : "0 ");
                    }

                    var hoursString = stringBuilder.ToString().Substring(0, stringBuilder.Length - 1);
                    textWriter.WriteLine($"{dayId} {hoursString}");
                }
            }
        }
    }
}