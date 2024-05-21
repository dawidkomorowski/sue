using System;
using NLog;

namespace Sue
{
    internal class Program
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        static void Main()
        {
            Logger.Debug("Test debug message.");
            Logger.Info("Test info message.");
            Logger.Warn("Test warn message.");
            Logger.Error("Test error message.");
            Logger.Fatal("Test fatal message.");
            Logger.Error("Test error message.");
            Logger.Warn("Test warn message.");

            Console.WriteLine("Hello, World!");
        }
    }
}