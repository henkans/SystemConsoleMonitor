using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SystemConsoleMonitor.Enum;
using SystemConsoleMonitor.Model;
using SystemConsoleMonitor.Utils;

namespace SystemConsoleMonitor.Builder
{
    internal class StaticSystemInfoBuilder
    {
        private StaticSystemInfo StaticSystemInfo { get; } = new StaticSystemInfo();
        private ManagementScope Scope { get; set; }

        public async Task<List<string>> Build(string machineName, bool compactMode)
        {
            try
            {
                Scope = string.IsNullOrEmpty(machineName) ? new ManagementScope("root\\cimv2") : new ManagementScope("\\\\" + machineName + "\\root\\cimv2");
                Scope.Connect();

                var processorTask = ProcessorTask();
                var computerSystemTask = ComputerSystemTask();
                var operatingSystemTask = OperatingSystemTask();
                var logicalDiskmTask = LogicalDiskTask();
                var physicalMemoryTask = PhysicalMemoryTask();
                await Task.WhenAll(processorTask, computerSystemTask, operatingSystemTask, logicalDiskmTask, physicalMemoryTask);
            }
            catch (COMException e)
            {
                Debug.WriteLine($"{machineName}: {e.GetType()} - {e.Message}");
                throw;
            }
            catch (ManagementException e)
            {
                Debug.WriteLine($"{machineName}: {e.GetType()} - {e.Message}");
                throw;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"{machineName}: {e.GetType()} - {e.Message}");
                throw;
            }

            if (compactMode) return GetCompactStaticInfo();
            return GetStaticInfo();

        }

        private List<string> GetCompactStaticInfo()
        {
            var staticSystemIfoList = new List<string>
            {
                StaticSystemInfo.MachineName,
                $"{StaticSystemInfo.Model}, {StaticSystemInfo.OS}",
                $"Memory {StaticSystemInfo.MemoryCapacity.ToMegabyte()}, Speed {(StaticSystemInfo.MaxClockSpeed / 1000d):0.00}GHz, Cores {StaticSystemInfo.CoreCount}, Logical Processors {StaticSystemInfo.ProcessorCount}"
            };
            return staticSystemIfoList;
        }

        private List<string> GetStaticInfo()
        {
            var staticSystemIfoList = new List<string>
            {
                StaticSystemInfo.MachineName,
                $"{StaticSystemInfo.Manufacturer} {StaticSystemInfo.Model}",
                $"{StaticSystemInfo.OS} {StaticSystemInfo.Version}",
                $"{StaticSystemInfo.ProcessorName}, Cores: {StaticSystemInfo.CoreCount}, Logical Processors: {StaticSystemInfo.ProcessorCount}",
                $"{StaticSystemInfo.Memory} {StaticSystemInfo.MemoryType} Capacity: {StaticSystemInfo.MemoryCapacity.ToMegabyte()}"
            };

            // Add disks
            var diskStringBuilder = new StringBuilder();
            var index = 1;
            foreach (var disk in StaticSystemInfo.Disks)
            {
                var used = disk.Size - disk.FreeSpace;
                diskStringBuilder.Append($"{disk.DeviceID}{used.ToGigabyte()}/{disk.Size.ToGigabyte()} ");
                if (index % 5 == 0)
                {
                    staticSystemIfoList.Add($"Disk usage  {diskStringBuilder}");
                    diskStringBuilder = new StringBuilder();
                }
                index++;
            }
            if (diskStringBuilder.Length > 0)
            {
                staticSystemIfoList.Add($"Disk usage  {diskStringBuilder}");
            }

            return staticSystemIfoList;
        }

        private async Task ComputerSystemTask()
        {
            await Task.Run(() =>
            {
                // Computer system
                ObjectQuery query = new ObjectQuery("SELECT Name, Manufacturer, Model FROM Win32_ComputerSystem");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(Scope, query);
                ManagementObjectCollection queryCollection = searcher.Get();

                foreach (var o in queryCollection)
                {
                    var managementObject = (ManagementObject)o;
                    StaticSystemInfo.MachineName = (managementObject["Name"] ?? string.Empty).ToString();
                    StaticSystemInfo.Manufacturer = (managementObject["Manufacturer"] ?? string.Empty).ToString();
                    StaticSystemInfo.Model = (managementObject["Model"] ?? string.Empty).ToString();
                    break; // take only first for now
                }
            });
        }

        private async Task ProcessorTask()
        {
            await Task.Run(() =>
            {
                // Processor
                var query = new ObjectQuery("SELECT Name, NumberOfCores,NumberOfLogicalProcessors,MaxClockSpeed FROM Win32_Processor");
                var searcher = new ManagementObjectSearcher(Scope, query);
                var queryCollection = searcher.Get();

                foreach (var o in queryCollection)
                {
                    var managementObject = (ManagementObject)o;
                    StaticSystemInfo.ProcessorName = (managementObject["Name"] ?? string.Empty).ToString();
                    StaticSystemInfo.CoreCount += Convert.ToInt32(managementObject["NumberOfCores"]);
                    StaticSystemInfo.ProcessorCount += Convert.ToInt32(managementObject["NumberOfLogicalProcessors"]);
                    StaticSystemInfo.MaxClockSpeed = Convert.ToUInt32(managementObject["MaxClockSpeed"]);
                }
            });
        }

        private async Task OperatingSystemTask()
        {
            await Task.Run(() =>
            {
                var query = new ObjectQuery("SELECT Caption, Version, BuildNumber FROM Win32_OperatingSystem");
                var searcher = new ManagementObjectSearcher(Scope, query);
                var queryCollection = searcher.Get();

                foreach (var o in queryCollection)
                {
                    var managementObject = (ManagementObject)o;
                    StaticSystemInfo.OS = (managementObject["Caption"] ?? string.Empty).ToString();
                    StaticSystemInfo.Version = (managementObject["Version"] ?? string.Empty).ToString();
                    StaticSystemInfo.Build = (managementObject["BuildNumber"] ?? string.Empty).ToString();
                    break; // take only first for now
                }

            });
        }
        
        private async Task PhysicalMemoryTask()
        {
            await Task.Run(() =>
            {
                var query = new ObjectQuery("SELECT Caption, Capacity, MemoryType FROM Win32_PhysicalMemory");
                var searcher = new ManagementObjectSearcher(Scope, query);
                var queryCollection = searcher.Get();

                foreach (var o in queryCollection)
                {
                    var managementObject = (ManagementObject)o;
                    StaticSystemInfo.Memory = (managementObject["Caption"] ?? string.Empty).ToString();
                    StaticSystemInfo.MemoryCapacity += Convert.ToUInt64(managementObject["Capacity"]);
                    StaticSystemInfo.MemoryType = ((MemoryType)Convert.ToUInt16(managementObject["MemoryType"])).ToString();
                }
            });
        }

        private async Task LogicalDiskTask()
        {
            await Task.Run(() =>
            {
                var query = new ObjectQuery("SELECT Caption, VolumeName, DeviceID, Status, FreeSpace, Size FROM Win32_LogicalDisk WHERE DriveType = 3");
                var searcher = new ManagementObjectSearcher(Scope, query);
                var queryCollection = searcher.Get();

                StaticSystemInfo.Disks = new List<Disk>();
                foreach (var o in queryCollection)
                {
                    var managementObject = (ManagementObject)o;
                    //Local hard disks.
                    var disk = new Disk
                    {
                        Caption = (managementObject["Caption"] ?? string.Empty).ToString(),
                        VolumeName = (managementObject["VolumeName"] ?? string.Empty).ToString(),
                        DeviceID = (managementObject["DeviceID"] ?? string.Empty).ToString(),
                        Status = (managementObject["Status"] ?? string.Empty).ToString(),
                        FreeSpace = Convert.ToUInt64(managementObject["FreeSpace"]),
                        Size = Convert.ToUInt64(managementObject["Size"])
                    };
                    StaticSystemInfo.Disks.Add(disk);

                }
            });
        }
    }   
}
