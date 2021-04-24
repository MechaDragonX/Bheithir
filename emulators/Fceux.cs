using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Bheithir.Emulators
{
    class Fceux : Presence, IPresence
    {
        public Fceux()
        {
            windowPattern = new Regex("(:\\s)+", RegexOptions.Compiled);
        }

        public void Initialize()
        {
            client = new DiscordRpcClient("693692813321437247");

            if(Process.GetProcesses().Where(x => x.ProcessName.StartsWith("fceux")).Count() == 0)
            {
                Console.WriteLine("FCEUX was not found! Is it open?");
                return;
            }
            emulator = Process.GetProcesses().Where(x => x.ProcessName.StartsWith("fceux")).ToList()[0];
            windowTitle = emulator.MainWindowTitle;

            client.OnReady += (sender, e) => { };
            client.OnPresenceUpdate += (sender, e) => { };

            try
            {
                client.Initialize();
                Console.WriteLine("Successfully connected to client!");
            }
            catch(Exception e)
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
                process = Process.GetProcesses().Where(x => x.ProcessName.StartsWith("fceux")).ToList()[0];
            }
            catch(Exception) { return; }

            if(process.MainWindowTitle != windowTitle)
            {
                emulator = process;
                windowTitle = emulator.MainWindowTitle;
                SetNewPresence();
            }
        }
        public void SetNewPresence()
        {
            string[] titleParts = windowPattern.Split(windowTitle);
            string details;
            try
            {
                if (titleParts.Length == 1)
                    details = "Idling with the Power Glove";
                else
                    details = titleParts[2];
            }
            catch(Exception) { return; }

            string status;
            try 
            {
                status = titleParts[0].Replace("FCEUX ", "v");
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
                        LargeImageKey = "fceux",
                        LargeImageText = "FCEUX"
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
