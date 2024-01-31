using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VMFLib.Parsers;
using VMFLib.VClass;

namespace VMfTest
{
    internal class Program
    {
        public static List<BaseVClass> VClasses = new List<BaseVClass>();
        public static BaseVClass SelectedClass;
        public static void Main(string[] args)
        {
            string currentInput = Console.ReadLine();
            while (currentInput != null)
            {
                string command = currentInput.Split(new[] { "\"" }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                string param = currentInput.Split(new[] { "\"" }, StringSplitOptions.RemoveEmptyEntries).Last().Trim('"');
                switch (command)
                {
                    case "read":
                    {
                        VClassReader classReader = new VClassReader(param);
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        
                        BaseVClass currentClass = classReader.ReadClass();
                        while (currentClass != null)
                        {
                            Console.WriteLine($"Successfully read class {currentClass}!");
                            currentClass = classReader.ReadClass();
                        }
                        
                        stopwatch.Stop();
                        classReader.Dispose();
                        Console.WriteLine($"Read {param} in {stopwatch.ElapsedMilliseconds / 1000.0}s");
                    } break;
                    case "readadd":
                    {
                        VClassReader classReader = new VClassReader(param);
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();
                        
                        BaseVClass currentClass = classReader.ReadClass();
                        while (currentClass != null)
                        {
                            Console.WriteLine($"Successfully read class {currentClass}!");
                            VClasses.Add(currentClass);
                            currentClass = classReader.ReadClass();
                        }
                        
                        stopwatch.Stop();
                        classReader.Dispose();
                        Console.WriteLine($"Read {param} in {stopwatch.ElapsedMilliseconds / 1000.0}s");
                    } break;
                    case "clear":
                    {
                        VClasses.Clear();
                        SelectedClass = null;
                    } break;
                    case "printall":
                    {
                        for (var idx = 0; idx < VClasses.Count; idx++)
                        {
                            BaseVClass vClass = VClasses[idx];
                            Console.WriteLine($"{idx}. {vClass}");
                        }
                    } break;
                    case "select":
                    {
                        int idx = 0;
                        if (int.TryParse(param, out idx) && idx < VClasses.Count)
                        {
                            SelectedClass = VClasses[idx];
                            Console.WriteLine($"Selected Class {SelectedClass} at index {idx}!");
                        }
                    } break;
                    case "printprop":
                    {
                        if (SelectedClass == null)
                        {
                            Console.WriteLine("Selected class was null! Use 'select' to select a class, use 'printall' to see all classes");
                            break;
                        }

                        foreach (VProperty key in SelectedClass.Properties.Values)
                        {
                            Console.WriteLine($"{SelectedClass.ToString()}: {key}, {key.Str()}");
                        }
                    } break;
                    case "edit":
                    {
                        if (SelectedClass == null)
                        {
                            Console.WriteLine("Selected class was null! Use 'select' to select a class, use 'printall' to see all classes");
                            break;
                        }

                        var cParams = param.Split('.');
                        switch (cParams[0])
                        {
                            case "AddProperty": //edit "AddProperty.{name}.{value}"
                            {
                                VProperty newProperty = new VProperty(cParams[1], cParams[2]);
                                SelectedClass.AddProperty(newProperty);
                            } break;
                            case "EditProperty": //edit "EditProperty.{name}.{value}"
                            {
                                if (!SelectedClass.Properties.ContainsKey(cParams[1]))
                                {
                                    Console.WriteLine($"Property {cParams[1]} does not exist!");
                                }
                                VProperty existingProperty = SelectedClass.Properties[cParams[1]];

                                if (existingProperty.Set(cParams[2]))
                                {
                                    Console.WriteLine($"Successfully edited {cParams[1]}!");
                                }
                                else
                                {
                                    Console.WriteLine($"Could not edit property {cParams[1]}");
                                }
                            } break;
                        }
                    } break;
                    case "save":
                    {
                        //TODO: Save modified data
                    } break;
                }
                currentInput = Console.ReadLine();
            }
        }
    }
}