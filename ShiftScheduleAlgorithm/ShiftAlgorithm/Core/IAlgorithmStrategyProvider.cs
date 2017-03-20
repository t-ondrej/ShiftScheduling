namespace ShiftScheduleAlgorithm.ShiftAlgorithm.Core
{
    internal interface IAlgorithmStrategyProvider
    {
        AlgorithmStrategy GetAlgorithm(string[] classNames);
    }
}
