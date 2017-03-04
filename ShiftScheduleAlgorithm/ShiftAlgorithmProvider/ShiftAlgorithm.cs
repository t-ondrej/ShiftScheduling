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

            public Requirements Requirements { get; }

            public Properties Properties { get; }

            public Input(IEnumerable<Person> persons, Requirements requirements, Properties properties)
            {
                Persons = persons;
                Requirements = requirements;
                Properties = properties;
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