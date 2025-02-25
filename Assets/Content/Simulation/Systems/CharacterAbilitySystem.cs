using Common;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum
{
    [Preserve]
    public unsafe class CharacterAbilitySystem : SystemMainThreadFilter<CharacterAbilitySystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Character* Character;
            public Transform3D* Transform;
            public AbilityAction* Action;
        }

        public static void Perform(Frame f, EntityRef entity, AssetRef<WeaponConfig> weapon)
        {
            var config = f.FindAsset<WeaponConfig>(weapon);
            Perform(f, entity, config.Combo[0]);
        }
        public static void Perform(Frame f, EntityRef entity, AssetRef<AbilityConfig> ability)
        {
            if (!f.Unsafe.TryGetPointer(entity, out Character* character))
                return;

            f.Add(entity,
                new AbilityAction()
                {
                    Ability = ability,
                });

            Debug.Log($"Ability Start=TEST");
        }

        public override void Update(Frame f, ref Filter filter)
        {
            var ability = f.FindAsset(filter.Action->Ability);
            filter.Action->Progress += f.DeltaTime * ability.SpeedFactor;

            foreach (var animatedHitBox in ability.AnimatedHitBoxes)
            {
                switch (animatedHitBox.Shape)
                {
                    case Shape3DType.Sphere:
                        if (animatedHitBox.TryEvaluateProperty(HitBoxProperty.Position, filter.Action->Progress, out var position) &&
                            animatedHitBox.TryEvaluateProperty(HitBoxProperty.Scale, filter.Action->Progress, out var scale) &&
                            scale.X > 0)
                        {
                            position = filter.Transform->TransformPoint(position);
                            Utilities.DebugCircle(position, scale.X, Color.magenta, 0.75f);
                        }
                        break;
                }
            }

            if (filter.Action->Progress >= ability.Duration)
            {
                Debug.Log($"Ability End=TEST");
                f.Remove<AbilityAction>(filter.Entity);
            }
        }
    }
}
