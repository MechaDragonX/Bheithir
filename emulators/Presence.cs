using DiscordRPC;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Bheithir.Emulators
{
    abstract class Presence
    {
        protected DiscordRpcClient Client;
        protected string DiscordAppId;
        protected string WindowTitle;
        protected Regex WindowPattern;
        protected Process Process;
        public string ProcessName { get; set; }

        public Presence() { }
        
        public abstract void Initialize();
        public abstract void Update();
        public abstract void Deinitialize();
        public abstract void OnUpdate();
        public abstract void SetNewPresence ();
    }
}
