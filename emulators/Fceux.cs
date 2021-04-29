using DiscordRPC;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bheithir.Emulators
{
    class Fceux : Presence
    {
        public Fceux()
        {
            windowPattern = new Regex("(:\\s)+", RegexOptions.Compiled);
        }

        public override void Initialize()
        {
            client = new DiscordRpcClient("693692813321437247");

            if(!Process.GetProcesses().Where(x => x.ProcessName.StartsWith("fceux")).Any())
            {
                Console.WriteLine("FCEUX was not found! Is it open?");
                return;
            }
            Process = Process.GetProcesses().Where(x => x.ProcessName.StartsWith("fceux")).ToList()[0];
            windowTitle = Process.MainWindowTitle;

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
        public override void Update()
        {
            client.OnPresenceUpdate += (sender, e) => { };
            client.Invoke();
            OnUpdate();
        }
        public override void Deinitialize()
        {
            client.ClearPresence();
            client.Dispose();
        }

        public override void OnUpdate()
        {
            Process process;
            try
            {
                process = Process.GetProcesses().Where(x => x.ProcessName.StartsWith("fceux")).ToList()[0];
            }
            catch(Exception) { return; }

            if(process.MainWindowTitle != windowTitle)
            {
                Process = process;
                windowTitle = Process.MainWindowTitle;
                SetNewPresence();
            }
        }
        public override void SetNewPresence()
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
