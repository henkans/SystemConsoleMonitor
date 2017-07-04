using System;
using System.Collections.Generic;

namespace SystemConsoleMonitor.Model
{
    public class StaticSystemInfo
    {
        public string MachineName { get; set; }
        public string OS { get; set; }
        public string Version { get; set; }
        public string Build { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string ProcessorName { get; set; }
        public int ProcessorCount { get; set; }
        public int CoreCount { get; set; }
        public UInt32 MaxClockSpeed { get; set; }

        public string Memory { get; set; }
        public ulong MemoryCapacity { get; set; }
        public string MemoryType { get; set; }

        public IList<Disk> Disks { get; set; }
    }
}
