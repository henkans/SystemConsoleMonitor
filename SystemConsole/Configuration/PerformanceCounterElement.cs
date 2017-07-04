using System.Configuration;
using SystemConsoleMonitor.Enum;

namespace SystemConsoleMonitor.Configuration
{
    public class PerformanceCounterElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name => this["name"] as string;

        [ConfigurationProperty("category", IsRequired = true)]
        public string Category => this["category"] as string;

        [ConfigurationProperty("counter", IsRequired = true)]
        public string Counter => this["counter"] as string;

        [ConfigurationProperty("instance", IsRequired = true)]
        public string Instance => this["instance"] as string;

        [ConfigurationProperty("unit", IsRequired = false)]
        public string Unit => this["unit"] as string;

        [ConfigurationProperty("style", IsRequired = true)]
        public PerformanceCounterStyle Style => (PerformanceCounterStyle)this["style"];

        [ConfigurationProperty("compactview", IsRequired = false)]
        public bool CompactView => (bool)this["compactview"];
    }    
}

