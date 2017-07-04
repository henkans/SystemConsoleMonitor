using SystemConsoleMonitor.Enum;

namespace SystemConsoleMonitor.Model
{
    public class PerformanceCounterEntity
    {
        public string Name { get; set;}
        public string Category { get; set; }
        public string Counter { get; set; }
        public string Instance { get; set; }
        public string Unit { get; set; }
        public PerformanceCounterStyle Style { get; set; }
        public bool CompactView { get; set; }
    }
}
