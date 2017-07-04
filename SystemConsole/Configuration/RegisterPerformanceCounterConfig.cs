using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace SystemConsole.Configuration
{
    public class RegisterPerformanceCounterConfig : ConfigurationSection
    {
        public static RegisterPerformanceCounterConfig GetConfig()
        {
            try
            {
                return (RegisterPerformanceCounterConfig)ConfigurationManager.GetSection("RegisterPerformanceCounters") ?? new RegisterPerformanceCounterConfig();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        [System.Configuration.ConfigurationProperty("PerformanceCounters")]
        [ConfigurationCollection(typeof(PerformanceCounter), AddItemName = "PerformanceCounter")]
        public PerformanceCounters PerformanceCounters
        {
            get
            {
                object o = this["PerformanceCounters"];
                return o as PerformanceCounters;
            }
        }



    }
}
