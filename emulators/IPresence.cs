using System;
using System.Collections.Generic;
using System.Text;

namespace Bheithir.Emulators
{
    interface IPresence
    {
        void Initialize();
        void Update();
        void Deinitialize();

        void OnUpdate();
        void SetNewPresence();
    }
}
