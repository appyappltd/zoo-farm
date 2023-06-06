namespace Services.Input
{
    public interface IPlayerInputService : IService
    {
        void Subscribe();
        void Cleanup();
    }
}