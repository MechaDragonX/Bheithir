using DiscordRPC;
using DiscordRPC.Message;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Bheithir
{
    class Program
    {
        private static DiscordRpcClient client;
        private static Process dos;
        private static string windowTitle;
        private static Regex windowPattern = new Regex("(,\\s)+", RegexOptions.Compiled);

        private static void Main(string[] args)
        {
            Initialize();
            if (Process.GetProcesses().Where(x => x.ProcessName.StartsWith("DOSBox")).Count() == 0)
                return;

            while (true)
            {
                Update();
                if(Process.GetProcesses().Where(x => x.ProcessName.StartsWith("DOSBox")).Count() == 0)
                {
                    Deinitialize();
                    Console.WriteLine("Thanks for using Bheithir!");
                    return;
                }
            }
        }

        private static void Initialize()
        {
            client = new DiscordRpcClient("693311130856325180");

            if(Process.GetProcesses().Where(x => x.ProcessName.StartsWith("DOSBox")).Count() == 0)
            {
                Console.WriteLine("DOSBox was not found! Is it open?");
                return;
            }
            dos = Process.GetProcesses().Where(x => x.ProcessName.StartsWith("DOSBox")).ToList()[0];
            windowTitle = dos.MainWindowTitle;

            client.OnReady += (sender, e) =>
            {
                Console.WriteLine("Received Ready from user {0}", e.User.Username);
            };
            client.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine("Received Update! {0}", e.Presence);
            };

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

            try { setNewPresence(); }
            catch(Exception e)
            {
                Console.WriteLine($"Setting presence was not successful!\nERROR: {e.Message}");
                return;
            }
        }
        private static void Update()
        {
            client.OnPresenceUpdate += (sender, e) =>
            {
                Console.WriteLine("Received Update! {0}", e.Presence);
            };
            client.Invoke();
            OnUpdate();
        }
        private static void Deinitialize()
        {
            client.ClearPresence();
            client.Dispose();
        }

        private static void OnUpdate()
        {
            Process process;
            try
            {
                process = Process.GetProcesses().Where(x => x.ProcessName.StartsWith("DOSBox")).ToList()[0];
            }
            catch(IndexOutOfRangeException) { return;  }

            if(process.MainWindowTitle != windowTitle)
            {
                dos = process;
                windowTitle = dos.MainWindowTitle;
                setNewPresence();
            }
        }
        private static void setNewPresence()
        {
            List<string> titleParts = windowPattern.Split(windowTitle).ToList();
            for(int i = 0; i < titleParts.Count; i++)
            {
                if(titleParts[i] == ", ")
                {
                    titleParts.Remove(titleParts[i]);
                    i--;
                }
            }
            string status;
            try
            {
                status = new StringBuilder($"v{titleParts[0].Split(' ')[1]}").AppendFormat(", {0}, {1}", titleParts[1], titleParts[2]).ToString();
            }
            catch(IndexOutOfRangeException) { return; }

            client.SetPresence(new RichPresence
            {
                Details = Regex.Split(titleParts[3], "\\s+")[1],
                State = status,
                Timestamps = new Timestamps(DateTime.UtcNow)
            });
            Console.WriteLine("Presence successfully set!");
        }
    }
}
