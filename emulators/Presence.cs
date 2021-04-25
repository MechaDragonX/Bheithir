using DiscordRPC;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Bheithir.Emulators
{
    abstract class Presence
    {
        protected DiscordRpcClient client;
        protected string windowTitle;
        protected Regex windowPattern;
        public Process Process { get; set; }

        public Presence() { }
        
        public abstract void Initialize();
        public abstract void Update();
        public abstract void Deinitialize();
        public abstract void OnUpdate();
        public abstract void SetNewPresence ();
    }
}
