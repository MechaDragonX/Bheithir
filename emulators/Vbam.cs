using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bheithir.Emulators
{
    class Vbam : Presence
    {
        public Vbam()
        {
            windowPattern = new Regex("(\\s-\\s)", RegexOptions.Compiled);
        }

        public override void Initialize()
        {
            client = new DiscordRpcClient("835659303808532601");

            if(Process.GetProcesses().Where(x => x.ProcessName.StartsWith("visualboyadvance-m")).Count() == 0)
            {
                Console.WriteLine("Vbam was not found! Is it open?");
                return;
            }
            Process = Process.GetProcesses().Where(x => x.ProcessName.StartsWith("visualboyadvance-m")).ToList()[0];
            windowTitle = Process.MainWindowTitle;

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
                process = Process.GetProcesses().Where(x => x.ProcessName.StartsWith("visualboyadvance-m")).ToList()[0];
            }
            catch (Exception) { return; }

            if(process.MainWindowTitle != windowTitle)
            {
                Process = process;
                windowTitle = Process.MainWindowTitle;
                SetNewPresence();
            }
        }
        public override void SetNewPresence()
        {
            List<string> titleParts = windowPattern.Split(windowTitle).ToList();
            titleParts.RemoveAll(x => x == " - ");
            string details;
            try
            {
                if(titleParts.Count == 1)
                    details = "Squinting at an un-backlit screen";
                else
                    details = titleParts[0];
            }
            catch(Exception) { return; }

            string status;
            try
            {
                if(titleParts.Count == 1)
                    status = titleParts[0].Replace("VisualBoyAdvance-M ", "v");
                else
                    status = titleParts[1].Replace("VisualBoyAdvance-M ", "v");
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
                        LargeImageKey = "gb",
                        LargeImageText = "Vbam"
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
