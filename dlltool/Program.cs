using Bleak;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace loader
{
    class Program
    {
        //https://www.pinvoke.net/default.aspx/kernel32/SetConsoleTitle.html
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        static extern bool SetConsoleTitle(string lpConsoleTitle);

        static void Main(string[] args)
        {
            //Usage
            if (args.Length < 2)
            {
                SetConsoleTitle("dlltool 1.5 [x64 | x86]");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("usage: dlltool.exe <pid> <path to .dll file>");
                Console.ForegroundColor = ConsoleColor.White;
                System.Threading.Thread.Sleep(2500);
                System.Environment.Exit(1);
            }

            //Window Properties
            Console.SetWindowSize(99, 19);
            SetConsoleTitle("- Select a Injection Method -");

            //Define ProcessID, File Path & Process Name

            int ProcID = int.Parse(args[0]);
            string DLLPath = args[1];

            Process ByID = Process.GetProcessById(ProcID);
            string PrintProcess = ByID.ProcessName;

            //Print                       
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("[#] Process Name: " + PrintProcess);
            Console.WriteLine("\n[#] Process ID: " + ProcID);
            Console.WriteLine("\n[#] File Path: " + DLLPath);

            //Menu Selection
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("             ");
                Console.Write("\nMethods:");
                Console.WriteLine("             ");
                Console.Write("\n1.) Create Thread");
                Console.Write("\n2.) Hijack Thread");
                Console.Write("\n3.) Manual Map");
                Console.Write("\n4.) Abort");
                Console.Write("\n\n->  ");
                Console.ForegroundColor = ConsoleColor.White;
                string methodselection = Console.ReadLine();
                if (methodselection == "1")
                    try
                    {
                        using var CreateThread = new Injector(ProcID, DLLPath, InjectionMethod.CreateThread, InjectionFlags.None);
                        var dllBaseAddress = CreateThread.InjectDll();
                        Console.WriteLine("             ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[!] Succesfully injected .dll File. ");
                        Console.ForegroundColor = ConsoleColor.White;
                        System.Threading.Thread.Sleep(2000);
                        System.Environment.Exit(0);
                    }
                    catch (Exception)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("             ");
                        Console.WriteLine("[!] Failed to inject file into selected process");
                        Console.WriteLine("\n[!] Disable your Antivirus or choose another Method.");
                        Console.ForegroundColor = ConsoleColor.White;
                        System.Threading.Thread.Sleep(2000);
                        System.Environment.Exit(0);
                    }
                if (methodselection == "2")
                    try
                    {
                        using var HijackThread = new Injector(ProcID, DLLPath, InjectionMethod.HijackThread, InjectionFlags.None);
                        var dllBaseAddress = HijackThread.InjectDll();
                        Console.WriteLine("             ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[!] Succesfully injected .dll File. ");
                        Console.ForegroundColor = ConsoleColor.White;
                        System.Threading.Thread.Sleep(2000);
                        System.Environment.Exit(0);
                    }
                    catch (Exception)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("             ");
                        Console.WriteLine("[!] Failed to inject file into selected process ");
                        Console.WriteLine("\n[!] Disable your Antivirus or choose another Method.");
                        Console.ForegroundColor = ConsoleColor.White;
                        System.Threading.Thread.Sleep(2000);
                        System.Environment.Exit(0);
                    }
                if (methodselection == "3")
                    try
                    {
                        using var ManualMap = new Injector(ProcID, DLLPath, InjectionMethod.ManualMap, InjectionFlags.None);
                        var dllBaseAddress = ManualMap.InjectDll();
                        Console.WriteLine("             ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[!] Succesfully injected .dll File. ");
                        Console.ForegroundColor = ConsoleColor.White;
                        System.Threading.Thread.Sleep(2000);
                        System.Environment.Exit(0);
                    }
                    catch (Exception)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("             ");
                        Console.WriteLine("[!] Failed to inject file into selected process");
                        Console.WriteLine("\n[!] Disable your Antivirus or choose another Method.");
                        Console.ForegroundColor = ConsoleColor.White;
                        System.Threading.Thread.Sleep(2000);
                        System.Environment.Exit(0);
                    }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[!] Aborted by user");
                    Console.ForegroundColor = ConsoleColor.White;
                    System.Environment.Exit(0);
                }

            }
        }
    }
}

