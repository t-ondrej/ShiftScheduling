namespace ShiftScheduleGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Generator generator = new Generator();
            generator.GenerateData(10, "TestData");
        }
    }
}