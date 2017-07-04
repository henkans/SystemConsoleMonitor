using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace SystemConsoleMonitor.Model
{
    class SystemConsoleOptions
    {
        [OptionList('m', "machines", Separator = ';', HelpText = "Machines to monitor, separated by a semicolon.")]
        public List<string> Machines { get; set; }

        [OptionList('e', "exclusivemachines", Separator = ';', HelpText = "Machines to monitor without default machines in config, separated by a semicolon.")]
        public List<string> ExclusiveMachines { get; set; }

        [Option('l', "localmachine", DefaultValue = false, HelpText = "Monitor local machine.")]
        public bool LocalMachine { get; set; }

        [Option('c', "compact", DefaultValue = false, HelpText = "View monitors in compact mode.")]
        public bool CompactMode { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
