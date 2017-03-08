using System;
using System.Collections.Generic;
using ShiftScheduleData.Entities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithmProvider
{
    internal static class ShiftAlgorithm
    {
        public sealed class Input
        {
            public IEnumerable<Person> Persons { get; }

            public MonthlyRequirements MonthlyRequirements { get; }

            public AlgorithmConfiguration AlgorithmConfiguration { get; }

            public Input(IEnumerable<Person> persons, MonthlyRequirements monthlyRequirements, AlgorithmConfiguration algorithmConfiguration)
            {
                Persons = persons;
                MonthlyRequirements = monthlyRequirements;
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