using System.Configuration;

namespace SystemConsoleMonitor.Configuration
{
    public class SystemConsoleMonitorConfigurationSection : ConfigurationSection
    {
        public static SystemConsoleMonitorConfigurationSection GetConfig()
        {
            return (SystemConsoleMonitorConfigurationSection)ConfigurationManager.GetSection("SystemConsoleMonitor");
        }

        [ConfigurationProperty("PerformanceCounters")]
        [ConfigurationCollection(typeof(PerformanceCounterElement), AddItemName = "PerformanceCounter")]
        public PerformanceCounterElementCollection PerformanceCounters
        {
            get
            {
                object o = this["PerformanceCounters"];
                return o as PerformanceCounterElementCollection;
            }
        }

        [ConfigurationProperty("DefaultMachines")]
        [ConfigurationCollection(typeof(DefaultMachineElement), AddItemName = "DefaultMachine")]
        public DefaultMachineElementCollection DefaultMachines
        {
            get
            {
                object o = this["DefaultMachines"];
                return o as DefaultMachineElementCollection;
            }
        }

    }
}
