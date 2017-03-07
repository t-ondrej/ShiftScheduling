using System.IO;
using ShiftScheduleData.DataAccess.FileDao;

namespace ShiftScheduleGenerator
{
    internal class Generator
    {
        public GeneratorConfiguration Configuration { get; }

        private readonly PersonsGenerator _scheduleGenerator;

        private readonly RequirementsGenerator _requirementsGenerator;

        private readonly string _workingFolder;

        public Generator(GeneratorConfiguration configuration, string workingFolder)
        {
            _workingFolder = workingFolder;
            Configuration = configuration;
            _scheduleGenerator = new PersonsGenerator(Configuration);
            _requirementsGenerator = new RequirementsGenerator(Configuration);
        }

        public void GenerateData()
        {
            for (var i = 1; i <= Configuration.NumberOfSets; i++)
            {
                var folderName = $"{FolderConstants.SetFolderName}_{i}";
                var folderPath = Path.Combine(_workingFolder, folderName);
                Directory.CreateDirectory(folderPath);
                var filePersonDao = new FilePersonDao(folderPath);
                var fileRequirementsDao = new FileRequirementsDao(folderPath);

                var persons = _scheduleGenerator.GeneratePersons();
                var requirements = _requirementsGenerator.GenerateRequirements(persons);

                foreach (var person in persons)
                {
                    filePersonDao.SavePerson(person);
                }

                fileRequirementsDao.SaveRequirements(requirements);
                Directory.CreateDirectory(Path.Combine(folderPath, FolderConstants.OutputFolderName));
            }
        }
    }
}