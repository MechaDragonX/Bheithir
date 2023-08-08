using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Bheithir.Emulators
{
    class DosBox_X : Presence
    {
        public DosBox_X()
        {
            DiscordAppId = "1138396799669895248";
            ProcessName = "dosbox-x";
            WindowPattern = new Regex("(,\\s)+", RegexOptions.Compiled);
        }

        public override void Initialize()
        {
            Client = new DiscordRpcClient(DiscordAppId);

            Process = Process.GetProcesses().Where(x => x.ProcessName.StartsWith(ProcessName)).ToList()[0];
            WindowTitle = Process.MainWindowTitle;

            Client.OnReady += (sender, e) => { };
            Client.OnPresenceUpdate += (sender, e) => { };

            try
            {
                Client.Initialize();
                Console.WriteLine("Successfully connected to client!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Connection to client was not successful!\nERROR: {e.Message}");
                return;
            }

            try { SetNewPresence(); }
            catch (Exception e)
            {
                Console.WriteLine($"Setting presence was not successful!\nERROR: {e.Message}");
                return;
            }
        }
        public override void Update()
        {
            Client.OnPresenceUpdate += (sender, e) => { };
            Client.Invoke();
            OnUpdate();
        }
        public override void Deinitialize()
        {
            Client.ClearPresence();
            Client.Dispose();
        }

        public override void OnUpdate()
        {
            Process process;
            try
            {
                process = Process.GetProcesses().Where(x => x.ProcessName.StartsWith(ProcessName)).ToList()[0];
            }
            catch (Exception) { return; }

            if (process.MainWindowTitle != WindowTitle)
            {
                Process = process;
                WindowTitle = Process.MainWindowTitle;
                SetNewPresence();
            }
        }
        public override void SetNewPresence()
        {
            char[] delimiters = { ':', '[', ']' };
            string[] titleParts = WindowTitle.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

            if (titleParts.Length >= 2)
            {
                string part1 = titleParts[0].Trim();
                string[] part2Parts = titleParts[1].Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                string part2 = part2Parts.Length > 0 ? part2Parts[0].Trim() : "";
                string part3 = part2Parts.Length > 1 ? part2Parts[1].Trim() : "";

                string details;
                try
                {
                    if (titleParts[1].Contains("COMMAND"))
                        details = "Idling in the Command Line";
                    else
                        details = part2;
                }
                catch (Exception c)
                {
                    Console.WriteLine(c);
                    return;
                }


                string status;
                try
                {
                    status = part3;
                }
                catch (Exception c)
                {
                    Console.WriteLine(c);
                    return;
                }

                try
                {
                    Client.SetPresence(new RichPresence
                    {
                        Details = details,
                        State = status,
                        Timestamps = new Timestamps(DateTime.UtcNow),
                        Assets = new Assets()
                        {
                            LargeImageKey = "dos",
                            LargeImageText = "DOSBox-X"
                        }
                    });
                    Console.WriteLine("Presence successfully set!");
                }
                catch (Exception)
                {
                    Console.WriteLine("Presence was not set successfully!");
                    return;
                }

                Console.WriteLine("titleParts[0]: " + part1);
                Console.WriteLine("titleParts[1]: " + part2);
                Console.WriteLine("titleParts[2]: " + part3);
            }
        }
    }
}
