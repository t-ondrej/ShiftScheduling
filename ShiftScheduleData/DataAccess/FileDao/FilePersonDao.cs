using System;
using System.Collections.Generic;
using System.IO;
using ShiftScheduleData.Entities;

namespace ShiftScheduleData.DataAccess.FileDao
{
    public class FilePersonDao : FileClient, IPersonDao
    {
        private const string RequirementsFileName = "requirements";

        public FilePersonDao(string folderPath) : base(folderPath)
        {
        }

        public IEnumerable<Person> GetAllPersons()
        {
            var persons = new List<Person>();

            foreach (var file in GetFiles())
            {
                var fileName = Path.GetFileNameWithoutExtension(file);

                if (fileName == RequirementsFileName)
                    continue;

                int id;

                if (!int.TryParse(fileName, out id))
                    throw new Exception("Incorrect file in the folder: " + FolderPath);

                using (var streamReader = GetStreamReader(file))
                {
                    try
                    {
                        var line = streamReader.ReadLine();

                        if (line == null)
                            throw new Exception("No content in the file.");

                        var maxHours = int.Parse(line);
                        var schedule = ScheduleParser.Get(streamReader);
                        persons.Add(new Person(id, schedule, maxHours));
                    }
                    catch
                    {
                        throw new Exception("Unable to parse file: " + file);
                    }
                }
            }

            return persons;
        }

        public void SavePerson(Person person)
        {
            throw new NotImplementedException();
        }
    }
}