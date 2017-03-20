using System;
using System.Collections.Generic;

namespace ShiftScheduleUtilities
{
    public static class SingletonFactory
    {
        private static readonly IDictionary<Type, object> Instances = new Dictionary<Type, object>();

        public static object GetInstance(Type type)
        {
            if (Instances.ContainsKey(type))
                return Instances[type];

            var newInstance = Activator.CreateInstance(type);
            Instances.Add(type, newInstance);
            return newInstance;
        }
    }
}