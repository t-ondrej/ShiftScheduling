using System;

namespace ShiftScheduleGenerator.Generation
{
    internal struct Difficulty
    {
        public enum Possilibity
        {
            Possible,
            Impossible,
            Maybe
        }

        public const double ImpossibleValue = 1;

        public const double MaybeVaue = -1;

        public double Value { get; }

        public Possilibity DifficutyPossilibity { get; }

        public Difficulty(string stringValue)
        {

            if (!double.TryParse(stringValue, out double value))
                throw new ArgumentException("Unparsable difficulty value");

            Value = value;

            if (value == ImpossibleValue)
                DifficutyPossilibity = Possilibity.Impossible;
            else if (value == MaybeVaue)
                DifficutyPossilibity = Possilibity.Maybe;
            else if (value > 0 || value < 1)
                DifficutyPossilibity = Possilibity.Possible;
            else
                throw new ArgumentException("Unparsable difficulty value");
        }
    }
}
