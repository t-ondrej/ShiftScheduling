namespace ShiftScheduleGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Generator generator = new Generator();
            generator.GenerateData(10, "TestData");          
        }
    }
}
