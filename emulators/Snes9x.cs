using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Bheithir.Emulators
{
    class Snes9x : IPresence
    {
        private DiscordRpcClient client;
        private Process super;
        private string windowTitle;
        private Regex windowPattern = new Regex("(\\s-\\s)(?!.*(\\s-\\s))", RegexOptions.Compiled);

        public Snes9x() { }

        public void Initialize()
        {
            client = new DiscordRpcClient("693881606309412874");

            if (Process.GetProcesses().Where(x => x.ProcessName.StartsWith("snes9x")).Count() == 0)
            {
                Console.WriteLine("FCEUX was not found! Is it open?");
                return;
            }
            super = Process.GetProcesses().Where(x => x.ProcessName.StartsWith("snes9x")).ToList()[0];
            windowTitle = super.MainWindowTitle;

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
            catch (Exception e)
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
                super = process;
                windowTitle = super.MainWindowTitle;
                SetNewPresence();
            }
        }
        public void SetNewPresence()
        {
            string details;
            try
            {
                details = windowPattern.Split(windowTitle)[0];
            }
            catch(Exception) { return; }

            string status;
            try
            {
                status = windowPattern.Split(windowTitle)[2].Replace("Snes9x ", "v");
            }
            catch(Exception) { return; }

            client.SetPresence(new RichPresence
            {
                Details = details,
                State = status,
                Timestamps = new Timestamps(DateTime.UtcNow),
                Assets = new Assets()
                {
                    LargeImageKey = "snes",
                    LargeImageText = "Snes9x"
                }
            });
            Console.WriteLine("Presence successfully set!");
        }
    }
}
