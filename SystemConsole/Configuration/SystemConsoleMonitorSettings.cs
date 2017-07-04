using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using SystemConsoleMonitor.Model;
using SystemConsoleMonitor.Utils;

namespace SystemConsoleMonitor.Configuration
{
    public class SystemConsoleMonitorSettings
    {
        public SystemConsoleMonitorSettings()
        {
            GetConfiguration();
        }

        public bool CompactMode { get; set; }
        public List<Machine> DefaultMachines { get; set; } = new List<Machine>();
        public IList<PerformanceCounterEntity> PerformanceCounterEntities { get; set; } = new List<PerformanceCounterEntity>();
        
        private void GetConfiguration()
        {
            try
            {
                var config = SystemConsoleMonitorConfigurationSection.GetConfig();

                CompactMode = config.DefaultMachines.Compact;

                foreach (var item in config.PerformanceCounters)
                {
                    PerformanceCounterEntities.Add(Map((PerformanceCounterElement) item));
                }
                foreach (var item in config.DefaultMachines)
                {
                    DefaultMachines.Add(new Machine(((DefaultMachineElement) item).Name.ToUpper(), ((DefaultMachineElement) item).Color));
                }
            }
            catch (ConfigurationErrorsException e)
            {
                if(!Console.IsOutputRedirected) Console.Clear();
                ConsoleOutput.Write($"Configuration error: {e.BareMessage}", 1);
                Debug.WriteLine($"Configuration error:\n {e}");
                Environment.Exit(-1);
            }
        }

        private static PerformanceCounterEntity Map(PerformanceCounterElement configElement)
        {
            var performanceCounterEntity = new PerformanceCounterEntity
            {
                Name = configElement.Name,
                Category = configElement.Category,
                Counter = configElement.Counter,
                Instance = configElement.Instance,
                Unit = configElement.Unit,
                Style = configElement.Style,
                CompactView = configElement.CompactView
            };
            return performanceCounterEntity;
        }

    }
}
