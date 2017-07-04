using System;
using System.Collections.Generic;
using SystemConsoleMonitor.Builder;
using SystemConsoleMonitor.Enum;
using SystemConsoleMonitor.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemConsole.Tests.BuilderTests
{
    [TestClass]
    public class MachineMonitorBuilderTests
    {
        [TestMethod]
        public void BuildMachineMonitor()
        {
            /* Arrange */
            var builder = new MachineMonitorBuilder();
            var pfce = new PerformanceCounterEntity
            {
                Name = "Threads:",
                Category = "System",
                Counter = "Threads",
                Instance = "",
                Unit = "",
                Style = PerformanceCounterStyle.Text,
                CompactView = false
            };
            var performanceCounterEntities = new List<PerformanceCounterEntity> {pfce};

            /* Act */
            var machineMonitor = builder.Build(string.Empty, performanceCounterEntities, false, ConsoleColor.DarkGreen);

            /* Assert */
            Assert.IsNotNull(machineMonitor);
        }
    }
}
