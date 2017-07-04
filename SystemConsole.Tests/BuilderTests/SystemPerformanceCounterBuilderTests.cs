using System;
using SystemConsoleMonitor.Builder;
using SystemConsoleMonitor.Enum;
using SystemConsoleMonitor.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemConsole.Tests.BuilderTests
{
    [TestClass]
    public class SystemPerformanceCounterBuilderTests
    {
        [TestMethod]
        public void BuildStaticSystemPerformanceCounter()
        {
            /* Arrange */
            var builder = new SystemPerformanceCounterBuilder();
            var pfce = new PerformanceCounterEntity();
            pfce.Name = "Threads:";
            pfce.Category = "System";
            pfce.Counter = "Threads";
            pfce.Instance = "";
            pfce.Unit = "";
            pfce.Style = PerformanceCounterStyle.Text;
            pfce.CompactView = false;
            
            /* Act */
            var systemPerformanceCounter = builder.Build(string.Empty, pfce);
            
            /* Assert */
            Assert.IsNotNull(systemPerformanceCounter);
        }
    }
}
