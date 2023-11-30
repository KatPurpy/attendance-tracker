namespace AttendanceTracker
{
    public interface IGuidDbKey
    {
        public Guid Guid { get; set; }
    }

    public interface IStringNameDbKey
    {
        public string Name { get; set; }
    }
}
