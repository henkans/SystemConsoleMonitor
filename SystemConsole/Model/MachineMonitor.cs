using System;
using System.Collections.Generic;
using SystemConsoleMonitor.Utils;

namespace SystemConsoleMonitor.Model
{
    public class MachineMonitor
    {
        public string MachineName { get; set; }
        public ConsoleColor? Color { get; set; }
        public int MachineMonitorHeight { get; set; }
        public int MachineMonitorWidth { get; set; }
        public int StartRow { get; set; }
        private IList<string> StaticInfoStrings { get; }
        internal SystemPerformanceCounter[] SystemPerformanceCounters { get; set; }
        
        public MachineMonitor(string name, IList<string> staticSystemInfo, SystemPerformanceCounter[] systemPerformanceCounters, ConsoleColor? color)
        {
            MachineName = name;
            SystemPerformanceCounters = systemPerformanceCounters;
            StaticInfoStrings = staticSystemInfo;
            MachineMonitorHeight = SystemPerformanceCounters?.Length + StaticInfoStrings.Count ?? StaticInfoStrings.Count;
            Color = color ?? ConsoleColor.DarkGreen;
        }

        public void Run()
        {
            int row = StartRow;
            row = ConsoleOutput.WriteMachineInfo(StaticInfoStrings, row, Color);

            if (SystemPerformanceCounters != null)
            {
                foreach (var systemPerformanceCounter in SystemPerformanceCounters)
                {
                    systemPerformanceCounter.Start(row++, Color);
                }
            }
        }
    }
}
