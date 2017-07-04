using System.Configuration;

namespace SystemConsoleMonitor.Configuration
{
    public class PerformanceCounterElementCollection : ConfigurationElementCollection
    {
        public PerformanceCounterElement this[int index]
        {
            get => BaseGet(index) as PerformanceCounterElement;
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new PerformanceCounterElement this[string responseString]
        {
            get => (PerformanceCounterElement)BaseGet(responseString);
            set
            {
                if (BaseGet(responseString) != null)
                {
                    BaseRemoveAt(BaseIndexOf(BaseGet(responseString)));
                }
                BaseAdd(value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new PerformanceCounterElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PerformanceCounterElement)element).Name;
        }
    }
}
