using System;
using Logic.Effects;
using Logic.Player;
using Logic.Storages;
using Services;
using Services.Effects;
using UnityEngine;

namespace Logic.Upgrades
{
    public class MaxInventoryWeightUpgrade : HeroUpgrade
    {
        private const string InterfaceImprovableCastException = "Hero inventory do not implement an interface IImprovable";

        private IEffectService _effectService;
        private Transform _cashedHeroTransform;

        protected override void OnAwake() =>
            Construct(AllServices.Container.Single<IEffectService>());

        protected override IImprovable GetImprovableFrom(Hero hero)
        {
            _cashedHeroTransform = hero.transform;
            return hero.Inventory as IImprovable ?? throw new InvalidCastException(InterfaceImprovableCastException);
        }

        private void Construct(IEffectService effectService) =>
            _effectService = effectService;

        protected override void OnImproved()
        {
            Location location = new Location(_cashedHeroTransform.position, Quaternion.LookRotation(Vector3.up));
            Effect effect = _effectService.SpawnEffect(EffectId.Upgrade, location);
            effect.transform.SetParent(_cashedHeroTransform, true);
        }
    }
}