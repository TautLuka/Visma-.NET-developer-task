namespace VismaTask
{
    public class Person
    {
        public Person(string name, bool responsible, DateTime startTime, DateTime endTime)
        {
            Name = name;
            Responsible = responsible;
            StartTime = startTime;
            EndTime = endTime;
        }
        public string Name { get; set; }
        public bool Responsible { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }
}