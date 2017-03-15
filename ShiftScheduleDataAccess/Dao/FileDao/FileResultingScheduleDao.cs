using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ShiftScheduleLibrary.Entities;
using ShiftScheduleLibrary.Utilities;

namespace ShiftScheduleDataAccess.Dao.FileDao
{
    internal class FileResultingScheduleDao : FileClient, IResultingScheduleDao
    {
        private readonly string _resultingScheduleFilePath;

        public FileResultingScheduleDao(string folderPath) : base(folderPath)
        {
            string fileName = $"{FolderConstants.ResultingScheduleFileName}.{FolderConstants.FileExtensions}";
            _resultingScheduleFilePath = Path.Combine(folderPath, fileName);
        }

        public ResultingSchedule GetResultingSchedule()
        {
            using (var textReader = GetTextReader(_resultingScheduleFilePath))
            {
                try
                {
                    string line;
                    var dailyShedules = new Dictionary<int, ResultingSchedule.DailySchedule>();

                    while ((line = textReader.ReadLine()) != null)
                    {
                        var dayId = int.Parse(line);
                        var scheduleForPersons = new Dictionary<int, Intervals<ResultingSchedule.ShiftInterval>>();

                        while ((line = textReader.ReadLine()) != null && line != "")
                        {
                            var splited = line.Split(' ');
                            var personId = int.Parse(splited[0]);
                            var intervals = splited.Skip(1).Select(ResultingSchedule.ShiftInterval.FromString).ToList();
                            scheduleForPersons.Add(personId, new Intervals<ResultingSchedule.ShiftInterval>(intervals));
                        }

                        var dailySchedule = new ResultingSchedule.DailySchedule(scheduleForPersons);
                        dailyShedules.Add(dayId, dailySchedule);
                    }

                    return new ResultingSchedule(dailyShedules);
                }
                catch
                {
                    throw new Exception($"Unable to parse file: {_resultingScheduleFilePath}");
                }
            }
        }

        public void SaveResultingSchedule(ResultingSchedule resultingSchedule)
        {
            using (var textWriter = GetTextWriter(_resultingScheduleFilePath))
            {
                foreach (var dayTodailySchedule in resultingSchedule.DailySchedules)
                {
                    var dayId = dayTodailySchedule.Key;
                    var dailySchedule = dayTodailySchedule.Value;
                    textWriter.WriteLine(dayId);

                    foreach (var personSchedule in dailySchedule.PersonIdToDailySchedule)
                    {
                        var personId = personSchedule.Key;
                        var intervals = personSchedule.Value.Select(interval => $"{interval}");
                        var intervalsString = string.Join(" ", intervals);
                        textWriter.WriteLine($"{personId} {intervalsString}");
                    }

                    textWriter.WriteLine();
                }
            }
        }
    }
}