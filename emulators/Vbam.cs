﻿using DiscordRPC;
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
            DiscordAppId = "835659303808532601";
            ProcessName = "visualboyadvance-m";
            WindowPattern = new Regex("(\\s-\\s)", RegexOptions.Compiled);
        }

        public override void Initialize()
        {
            Client = new DiscordRpcClient(DiscordAppId);

            if(!Process.GetProcesses().Where(x => x.ProcessName.StartsWith(ProcessName)).Any())
            {
                Console.WriteLine("VBA-M was not found! Is it open?");
                return;
            }
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
            List<string> titleParts = WindowPattern.Split(WindowTitle).ToList();
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
                Client.SetPresence(new RichPresence
                {
                    Details = details,
                    State = status,
                    Timestamps = new Timestamps(DateTime.UtcNow),
                    Assets = new Assets()
                    {
                        LargeImageKey = "gb",
                        LargeImageText = "VBA-M"
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
