using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SystemConsoleMonitor.Enum;

namespace SystemConsoleMonitor.Utils
{
    public class ConsoleOutput
    {
        private static readonly object SyncLock = new object();
        private const int NameMaxLenght = 12;
        private const int MachineMonitorWidth = 80;
        internal static void Header()
        {
            lock (SyncLock)
            {
                var assembly = Assembly.GetExecutingAssembly();
                var titleAttribute = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false).FirstOrDefault() as AssemblyTitleAttribute;
                var version = assembly.GetName().Version.ToString();
                Console.Title = $"{titleAttribute?.Title}  {version}";
                ConsoleClear();
            }
        }
        
        internal static void WritePerformanceCounter(string machineName, string name, double counterValue, int row, string unit, PerformanceCounterStyle style, ConsoleColor? color = null)
        {
            lock (SyncLock)
            {
                SetCursor(0,row);
                // Redirected output format
                if (Console.IsOutputRedirected || Console.IsErrorRedirected) name = $"{machineName};{DateTime.Now};{name};";

                switch (style)
                {
                    case PerformanceCounterStyle.Text:
                    {
                        WriteMachineLine($"{name,NameMaxLenght * -1}{(int) Math.Round(counterValue)}{unit,-4} ", row);
                        break;
                    }
                    case PerformanceCounterStyle.TimeSpan:
                    {
                        WriteMachineLine($"{name,NameMaxLenght * -1}{TimeSpan.FromSeconds(counterValue):dd\\.hh\\:mm\\:ss}{unit,-4} ", row);
                        break;
                    }
                    case PerformanceCounterStyle.Bar:
                    {
                        PerformanceCounterBar(name, (int) Math.Round(counterValue), row, 100, 50.0f, color);
                        break;
                    }
                    case PerformanceCounterStyle.BarInverse:
                    {
                        var inversedValue = 100 - (int) Math.Round(counterValue);
                        PerformanceCounterBar(name, inversedValue, row, 100, 50.0f, color);
                        break;
                    }
                }
            }
        }

        internal static void WriteMachineLineFrame(int row, ConsoleColor? color)
        {
            if (Console.IsOutputRedirected || Console.IsErrorRedirected) return;
            lock (SyncLock)
            {
                var originalColor = Console.ForegroundColor;
                Console.SetCursorPosition(1, row);
                if (color != null) Console.ForegroundColor = (ConsoleColor)color;
                Console.Write($"|{" ",-(MachineMonitorWidth - 2)}|");
                Console.ForegroundColor = originalColor;
            }
        }

        internal static void Write(string s, int row)
        {
            lock (SyncLock)
            {
                if(!Console.IsOutputRedirected && !Console.IsErrorRedirected) Console.SetCursorPosition(0, row);
                Console.Out.Write(s);
            }
        }

        internal static void WriteOnlyConsole(string s, int row)
        {
            if (Console.IsOutputRedirected || Console.IsErrorRedirected) return;
            lock (SyncLock)
            {
                Console.SetCursorPosition(0, row);
                Console.Out.Write(s);
            }
        }

        internal static void WriteLine(string s, int row)
        {
            lock (SyncLock)
            {
                if (!Console.IsOutputRedirected && !Console.IsErrorRedirected) Console.SetCursorPosition(0, row);
                Console.Out.WriteLine(s);
            }
        }

        internal static void WritePerformanceCounterError(string name, string error, int row)
        {
            if (Console.IsOutputRedirected) return;
            lock (SyncLock)
            {
                var originalColor = Console.ForegroundColor;
                Console.SetCursorPosition(1, row);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"! {name,NameMaxLenght * -1}{error.Truncate(63)}");
                Console.ForegroundColor = originalColor;
            }
        }

        internal static int WriteMachineInfo(IList<string> staticInfoStrings, int row, ConsoleColor? color)
        {
            lock (SyncLock)
            {
                var first = true;
                foreach (var staticInfoString in staticInfoStrings)
                {
                    if (first)
                    {
                        WriteMachineHeader(staticInfoString, row++, color);
                        first = false;
                    }
                    else
                    {
                        WriteMachineLineFrame(row, color);
                        WriteMachineLine(staticInfoString, row++);
                    }
                }
                if(Console.IsOutputRedirected || Console.IsErrorRedirected) Console.Out.WriteLine(new String('-', 20));
            }
            return row;
        }

        private static void WriteMachineHeader(string machineName, int row, ConsoleColor? color)
        {
            if (Console.IsOutputRedirected)
            {
                Console.Out.WriteLine(machineName);
                return;
            }

            var originalColor = Console.ForegroundColor;
            var originalBackgroundColor = Console.BackgroundColor;

            if (color != null) Console.ForegroundColor = (ConsoleColor)color;
            Console.SetCursorPosition(1, row);
            Console.Write("|");
            Console.ForegroundColor = ConsoleColor.White;

            if (color != null) Console.BackgroundColor = (ConsoleColor) color;
            Console.Write($" {machineName,-(MachineMonitorWidth-3)}");

            Console.BackgroundColor = originalBackgroundColor;
            if (color != null) Console.ForegroundColor = (ConsoleColor)color;
            Console.Write("|");
            Console.ForegroundColor = originalColor;
        }

        private static void WriteMachineLine(string s, int row)
        {
            if (Console.IsOutputRedirected || Console.IsErrorRedirected)
            {
                Console.Out.WriteLine($"{s}");
            }
            else
            {
                SetCursor(3, row);
                Console.Write(s);
            }
        }

        private static void PerformanceCounterBar(string name, int progress, int row, int total, float barSize = 50.0f, ConsoleColor? color = null)
        {

            //Redirected output
            if (Console.IsOutputRedirected || Console.IsErrorRedirected)
            {
                WriteMachineLine($"{name}{(progress / (float)total * 100)}%", row);
                return;
            }
            
            float chunk = barSize / total;
            var originalForeground = Console.ForegroundColor;
            WriteMachineLine($"{name,NameMaxLenght * -1}", row);

            //Progress
            int position = NameMaxLenght + 3;
            for (int i = 0; i < chunk * progress; i++)
            {
                //Progress colors
                switch (i)
                {
                    case int n when (n < barSize / 3):
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;

                    case int n when (n < barSize / 4 * 3):
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                }

                Console.CursorLeft = position++;
                Console.Write("|");
            }
            Console.ForegroundColor = originalForeground;

            //Empty colors
            for (int i = position; i <= barSize + NameMaxLenght + 3; i++)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.CursorLeft = position++;
                Console.Write("|");
            }
            Console.ForegroundColor = originalForeground;

            // Print text value
            Console.CursorLeft = (int)barSize + NameMaxLenght + 5;
            Console.Write((progress / (float)total * 100) + "%  ");

        }

        private static void SetCursor(int left, int top)
        {
            if (!Console.IsOutputRedirected && !Console.IsErrorRedirected) Console.SetCursorPosition(left, top);
        }

        private static void ConsoleClear()
        {
            if (Console.IsOutputRedirected || Console.IsErrorRedirected) return;
            Console.CursorVisible = false;
            Console.Clear();
        }
    }
}
