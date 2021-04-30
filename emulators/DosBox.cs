using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Bheithir.Emulators
{
    class DosBox : Presence
    {
        public DosBox()
        {
            DiscordAppId = "693311130856325180";
            ProcessName = "DOSBox";
            WindowPattern = new Regex("(,\\s)+", RegexOptions.Compiled);
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
            for(int i = 0; i < titleParts.Count; i++)
            {
                if(titleParts[i] == ", ")
                {
                    titleParts.Remove(titleParts[i]);
                    i--;
                }
            }

            string details;
            try
            {
                if(Regex.Split(titleParts[3], "\\s+")[1] == "DOSBOX")
                    details = "Idling in the Command Line";
                else
                    details = Regex.Split(titleParts[3], "\\s+")[1];
            }
            catch(Exception) { return; }

            string status;
            try
            {
                status = new StringBuilder($"v{titleParts[0].Split(' ')[1]}").AppendFormat(", {0}, {1}", titleParts[1], titleParts[2]).ToString();
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
                        LargeImageKey = "dos",
                        LargeImageText = "DOSBox"
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
