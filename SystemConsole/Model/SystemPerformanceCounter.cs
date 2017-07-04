using System;
using System.Diagnostics;
using System.Threading.Tasks;
using SystemConsoleMonitor.Enum;
using SystemConsoleMonitor.Utils;

namespace SystemConsoleMonitor.Model
{
    public class SystemPerformanceCounter
    {
        public bool Error { get; set; }
        public string ErrorMessage { get; set; }
        public string MachineName { get; set; }
        public string PerformanceCounterName { get; set; }
        public string PerformanceCounterUnit { get; set; }
        public PerformanceCounter PerformanceCounter { get; set; }
        public PerformanceCounterStyle PerformanceCounterStyle { get; set; }
        
        public async void Start(int row, ConsoleColor? color)
        {
            if (Error)
            {
                ConsoleOutput.WritePerformanceCounterError(PerformanceCounterName, ErrorMessage, row);
                return;
            }

            if (PerformanceCounter == null) return;


            ConsoleOutput.WriteMachineLineFrame(row, color);

            await Task.Run(async () =>
            {
                while (true)
                {
                    ConsoleOutput.WritePerformanceCounter(MachineName, PerformanceCounterName, PerformanceCounter.NextValue(), row, PerformanceCounterUnit, PerformanceCounterStyle, color);
                    if(!Console.IsOutputRedirected)Console.CursorVisible = false; //NOTE Hack to hide cursor when console window is resized be user.
                    await Task.Delay(1500);
                }
            });
        }
    }
}
