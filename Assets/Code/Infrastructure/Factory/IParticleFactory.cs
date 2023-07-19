using Logic.Effects;
using Services.Effects;

namespace Infrastructure.Factory
{
    public interface IEffectFactory
    {
        Effect CreateParticle(EffectId id);
    }
}