using System;
using System.Collections.Generic;
using System.Text;

namespace Bheithir
{
    interface IPresence
    {
        void Execute();

        void Initialize();
        void Update();
        void Deinitialize();

        void OnUpdate();
        void SetNewPresence();
    }
}
