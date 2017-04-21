﻿using ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers;
using ShiftScheduleUtilities;
using System.Linq;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm.Implementations
{
    class AcuteTimeUnitChooser : ITimeUnitChooser
    {
        public TimeUnit FindTimeUnitToBeProccessed(TimeUnitsManager timeUnitsManager)
        {
            return timeUnitsManager.AllTimeUnits
                   .Where(timeUnit => !timeUnit.Fulfilled && timeUnit.Fulfillable)
                   .OrderBy(timeUnit => timeUnit.Acuteness)
                   .FirstOrDefault();
        }
    }
}
