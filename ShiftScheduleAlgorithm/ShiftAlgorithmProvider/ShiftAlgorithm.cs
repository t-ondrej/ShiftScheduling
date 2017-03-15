using System;
using System.Collections.Generic;
using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithmProvider
{
    internal static class ShiftAlgorithm
    {
        public sealed class Input
        {
            public IEnumerable<Person> Persons { get; }

            public Requirements MonthlyRequirementsOld { get; }

            public RequirementsFulfillingStats RequirementsFulfillingStats { get; }

            public AlgorithmConfiguration AlgorithmConfiguration { get; }

            public Input(IEnumerable<Person> persons, Requirements monthlyRequirementsOld, RequirementsFulfillingStats requirementsFulfillingStats, AlgorithmConfiguration algorithmConfiguration)
            {
                Persons = persons;
                MonthlyRequirementsOld = monthlyRequirementsOld;
                RequirementsFulfillingStats = requirementsFulfillingStats;
                AlgorithmConfiguration = algorithmConfiguration;
            }
        }

        public abstract class Algorithm
        {
            public Input AlgorithmInput { get; }

            protected Algorithm(Input algorithmInput)
            {
                AlgorithmInput = algorithmInput;
            }

            public abstract ResultingSchedule CreateScheduleForPeople();
        }

        public enum Strategy
        {
            Test
        }

        public static ResultingSchedule ExecuteAlgorithm(Input algorithmInput, Strategy strategy)
        {
            Algorithm algorithm;

            switch (strategy)
            {
                case Strategy.Test:
                    algorithm = new TestAlgorithm(algorithmInput);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return algorithm.CreateScheduleForPeople();
        }
    }
}