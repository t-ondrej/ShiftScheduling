using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ShiftScheduleLibrary.Entities;
using ShiftScheduleLibrary.Utilities;
using static ShiftScheduleDataAccess.Dao.FileDao.FolderConstants;

namespace ShiftScheduleDataAccess.Dao.FileDao
{
    internal class FileResultingScheduleDao : FileClient, IResultingScheduleDao
    {
        public FileResultingScheduleDao(string folderPath) : base(folderPath)
        {
        }

        public IEnumerable<ResultingSchedule> GetResultingSchedules()
        {
            var resultFiles = Directory.EnumerateFiles(FolderPath, ResultingScheduleFilesPattern);
            var result = new List<ResultingSchedule>();

            foreach (var resultFile in resultFiles)
            {
                var specification = ExtractSpecification(resultFile);

                using (var textReader = GetTextReader(resultFile))
                {
                    try
                    {
                        string line;
                        var dailyShedules = new Dictionary<int, ResultingSchedule.DailySchedule>();

                        while ((line = textReader.ReadLine()) != null)
                        {
                            var dayId = int.Parse(line);
                            var scheduleForPersons = new Dictionary<int, Intervals<ShiftInterval>>();

                            while ((line = textReader.ReadLine()) != null && line != "")
                            {
                                var splited = line.Split(' ');
                                var personId = int.Parse(splited[0]);
                                var intervals = splited.Skip(1).Select(ShiftInterval.FromString).ToList();
                                scheduleForPersons.Add(personId, new Intervals<ShiftInterval>(intervals));
                            }

                            var dailySchedule = new ResultingSchedule.DailySchedule(scheduleForPersons);
                            dailyShedules.Add(dayId, dailySchedule);
                        }


                        result.Add(new ResultingSchedule(dailyShedules, specification));
                    }
                    catch
                    {
                        throw new Exception($"Unable to parse file: {resultFile}");
                    }
                }
            }

            return result;
        }

        public void SaveResultingSchedule(ResultingSchedule resultingSchedule)
        {
            if (resultingSchedule.Specification == null)
                throw new Exception("Specification must be set before saving the result.");

            var fileName = $"{ResultingSchedulePreffix}{resultingSchedule.Specification}{Extension}";
            var filePath = Path.Combine(FolderPath, fileName);

            using (var textWriter = GetTextWriter(filePath))
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

        private static string ExtractSpecification(string resultFile)
        {
            var fileName = Path.GetFileNameWithoutExtension(resultFile);
            var preffixLength = ResultingSchedulePreffix.Length;
            return fileName?.Substring(preffixLength);
        }
    }
}