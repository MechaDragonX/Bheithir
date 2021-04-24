using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bheithir.Emulators
{
    class Fusion : Presence, IPresence
    {
        public Fusion()
        {
            windowPattern = new Regex("(\\s-\\s)", RegexOptions.Compiled);
        }

        public void Initialize()
        {
            client = new DiscordRpcClient("835587751239090187");

            if(Process.GetProcesses().Where(x => x.ProcessName.StartsWith("Fusion")).Count() == 0)
            {
                Console.WriteLine("Fusion was not found! Is it open?");
                return;
            }
            emulator = Process.GetProcesses().Where(x => x.ProcessName.StartsWith("Fusion")).ToList()[0];
            windowTitle = emulator.MainWindowTitle;

            client.OnReady += (sender, e) => { };
            client.OnPresenceUpdate += (sender, e) => { };

            try
            {
                client.Initialize();
                Console.WriteLine("Successfully connected to client!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Connection to client was not successful!\nERROR: {e.Message}");
                return;
            }

            try { SetNewPresence(); }
            catch(Exception e)
            {
                Console.WriteLine($"Setting presence was not successful!\nERROR: {e.Message}");
                return;
            }
        }
        public void Update()
        {
            client.OnPresenceUpdate += (sender, e) => { };
            client.Invoke();
            OnUpdate();
        }
        public void Deinitialize()
        {
            client.ClearPresence();
            client.Dispose();
        }

        public void OnUpdate()
        {
            Process process;
            try
            {
                process = Process.GetProcesses().Where(x => x.ProcessName.StartsWith("Fusion")).ToList()[0];
            }
            catch (Exception) { return; }

            if(process.MainWindowTitle != windowTitle)
            {
                emulator = process;
                windowTitle = emulator.MainWindowTitle;
                SetNewPresence();
            }
        }
        public void SetNewPresence()
        {
            List<string> titleParts = windowPattern.Split(windowTitle).ToList();
            titleParts.RemoveAll(x => x == " - ");
            string details;
            try
            {
                if(titleParts.Count == 1)
                    details = "Idling on the Sega/Mega CD boot-up screen";
                else
                    details = titleParts[2];
            }
            catch(Exception) { return; }

            string status;
            try
            {
                if(titleParts.Count == 1)
                    status = titleParts[0].Replace("Fusion ", "v").Replace(" (C) Steve Snake, 2010.", "");
                else
                    status = titleParts[1] + ", " + titleParts[0].Replace("Fusion ", "v");
            }
            catch(Exception) { return; }

            try
            {
                client.SetPresence(new RichPresence
                {
                    Details = details,
                    State = status,
                    Timestamps = new Timestamps(DateTime.UtcNow),
                    Assets = new Assets()
                    {
                        LargeImageKey = "md",
                        LargeImageText = "Fusion"
                    }
                });
                Console.WriteLine("Presence successfully set!");
            }
            catch (Exception)
            {
                Console.WriteLine("Presence was not set successfully!");
                return;
            }
        }
    }
}
