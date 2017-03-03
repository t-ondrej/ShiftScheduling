using System;
using System.Collections.Generic;
using System.IO;
using ShiftScheduleData.Entities;

namespace ShiftScheduleData.DataAccess.FileDao
{
    public class FilePersonDao : FileClient, IPersonDao
    {
        public FilePersonDao(string folderPath) : base(folderPath)
        {
        }

        public IEnumerable<Person> GetAllPersons()
        {
            var persons = new List<Person>();

            foreach (var file in GetFiles())
            {
                var fileName = Path.GetFileNameWithoutExtension(file);

                int id;

                if (!int.TryParse(fileName, out id))
                    continue;

                using (var textReader = GetTextReader(file))
                {
                    try
                    {
                        var line = textReader.ReadLine();

                        if (line == null)
                            throw new Exception("No content in the file.");

                        var maxHours = int.Parse(line);
                        var schedule = ScheduleParser.Get(textReader);
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
            string fileName = $"{person.Id}.{FolderConstants.FileExtensions}";
            var path = Path.Combine(FolderPath, fileName);

            using (var textWriter = GetTextWriter(path))
            {
                try
                {
                    textWriter.WriteLine(person.MaxHoursPerMonth);
                    ScheduleParser.Put(textWriter, person.MonthlySchedule);
                }
                catch
                {
                    throw new Exception("Unable to save person to the file: " + path);
                }
            }
        }
    }
}