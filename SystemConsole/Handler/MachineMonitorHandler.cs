using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using SystemConsoleMonitor.Builder;
using SystemConsoleMonitor.Configuration;
using SystemConsoleMonitor.Model;
using SystemConsoleMonitor.Utils;

namespace SystemConsoleMonitor.Handler
{
    public class MachineMonitorHandler
    {
        private int startRow = 1;
        public MachineMonitor[] MachineMonitors { get; set; } 
        private SystemConsoleMonitorSettings ConfigSettings { get; } = new SystemConsoleMonitorSettings();
        private bool ViewLocal { get; }
        private bool CompactMode { get; }
        
        public MachineMonitorHandler(List<string> machineNames, List<string> exclusiveMachines, bool viewLocal, bool compactMode)
        {
            ViewLocal = viewLocal;
            CompactMode = ConfigSettings.CompactMode || compactMode;
            CreateMachineMonitors(machineNames??new List<string>(), exclusiveMachines);
        }

        private void CreateMachineMonitors(List<string> machineNames, List<string> exclusiveMachines)
        {
            var stopWatch = new Stopwatch();

            List<Machine> allMachines;
            if (exclusiveMachines != null)
            {
                // add only parameter machines
                allMachines = exclusiveMachines.Select(name => new Machine(name.ToUpper(), null)).ToList();
            }
            else
            {
                // add parameter machines and default machines
                var paramMachinesNotToAdd = ConfigSettings.DefaultMachines.Select(x => x.Name).ToArray();
                allMachines = machineNames.Select(name => new Machine(name.ToUpper(), null)).Where(m => !paramMachinesNotToAdd.Contains(m.Name)).ToList();
                allMachines.AddRange(ConfigSettings.DefaultMachines);
            }
            
            //add local machines
            if (!allMachines.Any() || ViewLocal)
            {
                allMachines.Add(new Machine("", null));
            }
            
            // Create machine monitors
            ConsoleOutput.Header();
            ConsoleOutput.WriteOnlyConsole(" Creating machine monitors", 1);
            MachineMonitors = new MachineMonitor[allMachines.Count]; 

            Parallel.For(0, allMachines.Count, i =>
            {
                stopWatch.Reset();
                stopWatch.Start();

                var machineName = string.IsNullOrEmpty(allMachines[i].Name) ? "Local machine" : allMachines[i].Name;
                ConsoleOutput.WriteOnlyConsole($" - {machineName}...", i+2);

                var machineMonitor = new MachineMonitorBuilder().Build(allMachines[i].Name, ConfigSettings.PerformanceCounterEntities, CompactMode, allMachines[i].Color);
                MachineMonitors[i] = machineMonitor;

                ConsoleOutput.WriteOnlyConsole($" - {machineName}...Done.", i + 2);
                Debug.WriteLine($"Create, add and start {machineName}: {stopWatch.Elapsed}");
            });

            // Set startrows on monitors.
            foreach (var monitor in MachineMonitors)
            {
                monitor.StartRow = startRow;
                startRow += monitor.MachineMonitorHeight + 1;
            }
            Program.EndRow = startRow;

        }

        public void MonitorAll()
        {
            // Start all machineMonitors
            ConsoleOutput.Header();
            Parallel.ForEach(MachineMonitors, machineMonitor =>
            {
                machineMonitor.Run();
            });
        }
    }
}
