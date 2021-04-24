using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Bheithir.Emulators
{
    class Fceux : IPresence
    {
        private DiscordRpcClient client;
        private Process famicom;
        private string windowTitle;
        private readonly Regex windowPattern = new Regex("(:\\s)+", RegexOptions.Compiled);

        public Fceux() { }

        public void Initialize()
        {
            client = new DiscordRpcClient("693692813321437247");

            if(Process.GetProcesses().Where(x => x.ProcessName.StartsWith("fceux")).Count() == 0)
            {
                Console.WriteLine("FCEUX was not found! Is it open?");
                return;
            }
            famicom = Process.GetProcesses().Where(x => x.ProcessName.StartsWith("fceux")).ToList()[0];
            windowTitle = famicom.MainWindowTitle;

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
                process = Process.GetProcesses().Where(x => x.ProcessName.StartsWith("fceux")).ToList()[0];
            }
            catch (Exception) { return; }

            if (process.MainWindowTitle != windowTitle)
            {
                famicom = process;
                windowTitle = famicom.MainWindowTitle;
                SetNewPresence();
            }
        }
        public void SetNewPresence()
        {
            string[] titleParts = windowPattern.Split(windowTitle);
            string details;
            try
            {
                details = titleParts[2];
            }
            catch(Exception) { return; }

            string status;
            try 
            {
                status = titleParts[0].Replace("FCEUX ", "v");
            }
            catch(Exception) { return; }

            client.SetPresence(new RichPresence
            {
                Details = details,
                State = status,
                Timestamps = new Timestamps(DateTime.UtcNow),
                Assets = new Assets()
                {
                    LargeImageKey = "fc",
                    LargeImageText = "FCEUX"
                }
            });
            Console.WriteLine("Presence successfully set!");
        }
    }
}
