using System;

namespace SystemConsoleMonitor.Model
{
    public class Disk
    {
        public string Caption { get; set; }
        public string VolumeName { get; set; }
        public string Status { get; set; }

        public string DeviceID { get; set; }

        public UInt64 FreeSpace { get; set; }
        public UInt64 Size { get; set; }
        
    }
}
