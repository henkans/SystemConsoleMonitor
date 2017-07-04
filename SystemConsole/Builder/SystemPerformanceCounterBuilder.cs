using System;
using System.Diagnostics;
using SystemConsoleMonitor.Model;

namespace SystemConsoleMonitor.Builder
{
    public class SystemPerformanceCounterBuilder
    {
        private SystemPerformanceCounter SystemPerformanceCounter { get; set; } 

        public SystemPerformanceCounter Build(string machineName, PerformanceCounterEntity performenceCounterSetting)
        {
            SystemPerformanceCounter = new SystemPerformanceCounter();
            SystemPerformanceCounter.PerformanceCounter = CreatePerformanceCounter(machineName, performenceCounterSetting);
            SystemPerformanceCounter.PerformanceCounterName = performenceCounterSetting.Name;
            SystemPerformanceCounter.PerformanceCounterUnit = performenceCounterSetting.Unit;
            SystemPerformanceCounter.PerformanceCounterStyle = performenceCounterSetting.Style;
            SystemPerformanceCounter.MachineName = machineName;
            return SystemPerformanceCounter;
        }
        
        private PerformanceCounter CreatePerformanceCounter(string machine, PerformanceCounterEntity settings)
        {
            PerformanceCounter perfCounter;
            try
            {
                perfCounter = string.IsNullOrEmpty(machine) ? 
                    new PerformanceCounter(settings.Category, settings.Counter, settings.Instance) : 
                    new PerformanceCounter(settings.Category, settings.Counter, settings.Instance, machine);
            }
            catch (Exception e)
            {
                SystemPerformanceCounter.ErrorMessage = e.Message;
                SystemPerformanceCounter.Error = true;
                perfCounter = null;
            }
            return perfCounter;
        }
    }
}
