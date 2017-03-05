using System;
using System.Collections.Generic;
using ShiftScheduleData.Entities;
using ShiftScheduleData.Helpers;

namespace ShiftScheduleAlgorithm.ShiftAlgorithmProvider
{
    internal class Validator
    {
        public ShiftAlgorithm.Input AlgorithmInput { get; }

        public ResultingSchedule Schedule { get; }

        private AlgorithmErrorReport AlgorithmErrorReport;

        public Validator(ShiftAlgorithm.Input algorithmInput, ResultingSchedule schedule)
        {
            AlgorithmInput = algorithmInput;
            Schedule = schedule;
            AlgorithmErrorReport = new AlgorithmErrorReport();
        }

        public AlgorithmErrorReport Validate()
        {
            CheckMaxDailyWorkPropertyNotMet();
            CheckWorkerPauseLengthPropertyNotMet();
            CheckMaxConsecutiveWorkHoursPropertyNotMet();
            CheckPersonScheduleRequirementsNotMet();
            CheckRequirementsAreNotMet();

            return AlgorithmErrorReport;
        }


        private void CheckMaxDailyWorkPropertyNotMet()
        {
            throw new NotImplementedException();
        }

        private void CheckWorkerPauseLengthPropertyNotMet()
        {
            throw new NotImplementedException();
        }

        private void CheckMaxConsecutiveWorkHoursPropertyNotMet()
        {
            throw new NotImplementedException();
        }

        private void CheckPersonScheduleRequirementsNotMet()
        {
            throw new NotImplementedException();
        }

        private void CheckRequirementsAreNotMet()
        {
            throw new NotImplementedException();
        }
    }
}