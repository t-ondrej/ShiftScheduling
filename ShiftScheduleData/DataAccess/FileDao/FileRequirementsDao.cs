using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ShiftScheduleData.Entities;

namespace ShiftScheduleData.DataAccess.FileDao
{
    public class FileRequirementsDao : FileClient, IRequirementsDao
    {
        private readonly string _requirementFilePath;

        public FileRequirementsDao(string folderPath) : base(folderPath)
        {
            string fileName = $"{FolderConstants.RequirementsFileName}.{FolderConstants.FileExtensions}";
            _requirementFilePath = Path.Combine(folderPath, fileName);
        }

        public MonthlyRequirements GetRequirements()
        {
            using (var textReader = GetTextReader(_requirementFilePath))
            {
                try
                {
                    var dictionary = new Dictionary<int, MonthlyRequirements.DailyRequirement>();
                    string line;

                    while ((line = textReader.ReadLine()) != null)
                    {
                        var splited = line.Split(' ');
                        var dayId = int.Parse(splited[0]);
                        var dailyDictionary = new Dictionary<int, int>();
                        var hours = splited[1].Split(',');

                        for (var index = 0; index < hours.Length; index++)
                        {
                            var value = int.Parse(hours[index]);
                            dailyDictionary.Add(index, value);
                        }

                        dictionary.Add(dayId, new MonthlyRequirements.DailyRequirement(dailyDictionary));
                    }

                    return new MonthlyRequirements(dictionary);
                }
                catch
                {
                    throw new Exception("Couldn't parse monthlyRequirements from the file: " + _requirementFilePath);
                }
            }
        }

        public void SaveRequirements(MonthlyRequirements monthlyRequirements)
        {
            using (var textWriter = GetTextWriter(_requirementFilePath))
            {
                foreach (var dailyRequirement in monthlyRequirements.DaysToRequirements)
                {
                    var dayId = dailyRequirement.Key;
                    var hourToWorkers = dailyRequirement.Value.HourToWorkers;
                    var maxHour = hourToWorkers.Keys.Max();
                    var stringBuilder = new StringBuilder();

                    for (var i = 0; i <= maxHour; i++)
                    {
                        stringBuilder.Append(hourToWorkers.ContainsKey(i) ? $"{hourToWorkers[i]}," : "0,");
                    }

                    var hoursString = stringBuilder.ToString().Substring(0, stringBuilder.Length - 1);
                    textWriter.WriteLine($"{dayId} {hoursString}");
                }
            }
        }
    }
}