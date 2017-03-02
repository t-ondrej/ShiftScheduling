using System;
using System.Collections.Generic;
using ShiftScheduleData.Entities;

namespace ShiftShedule.ShiftAlgorithm
{
    internal static class ShiftAlgorithm
    {
        public sealed class Input
        {
            public ICollection<Person> Persons { get; }

            public Requirements Requirements { get; }

            public Properties Properties { get; }

            public Input(ICollection<Person> persons, Requirements requirements, Properties properties)
            {
                Persons = persons;
                Requirements = requirements;
                Properties = properties;
            }
        }

        public interface IAlgorithm
        {
            ResultingSchedule CreateScheduleForPeople(Input algorithmInput);
        }

        public enum Strategy
        {
            Test
        }
        
        public static IAlgorithm GetAlgorithmForStrategy(Strategy strategy)
        {
            switch (strategy)
            {
                case Strategy.Test:
                    return new TestAlgorithm();
                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }
        }
    }
}