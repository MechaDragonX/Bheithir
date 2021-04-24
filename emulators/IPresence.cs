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
