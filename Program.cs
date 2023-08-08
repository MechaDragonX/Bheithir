using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bheithir.Emulators;

namespace Bheithir
{
    class Program
    {
        private static readonly Dictionary<string, Presence> emulators = new Dictionary<string, Presence>()
        {
            { "dosbox", new DosBox() },
            { "dosbox-x", new DosBox_X() },
            { "fceux", new Fceux() },
            { "snes9x", new Snes9x() },
            { "fusion", new Fusion() },
            { "vbam", new Vbam() },
            { "mame", new Mame() }
        };

        private static void Main(string[] args)
        {
            bool success = false;
            string emulator = "";

            if (args.Length > 0)
            {
                emulator = args[0].ToLower();
                if (!emulators.ContainsKey(emulator))
                {
                    Console.WriteLine("The specified emulator is not supported!");
                    return;
                }
                else
                {
                    success = true;
                }
            }
            else
            {
                while (!success)
                {
                    Console.WriteLine("What emulator are you using?");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    emulator = Console.ReadLine().ToLower();
                    if (emulators.ContainsKey(emulator))
                        success = true;
                    else
                    {
                        Console.ResetColor();
                        Console.WriteLine("You misspelled the emulator name or that emulator is not supported!\n");
                    }
                }
                Console.ResetColor();
            }

            Presence presence = emulators[emulator];

            if (!Process.GetProcesses().Any(x => x.ProcessName.StartsWith(presence.ProcessName)))
            {
                Console.WriteLine("The specified emulator was not found! Is it open?");
                return;
            }

            presence.Initialize();
            while (true)
            {
                presence.Update();
                if (!Process.GetProcesses().Any(x => x.ProcessName.StartsWith(presence.ProcessName)))
                {
                    presence.Deinitialize();
                    Console.WriteLine("Thanks for using Bheithir!");
                    return;
                }
            }
        }
    }
}
