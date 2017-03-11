using System.IO;
using ShiftScheduleDataAccess;
using ShiftScheduleDataAccess.FileDao;

namespace ShiftScheduleGenerator
{
    internal class Generator
    {
        public GeneratorConfiguration Configuration { get; }

        private readonly PersonsGenerator _scheduleGenerator;

        private readonly RequirementsGenerator _requirementsGenerator;

        private readonly DataAccessClient _dataAccessClient;

        public Generator(GeneratorConfiguration configuration, string workingFolder)
        {
            Configuration = configuration;
            _scheduleGenerator = new PersonsGenerator(Configuration);
            _requirementsGenerator = new RequirementsGenerator(Configuration);
            _dataAccessClient = new DataAccessClient(workingFolder);
        }

        public void GenerateData()
        {
            for (var i = 1; i <= Configuration.NumberOfSets; i++)
            {
                var folderName = $"{FolderConstants.SetFolderName}_{i}";
                var folderPath = Path.Combine(_dataAccessClient.WorkingFolder, folderName);
                Directory.CreateDirectory(folderPath);
                var filePersonDao = _dataAccessClient.GetPersonDao();
                var fileRequirementsDao = _dataAccessClient.GetRequirementsDao();

                var persons = _scheduleGenerator.GeneratePersons();
                var requirements = _requirementsGenerator.GenerateRequirements(persons);

                foreach (var person in persons)
                {
                    //filePersonDao.SavePerson(person);
                }

                //fileRequirementsDao.SaveRequirements(requirements);
                Directory.CreateDirectory(Path.Combine(folderPath, FolderConstants.OutputFolderName));
            }
        }
    }
}