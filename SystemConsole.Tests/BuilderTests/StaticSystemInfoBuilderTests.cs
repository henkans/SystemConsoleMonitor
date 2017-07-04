using System;
using SystemConsoleMonitor.Builder;
using SystemConsoleMonitor.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SystemConsole.Tests.BuilderTests
{
    [TestClass]
    public class StaticSystemInfoBuilderTests
    {
        [TestMethod]
        public void BuildStaticSystemInfoFromLocalMachine()
        {
            /* Arrange */
            var builder = new StaticSystemInfoBuilder();


            /* Act */
            var infoTask = builder.Build(string.Empty,false);
            var result = infoTask.Result;
            
            /* Assert */
            Assert.IsTrue(result.Count > 1);
            Assert.IsFalse(string.IsNullOrEmpty(result[0]));
            
        }
    }
}
