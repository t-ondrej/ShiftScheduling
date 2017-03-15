using System.Collections.Generic;
using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.Core
{
    internal sealed class AlgorithmInput
    {
        public IEnumerable<Person> Persons { get; }

        public Requirements Requirements { get; }

        public RequirementsFulfillingStats RequirementsFulfillingStats { get; }

        public AlgorithmConfiguration AlgorithmConfiguration { get; }

        public AlgorithmInput(IEnumerable<Person> persons, Requirements requirements,
            RequirementsFulfillingStats requirementsFulfillingStats, AlgorithmConfiguration algorithmConfiguration)
        {
            Persons = persons;
            Requirements = requirements;
            RequirementsFulfillingStats = requirementsFulfillingStats;
            AlgorithmConfiguration = algorithmConfiguration;
        }
    }
}