using System.Collections.Generic;
using System.IO;
using System.Linq;
using ShiftScheduleData.Entities;

namespace ShiftScheduleData.DataAccess.FileDao
{
    public class FileResultingScheduleDao : FileClient, IResultingScheduleDao
    {
        private readonly string _resultingScheduleFilePath;

        public FileResultingScheduleDao(string folderPath) : base(folderPath)
        {
            string fileName = $"{FolderConstants.ResultingScheduleFileName}.{FolderConstants.FileExtensions}";
            _resultingScheduleFilePath = Path.Combine(folderPath, fileName);
        }

        public ResultingScheduleOld GetResultingSchedule(IEnumerable<PersonOld> persons)
        {
            using (var textReader = GetTextReader(_resultingScheduleFilePath))
            {
                var personIdToPerson = persons.ToDictionary(p => p.Id, p => p);
                var dictionary = new Dictionary<PersonOld, Schedule>();
                string line;

                while ((line = textReader.ReadLine()) != null)
                {
                    var personId = int.Parse(line);
                    var person = personIdToPerson[personId];
                    var schedule = ScheduleParser.Get(textReader);
                    dictionary.Add(person, schedule);
                }

                return new ResultingScheduleOld(dictionary);
            }
        }

        public void SaveResultingSchedule(ResultingScheduleOld resultingScheduleOld)
        {
            using (var textWriter = GetTextWriter(_resultingScheduleFilePath))
            {
                foreach (var personSchedule in resultingScheduleOld.SchedulesForPeople)
                {
                    textWriter.WriteLine(personSchedule.Key.Id);
                    ScheduleParser.Put(textWriter, personSchedule.Value);
                    textWriter.WriteLine();
                }
            }
        }
    }
}