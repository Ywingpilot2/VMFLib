using System;
using System.Collections.Generic;
using System.Diagnostics;
using VMFLib.Parsers;
using VMFLib.VClass;

namespace VMfTest
{
    internal class Program
    {
        internal List<BaseVClass> VClasses = new List<BaseVClass>();
        public static void Main(string[] args)
        {
            string currentInput = Console.ReadLine();
            while (currentInput != null)
            {
                switch (currentInput.Split(new[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim())
                {
                    case "read":
                    {
                        VClassReader classReader = new VClassReader(currentInput.Split(new[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[1].Trim('"'));
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        
                        BaseVClass currentClass = classReader.ReadClass();
                        while (currentClass != null)
                        {
                            Console.WriteLine($"Successfully read class {currentClass.ClassHeader}!");
                            currentClass = classReader.ReadClass();
                        }
                        
                        stopwatch.Stop();
                        Console.WriteLine($"Read {currentInput.Split(' ')[1]} in {stopwatch.ElapsedMilliseconds / 1000.0}s");
                    } break;
                }
                currentInput = Console.ReadLine();
            }
        }
    }
}