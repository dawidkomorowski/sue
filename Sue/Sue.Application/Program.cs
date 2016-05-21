using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sue.Application
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            while (true)
            {
                string input = Console.ReadLine();
                File.WriteAllText("input" + DateTime.Now.Ticks + ".txt", input);
            }
        }
    }
}