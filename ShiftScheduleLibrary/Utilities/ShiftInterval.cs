using ShiftScheduleUtilities;

namespace ShiftScheduleLibrary.Utilities
{
    public class ShiftInterval : Interval
    {
        public enum IntervalType
        {
            Pause,
            Work
        }

        public IntervalType Type { get; }

        public ShiftInterval(int start, int end, IntervalType type) : base(start, end)
        {
            Type = type;
        }

        public override string ToString()
        {
            return $"{base.ToString()}={Type}";
        }

        public new static ShiftInterval FromString(string s)
        {
            var splited = s.Split('=');
            var interval = Interval.FromString(splited[0]);
            var type = EnumUtilities.ParseEnum<IntervalType>(splited[1]);
            return new ShiftInterval(interval.Start, interval.End, type);
        }
    }
}
