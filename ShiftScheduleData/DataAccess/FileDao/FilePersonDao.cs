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

        public IEnumerable<PersonOld> GetAllPersons()
        {
            var persons = new List<PersonOld>();

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
                        persons.Add(new PersonOld(id, schedule, maxHours));
                    }
                    catch
                    {
                        throw new Exception("Unable to parse file: " + file);
                    }
                }
            }

            return persons;
        }

        public void SavePerson(PersonOld personOld)
        {
            string fileName = $"{personOld.Id}.{FolderConstants.FileExtensions}";
            var path = Path.Combine(FolderPath, fileName);

            using (var textWriter = GetTextWriter(path))
            {
                try
                {
                    textWriter.WriteLine(personOld.MaxHoursPerMonth);
                    ScheduleParser.Put(textWriter, personOld.Schedule);
                }
                catch
                {
                    throw new Exception("Unable to save personOld to the file: " + path);
                }
            }
        }
    }
}