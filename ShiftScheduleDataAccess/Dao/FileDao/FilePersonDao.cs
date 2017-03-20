using System;
using System.Collections.Generic;
using System.IO;
using ShiftScheduleLibrary.Entities;
using ShiftScheduleLibrary.Utilities;

namespace ShiftScheduleDataAccess.Dao.FileDao
{
    internal class FilePersonDao : FileClient, IPersonDao
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
                        var dictionary = new Dictionary<int, Person.DailyAvailability>();

                        while ((line = textReader.ReadLine()) != null)
                        {
                            var splited = line.Split(' ');
                            var dayId = int.Parse(splited[0]);
                            var interval = Interval.FromString(splited[1]);
                            var leftTolerance = int.Parse(splited[2]);
                            var rightTolerance = int.Parse(splited[3]);
                            var shiftWeight = double.Parse(splited[4]);
                            var availability = new Person.DailyAvailability(interval, leftTolerance, rightTolerance, shiftWeight);
                            dictionary.Add(dayId, availability);
                        }

                        persons.Add(new Person(id, maxHours, dictionary));
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
            string fileName = $"{person.Id}{FolderConstants.Extension}";
            var path = Path.Combine(FolderPath, fileName);

            using (var textWriter = GetTextWriter(path))
            {
                try
                {
                    textWriter.WriteLine(person.MaxWork);

                    foreach (var personDailyAvailability in person.DailyAvailabilities)
                    {
                        var dayId = personDailyAvailability.Key;
                        var dailyAvailability = personDailyAvailability.Value;

                        textWriter.WriteLine
                        (
                            $"{dayId} " +
                            $"{dailyAvailability.Availability} " +
                            $"{dailyAvailability.LeftTolerance} " +
                            $"{dailyAvailability.RightTolerance} " +
                            $"{dailyAvailability.ShiftWeight}"
                        );
                    }
                }
                catch
                {
                    throw new Exception("Unable to save personOld to the file: " + path);
                }
            }
        }
    }
}