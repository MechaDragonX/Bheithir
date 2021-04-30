using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bheithir.Emulators
{
    class Mame : Presence
    {
        public Mame()
        {
            DiscordAppId = "837778226137792543";
            ProcessName = "mame";
            WindowPattern = new Regex(" \\[([^;]*)\\]", RegexOptions.Compiled);
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
            catch(Exception) { return; }

            if(process.MainWindowTitle != WindowTitle)
            {
                Process = process;
                WindowTitle = Process.MainWindowTitle;
                SetNewPresence();
            }
        }
        public override void SetNewPresence()
        {
            string primaryTitle = "";
            if(WindowTitle != "")
            {
                primaryTitle = WindowTitle.Remove(0, 6);
                primaryTitle = WindowPattern.Split(primaryTitle)[0];
            }

            string details;
            try
            {
                if(primaryTitle == "No Driver Loaded" || primaryTitle == "")
                    details = "Idling with quarters";
                else
                    details = primaryTitle;
            }
            catch(Exception) { return; }

            try
            {
                Client.SetPresence(new RichPresence
                {
                    Details = details,
                    State = "",
                    Timestamps = new Timestamps(DateTime.UtcNow),
                    Assets = new Assets()
                    {
                        LargeImageKey = "mame",
                        LargeImageText = "MAME"
                    }
                });
                Console.WriteLine("Presence successfully set!");
            }
            catch(Exception)
            {
                Console.WriteLine("Presence was not set successfully!");
                return;
            }
        }
    }
}
