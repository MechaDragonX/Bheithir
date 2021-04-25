using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bheithir.Emulators;

namespace Bheithir
{
    class Program
    {
        private static Dictionary<string, Presence> emulators = new Dictionary<string, Presence>()
        {
            { "dosbox", new DosBox() },
            { "fceux", new Fceux() },
            { "snes9x", new Snes9x() },
            { "fusion", new Fusion() },
            { "vbam", new Vbam() }
        };

        private static void Main(string[] args)
        {
            bool success = false;
            string emulator = "";
            while(!success)
            {
                Console.WriteLine("What emulator are you using?");
                Console.ForegroundColor = ConsoleColor.Cyan;
                emulator = Console.ReadLine().ToLower();
                if(emulators.ContainsKey(emulator))
                    break;
                Console.ResetColor();
                Console.WriteLine("You misspelled the emulator name or that emulator is not supported!\n");
            }
            Console.ResetColor();

            Presence presence = emulators[emulator];
            presence.Initialize();
            if(!Process.GetProcesses().Where(x => x.ProcessName == presence.Process.ProcessName).Any())
                return;
            while(true)
            {
                presence.Update();
                if(!Process.GetProcesses().Where(x => x.ProcessName == presence.Process.ProcessName).Any())
                {
                    presence.Deinitialize();
                    Console.WriteLine("Thanks for using Bheithir!");
                    return;
                }
            }
        }
    }
}
