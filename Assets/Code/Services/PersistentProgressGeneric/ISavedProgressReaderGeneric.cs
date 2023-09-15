using Services.SaveLoad;

namespace Services.PersistentProgressGeneric
{
    public interface ISavedProgressReaderGeneric<TProgress> where TProgress : IProgressKey
    {
        void LoadProgress(in TProgress progress);

        void RegisterSave(ISaveLoadService saveLoadService) =>
            saveLoadService.Register(this);
    }
}