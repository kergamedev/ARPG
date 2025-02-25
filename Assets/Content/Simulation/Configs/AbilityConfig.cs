using Common;
using Photon.Deterministic;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Quantum
{
    public partial class AbilityConfig : AssetObject
    {
        [field: SerializeField]
        [field: ContextMenuItem("Bake", "EDITOR_BakeAnimation")]
        public int Animation { get; private set; }

        [field: SerializeField]
        public FP Duration { get; private set; }

        [field: SerializeField]
        public FP SpeedFactor { get; private set; }

        [SerializeField]
        private AnimatedHitBox[] _animatedHitBoxes;

        public IReadOnlyList<AnimatedHitBox> AnimatedHitBoxes => _animatedHitBoxes;

        #if UNITY_EDITOR

        private void EDITOR_BakeAnimation()
        {
            if (!UnityEditor.EditorWindow.HasOpenInstances<UnityEditor.AnimationWindow>())
            {
                Debug.LogWarning("The animation window must be open for starting the baking process");
                return;
            }

            var animator = UnityEditor.Selection.activeGameObject == null ? null : UnityEditor.Selection.activeGameObject.GetComponentInParent<Animator>();
            if (animator == null)
            {
                Debug.LogWarning("An animator's hierarchy must be selected for starting the baking process");
                return;
            }


            UnityEditor.AssetDatabase.StartAssetEditing();

            try
            {
                var animation = UnityEditor.EditorWindow.GetWindow<UnityEditor.AnimationWindow>().animationClip;
                var animatorController = (UnityEditor.Animations.AnimatorController)animator.runtimeAnimatorController;
                var mainAnimationLayer = animatorController.layers[0];
                var animationState = mainAnimationLayer.stateMachine.states.First(entry => entry.state.motion == animation).state;

                Animation = Animator.StringToHash($"{mainAnimationLayer.name}.{animationState.name}");
                Duration = FP.FromFloat_UNSAFE(animation.length);
                SpeedFactor = FP.FromFloat_UNSAFE(animationState.speed);

                var animatedHitBoxes = new Dictionary<string, (Shape3DType Shape, Dictionary<string, FPAnimationCurve> Animations)>();
                var bindings = UnityEditor.AnimationUtility.GetCurveBindings(animation);
                foreach (var binding in bindings)
                {
                    if (!binding.path.Contains("HitBox"))
                        continue;

                    if (!animator.transform.TryFindChild(binding.path, out var transform))
                        continue;

                    if (!transform.TryGetComponent(out Collider collider))
                        continue;

                    if (!animatedHitBoxes.TryGetValue(binding.path, out var data))
                    {
                        var shape = default(Shape3DType);
                        if (collider is SphereCollider)
                            shape = Shape3DType.Sphere;
                        else if (collider is BoxCollider)
                            shape = Shape3DType.Box;
                        else if (collider is CapsuleCollider)
                            shape = Shape3DType.Capsule;
                        else
                        {
                            Debug.LogWarning($"Caught unhandled 'Collider={collider.GetType().Name}'. Skipping it...");
                            continue;
                        }

                        var animations = new Dictionary<string, FPAnimationCurve>();
                        data = (shape, animations);

                        animatedHitBoxes.Add(binding.path, data);
                    }

                    var curve = UnityEditor.AnimationUtility.GetEditorCurve(animation, binding);
                    data.Animations.Add(binding.propertyName, curve.EDITOR_ToFPCurve());
                }

                _animatedHitBoxes = new AnimatedHitBox[animatedHitBoxes.Count];

                var index = 0;
                foreach (var data in animatedHitBoxes.Values)
                {
                    var animatedHitBox = new AnimatedHitBox(data.Shape);

                    if (data.Animations.TryGetValue("m_LocalPosition.x", out var positionX) &&
                        data.Animations.TryGetValue("m_LocalPosition.y", out var positionY) &&
                        data.Animations.TryGetValue("m_LocalPosition.z", out var positionZ))
                    {
                        animatedHitBox.AddProperty(new HitBoxAnimatedProperty(HitBoxProperty.Position, positionX, positionY, positionZ));
                    }

                    if (data.Animations.TryGetValue("m_LocalRotation.x", out var rotationX) &&
                        data.Animations.TryGetValue("m_LocalRotation.y", out var rotationY) &&
                        data.Animations.TryGetValue("m_LocalRotation.z", out var rotationZ))
                    {
                        animatedHitBox.AddProperty(new HitBoxAnimatedProperty(HitBoxProperty.Rotation, rotationX, rotationY, rotationZ));
                    }

                    if (data.Animations.TryGetValue("m_LocalScale.x", out var scaleX) &&
                        data.Animations.TryGetValue("m_LocalScale.y", out var scaleY) &&
                        data.Animations.TryGetValue("m_LocalScale.z", out var scaleZ))
                    {
                        animatedHitBox.AddProperty(new HitBoxAnimatedProperty(HitBoxProperty.Scale, scaleX, scaleY, scaleZ));
                    }

                    _animatedHitBoxes[index] = animatedHitBox;
                    index++;

                    Debug.Log($"Successfuly baked {_animatedHitBoxes.Length} hitboxes");
                }
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
            finally
            {
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEditor.AssetDatabase.StopAssetEditing();
                UnityEditor.AssetDatabase.SaveAssetIfDirty(this);
                UnityEditor.AssetDatabase.Refresh();
            }      
        }

        #endif
    }
}