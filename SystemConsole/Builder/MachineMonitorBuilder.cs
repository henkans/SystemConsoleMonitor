using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SystemConsoleMonitor.Model;

namespace SystemConsoleMonitor.Builder
{
    public class MachineMonitorBuilder
    {
        public MachineMonitor Build(string machineName, IList<PerformanceCounterEntity> performanceCounterEntities, bool compactMode, ConsoleColor? color = ConsoleColor.DarkGreen)
        {
            try
            {
                //Build static system information.
                var staticSystemInfoBuilder = new StaticSystemInfoBuilder();
                var staticinfo = staticSystemInfoBuilder.Build(machineName, compactMode).Result;
               
                //Build System performance couters 
                if (compactMode)
                {
                    performanceCounterEntities = performanceCounterEntities.Where(e => e.CompactView).ToList();
                }

                var systemPerformanceCounters = new SystemPerformanceCounter[performanceCounterEntities.Count];
                Parallel.For(0, performanceCounterEntities.Count, i =>
                {
                    var perfCounter = new SystemPerformanceCounterBuilder().Build(staticinfo[0], performanceCounterEntities[i]);
                    systemPerformanceCounters[i] = perfCounter;
                });
                
                return new MachineMonitor(staticinfo[0], staticinfo, systemPerformanceCounters, color);
            }
            catch (AggregateException e)
            {
                var staticerrorinfo = new List<string> {machineName};
                foreach (var ex in e.Flatten().InnerExceptions)
                {
                    staticerrorinfo.Add($"Error: {ex.Message}");
                }
                return new MachineMonitor(machineName, staticerrorinfo, null, ConsoleColor.Red);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{machineName}: {e.GetType()} - {e.Message}");
                var staticerrorinfo = new List<string>
                {
                    machineName,
                    $"Error: {e.Message}"
                };
                return new MachineMonitor(machineName, staticerrorinfo, null, ConsoleColor.Red);
            }
        }
    }
}
