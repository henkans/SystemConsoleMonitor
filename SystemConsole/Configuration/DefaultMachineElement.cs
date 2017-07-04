using System;
using System.Configuration;

namespace SystemConsoleMonitor.Configuration
{
    public class DefaultMachineElement: ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name => this["name"] as string;

        [ConfigurationProperty("color", IsRequired = false)]
        public ConsoleColor? Color => this["color"] is ConsoleColor ? (ConsoleColor) this["color"] : ConsoleColor.DarkGreen;
    }
}
