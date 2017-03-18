using ShiftScheduleLibrary.Entities;
using System;
using System.Collections.Generic;
using static ShiftScheduleLibrary.Entities.RequirementsFulfillingStats;

namespace ShiftScheduleGenerator.Generation
{
    class FulfilingStatsGenerator
    {
        private static readonly Random Random = new Random();

        public GeneratorConfiguration Configuration { get; }

        public FulfilingStatsGenerator(GeneratorConfiguration configuration)
        {
            Configuration = configuration;
        }

        public RequirementsFulfillingStats GenerateFulfillingStats(List<Person> persons)
        {
            var fulfillingStats = new Dictionary<int, PersonStats>();

            foreach (var person in persons)
            {
                var periodToFulfilling = new Dictionary<int, double>();

                for (var period = 0; period < Configuration.PastPeriodCount; period++)             
                    periodToFulfilling.Add(period, Random.NextDouble());

                fulfillingStats.Add(person.Id, new PersonStats(periodToFulfilling));
            }

            return new RequirementsFulfillingStats(fulfillingStats);
        }

    }
}
