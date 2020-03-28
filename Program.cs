using DiscordRPC;
using System;
using System.Diagnostics;
using System.Linq;

namespace Bheithir
{
    class Program
    {
        private static DiscordRpcClient client;

        static void Main(string[] args)
        {
            Initialize();
            // Update();
            while(true)
            {
                if(Process.GetProcesses().Where(x => x.ProcessName.StartsWith("DOSBox")).Count() == 0)
                {
                    Deinitialize();
                    Console.WriteLine("Thanks for using Bheithir!");
                    return;
                }
            }
        }

        static void Initialize()
        {
            client = new DiscordRpcClient("693311130856325180");

            if(Process.GetProcesses().Where(x => x.ProcessName.StartsWith("DOSBox")).Count() == 0)
            {
                Console.WriteLine("DOSBox was not found! Is it open?");
                return;
            }
            Process dos = Process.GetProcesses().Where(x => x.ProcessName.StartsWith("DOSBox")).ToList()[0];
            string currentWindowTitle = dos.MainWindowTitle;

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

            try
            {
                client.SetPresence(new RichPresence
                {
                    Details = currentWindowTitle,
                    State = "State Placeholder",
                    Timestamps = new Timestamps(DateTime.UtcNow)
                });
                Console.WriteLine("Presence successfully set!");
            }
            catch(Exception e)
            {
                Console.WriteLine($"Setting presence was not successful!\nERROR: {e.Message}");
                return;
            }
        }

        static void Update()
        {
            client.Invoke();
        }

        static void Deinitialize()
        {
            client.ClearPresence();
            client.Dispose();
        }
    }
}
