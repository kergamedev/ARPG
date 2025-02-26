using Photon.Deterministic;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Quantum
{
    [Serializable]
    public class AnimatedHitBox
    {
        public AnimatedHitBox(Shape3DType shape)
        {
            Shape = shape;
            _animatedProperties = new List<HitBoxAnimatedProperty>();
        }

        [field: SerializeField]
        public Shape3DType Shape { get; private set; }

        [SerializeField]
        private List<HitBoxAnimatedProperty> _animatedProperties;

        public void AddProperty(HitBoxAnimatedProperty property)
        {
            if (_animatedProperties.Exists(existing => existing.Kind == property.Kind))
                return;

            _animatedProperties.Add(property);
        }

        public bool TryEvaluateProperty(HitBoxProperty kind, FP time, out FPVector3 value)
        {
            foreach (var animatedProperty in _animatedProperties)
            {
                if (animatedProperty.Kind != kind)
                    continue;

                var x = animatedProperty.X.Evaluate(time);
                var y = animatedProperty.Y.Evaluate(time);
                var z = animatedProperty.Z.Evaluate(time);
                value = new FPVector3(x, y, z);
                return true;
            }

            value = default;
            return false;
        }

        public bool TryEvaluateShape(FP time, out Transform3D transform, out Shape3D shape)
        {
            transform = new Transform3D();
            transform.Position = FPVector3.Zero;
            transform.Rotation = FPQuaternion.Identity;

            shape = default;

            if (!TryEvaluateProperty(HitBoxProperty.Position, time, out var position))
                return false;

            transform.Position = position;

            if (TryEvaluateProperty(HitBoxProperty.Rotation, time, out var rotation))
                transform.Rotation = FPQuaternion.Euler(rotation);

            switch (Shape)
            {
                case Shape3DType.Sphere:
                    if (TryEvaluateProperty(HitBoxProperty.Scale, time, out var scale) && scale != FPVector3.Zero)
                    {
                        shape = Shape3D.CreateSphere(FPMath.Max(scale.X, scale.Y, scale.Z));
                        return true;
                    }
                    else return false;

                default: return false;
            }
        }
    }

    [Serializable]
    public struct HitBoxAnimatedProperty
    {
        public HitBoxAnimatedProperty(HitBoxProperty kind, FPAnimationCurve x, FPAnimationCurve y, FPAnimationCurve z)
        {
            Kind = kind;
            X = x;
            Y = y;
            Z = z;
        }

        [field: SerializeField]
        public HitBoxProperty Kind { get; private set; }

        [field: SerializeField]
        public FPAnimationCurve X { get; private set; }

        [field: SerializeField]
        public FPAnimationCurve Y { get; private set; }

        [field: SerializeField]
        public FPAnimationCurve Z { get; private set; }
    }

    public enum HitBoxProperty
    {
        Position,
        Rotation,
        Scale
    }
}