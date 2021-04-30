using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bheithir.Emulators
{
    class Fusion : Presence
    {
        public Fusion()
        {
            DiscordAppId = "835587751239090187";
            ProcessName = "Fusion";
            WindowPattern = new Regex("(\\s-\\s)", RegexOptions.Compiled);
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
            catch (Exception) { return; }

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
                    details = "Idling on the Sega/Mega CD boot-up screen";
                else
                    details = titleParts[2];
            }
            catch(Exception) { return; }

            string status = "";
            try
            {
                if(titleParts.Count == 1)
                    status = titleParts[0].Replace("Fusion ", "v").Replace(" (C) Steve Snake, 2010.", "");
                else
                {
                    if(titleParts[1].Contains(' ') || titleParts[1] == "Genesis")
                        status = titleParts[1] + " | " + titleParts[0].Replace("Fusion ", "v");
                    else
                    {
                        switch(titleParts[1])
                        {
                            case "GameGear":
                                status = "Game Gear";
                                break;
                            case "MegaDrive":
                                status = "Mega Drive";
                                break;
                            case "MegaDrive/32X":
                                status = "Mega Drive / 32X";
                                break;
                            case "Genesis/32X":
                                status = "Genesis / 32X";
                                break;
                            case "MegaCD":
                                status = "Mega CD";
                                break;
                            case "SegaCD":
                                status = "Sega CD";
                                break;
                        }
                        status += " | " + titleParts[0].Replace("Fusion ", "v");
                    }
                }
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
