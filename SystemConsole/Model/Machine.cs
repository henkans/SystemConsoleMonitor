using System;

namespace SystemConsoleMonitor.Model
{
    public class Machine
    {
        public Machine(string name, ConsoleColor? color)
        {
            Name = name;
            Color = color;
        }
        public string Name { get; set; }
        public ConsoleColor? Color { get; set; }
    }
}
