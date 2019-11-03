using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.ConstrainedExecution;
using System.Security;
using Bleak;

namespace dlltool
{ 
    class Program
    {
        //https://www.pinvoke.net/default.aspx/kernel32/SetConsoleTitle.html
        [DllImport("kernel32.dll")]
        static extern bool SetConsoleTitle(string lpConsoleTitle);
        
        static void Main(string[] args)
        {
            //Define Usage
            if (args.Length < 2)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("usage: dlltool.exe <pid> <path to .dll file>");
                Console.ForegroundColor = ConsoleColor.White;
                System.Environment.Exit(1);
            }

            //Window Properties
            Console.SetWindowSize(98, 11);
            SetConsoleTitle("dll-tool - powered by Bleak");

            //Define ProcessID, File Path & Process Name
            int ProcID = int.Parse(args[0]);
            string DLLPath = args[1];
            Process ByID = Process.GetProcessById(ProcID);
            string ProcName = ByID.ProcessName;
   
            //Clear Console
            Console.Clear();

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("[#] Process Name: " + ProcName);
            Console.Write("\n[#] Process ID: " + ProcID);
            Console.Write("\n[#] File Path: " + DLLPath);

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("             ");
                Console.Write("\n[?]Do you want to inject the selected .dll file?.\n[?]Yes/No");
                Console.Write("               ");
                Console.ForegroundColor = ConsoleColor.White;
                string line = Console.ReadLine();
                if (line == "Yes")
                {
                    //Injection
                    using var injector = new Injector(ProcID, DLLPath, InjectionMethod.HijackThread, InjectionFlags.RandomiseDllHeaders);
                    var dllBaseAddress = injector.InjectDll();
                    
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("[!]Succesfully injected .dll File. ");
                    Console.ForegroundColor = ConsoleColor.White;
                    System.Environment.Exit(0);
                    break;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[!]Injection aborted by user.");
                    Console.ForegroundColor = ConsoleColor.White;
                    System.Environment.Exit(0);           
                }

            }  

        }
    }
}
