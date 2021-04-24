using DiscordRPC;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bheithir.Emulators
{
    class Snes9x : Presence, IPresence
    {
        public Snes9x()
        {
            windowPattern = new Regex("(\\s-\\s)(?!.*(\\s-\\s))", RegexOptions.Compiled);
        }

        public void Initialize()
        {
            client = new DiscordRpcClient("693881606309412874");

            if(Process.GetProcesses().Where(x => x.ProcessName.StartsWith("snes9x")).Count() == 0)
            {
                Console.WriteLine("Snes9x was not found! Is it open?");
                return;
            }
            emulator = Process.GetProcesses().Where(x => x.ProcessName.StartsWith("snes9x")).ToList()[0];
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
                process = Process.GetProcesses().Where(x => x.ProcessName.StartsWith("snes9x")).ToList()[0];
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
            string[] titleParts = windowPattern.Split(windowTitle);
            string details;
            try
            {
                if(titleParts.Length == 1)
                    details = "Idling with the Super Scope";
                else
                    details = titleParts[0];
            }
            catch(Exception) { return; }

            string status;
            try
            {
                if(titleParts.Length == 1)
                    status = titleParts[0].Replace("Snes9x ", "v");
                else
                    status = titleParts[2].Replace("Snes9x ", "v");
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
                        LargeImageKey = "snes",
                        LargeImageText = "Snes9x"
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
