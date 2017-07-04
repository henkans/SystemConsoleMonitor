using System.Configuration;

namespace SystemConsoleMonitor.Configuration
{
    public class DefaultMachineElementCollection : ConfigurationElementCollection
    {
        public DefaultMachineElement this[int index]
        {
            get => BaseGet(index) as DefaultMachineElement;
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new DefaultMachineElement this[string responseString]
        {
            get => (DefaultMachineElement)BaseGet(responseString);
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
            return new DefaultMachineElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DefaultMachineElement)element).Name;
        }

        [ConfigurationProperty("compact", IsRequired = false)]
        public bool Compact => (bool)base["compact"];

    }
}
