using System;

namespace ShiftScheduleUtilities
{
    public static class EnumUtilities
    {
        public static T ParseEnum<T>(string value)
        {
            return (T) Enum.Parse(typeof(T), value, true);
        }
    }
}