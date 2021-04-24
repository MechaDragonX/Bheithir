using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Bheithir.Emulators;

namespace Bheithir
{
    class Program
    {
        private static Dictionary<string, IPresence> emulators = new Dictionary<string, IPresence>()
        {
            { "dosbox", new DosBox() },
            { "fceux", new Fceux() },
            { "snes9x", new Snes9x() },
            { "fusion", new Fusion() }
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

            IPresence presence = emulators[emulator];
            presence.Initialize();
            if(Process.GetProcesses().Where(x => x.ProcessName.ToLower().StartsWith(emulator)).Count() == 0)
                return;
            while(true)
            {
                presence.Update();
                if(Process.GetProcesses().Where(x => x.ProcessName.ToLower().StartsWith(emulator)).Count() == 0)
                {
                    presence.Deinitialize();
                    Console.WriteLine("Thanks for using Bheithir!");
                    return;
                }
            }
        }
    }
}
