using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Bheithir.Emulators
{
    class Presence
    {
        protected DiscordRpcClient client;
        protected Process emulator;
        protected string windowTitle;
        protected Regex windowPattern;

        public Presence() { }
    }
}
