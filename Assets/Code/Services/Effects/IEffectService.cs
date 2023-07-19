using Logic;

namespace Services.Effects
{
    public interface IEffectService : IService
    {
        void SpawnEffect(EffectId id, Location location);
    }
}