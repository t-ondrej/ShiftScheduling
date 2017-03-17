using ShiftScheduleAlgorithm.ShiftAlgorithm.AlgorithmHelpers;
using ShiftScheduleAlgorithm.ShiftAlgorithm.Core;
using ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm.ScheduleChoosing;
using ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm.TimeUnitChoosing;
using ShiftScheduleLibrary.Entities;

namespace ShiftScheduleAlgorithm.ShiftAlgorithm.TimeUnitProccesingAlgorithm
{
    internal class TimeUnitAlgorithm : Algorithm
    {
        private TimeUnitsManager _timeUnitsManager;

        private readonly TimeUnitChooser _timeUnitChooser;

        private readonly ScheduleChooser _scheduleChooser;

        public TimeUnitAlgorithm(AlgorithmInput algorithmInput, TimeUnitChooser timeUnitChooser,
            ScheduleChooser scheduleChooser) : base(algorithmInput)
        {
            _timeUnitChooser = timeUnitChooser;
            _scheduleChooser = scheduleChooser;
        }

        public override ResultingSchedule CreateScheduleForPeople()
        {
            _timeUnitsManager = new TimeUnitsManager(AlgorithmInput);

            while (true)
            {
                // First we let the algoritm to find the time unit to be proccessed
                var timeUnit = _timeUnitChooser.FindTimeUnitToBeProccessed();

                // If there's no such unit, we can celebrate. We're done
                if (timeUnit == null)
                {
                    break;
                }

                var schedule = _scheduleChooser.FindScheduleToCoverUnit(timeUnit);

                if (schedule == null)
                {
                    // We can't fulfil the given time unit. But we can mark it as unsucessful and move on.
                    timeUnit.Fulfillable = false;
                    continue;
                }

                timeUnit.AssignSchedule(schedule);
                // TODO: Some logic about scheduling, calling some entity methods
            }

            return CreateResultingSchedule();
        }

        private static ResultingSchedule CreateResultingSchedule()
        {
            return null;
        }
    }
}