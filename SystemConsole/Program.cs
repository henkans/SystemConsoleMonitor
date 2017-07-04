using System;
using System.Globalization;
using System.Threading;
using SystemConsoleMonitor.Handler;
using SystemConsoleMonitor.Model;
using CommandLine;

namespace SystemConsoleMonitor
{
    class Program
    {
        protected static SystemConsoleOptions Options = new SystemConsoleOptions();
        public static int EndRow;

        static void Main(string[] args)
        {
            // Always use en-US culture
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            //TODO fix resizing problem in a good way!
            var width = 82;
            if (Console.WindowWidth < width)
            {
                Console.SetWindowSize(width, Console.WindowHeight);
                Console.SetBufferSize(width, Console.BufferHeight);
            }

            // Wire up cancel
            var exitEvent = new ManualResetEvent(false);
            Console.CancelKeyPress += (sender, eventArgs) => {
                // Close gracefully
                if (!Console.IsOutputRedirected && !Console.IsErrorRedirected)
                {
                    Console.CursorVisible = true;
                    Console.CursorTop = EndRow;
                }
                eventArgs.Cancel = true;
                exitEvent.Set();
            };
            
            // Parse Args            
            var parser = new Parser(s => { s.CaseSensitive = false; s.HelpWriter = Console.Error; });

            if (parser.ParseArgumentsStrict(args, Options))
            {
                //Build and start all machine monitors
                var machineMonitorHandler = new MachineMonitorHandler(Options.Machines, Options.ExclusiveMachines, Options.LocalMachine, Options.CompactMode);
                machineMonitorHandler.MonitorAll();
            }

            exitEvent.WaitOne();
        }
    }
}
