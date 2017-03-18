using System.IO;
using ShiftScheduleDataAccess.Dao;
using ShiftScheduleUtilities;

namespace ShiftScheduleGenerator.Generation
{
    internal class Generator
    {
        public GeneratorConfiguration Configuration { get; }

        private readonly PersonsGenerator _scheduleGenerator;

        private readonly RequirementsGenerator _requirementsGenerator;

        private readonly FulfilingStatsGenerator _fulfilingStatsGenerator;

        private readonly string _workingFolder;

        private readonly string _dataSetFolderName;

        public Generator(GeneratorConfiguration configuration, string workingFolder, string dataSetFolderName)
        {
            _workingFolder = workingFolder;
            _dataSetFolderName = dataSetFolderName;
            Configuration = configuration;
            _scheduleGenerator = new PersonsGenerator(Configuration);
            _requirementsGenerator = new RequirementsGenerator(Configuration);
            _fulfilingStatsGenerator = new FulfilingStatsGenerator(Configuration);
        }

        public void GenerateData()
        {
            var lettersNumber = Configuration.NumberOfSets.ToString().Length;

            for (var i = 1; i <= Configuration.NumberOfSets; i++)
            {
                // Initialize folder with the test data
                var iAsString = i.ToString();
                var zeroesToMatchLength = new string('0', lettersNumber - iAsString.Length);
                var folderName = $"{_dataSetFolderName}_{zeroesToMatchLength}{iAsString}";
                var folderPath = Path.Combine(_workingFolder, folderName);
                var dataAccessClient = new DataAccessClient(folderPath);
                dataAccessClient.InitializeWorkingFolder();

                // Generate the data
                var persons = _scheduleGenerator.GeneratePersons();
                var requirements = _requirementsGenerator.GenerateRequirements(persons);
                var fulfillingStats = _fulfilingStatsGenerator.GenerateFulfillingStats(persons);

                // Save the data
                LinqExtensions.ForEach(persons, person => dataAccessClient.PersonDao.SavePerson(person));
                dataAccessClient.RequirementsDao.SaveRequirements(requirements);
                dataAccessClient.RequirementsFulfillingStatsDao.SaveRequirements(fulfillingStats);
            }
        }
    }
}