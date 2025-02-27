using ARPG.Common;
using Quantum;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ARPG.Unity
{
    // TODO: Figure out if it's better for individual views to subscribe to callbacks and check if an event concerns them
    // OR if it's better to have a dispatch logic like that
    public class EventHandler : MonoBehaviour
    {
        [SerializeField]
        private QuantumEntityViewUpdater _viewUpdater;

        // TODO: Prototype version has only one hit VFX but ideally the hit event would specify something that is able to figure out which VFX to use
        // Same goes for dashing
        [SerializeField, FoldoutGroup("VFX")]
        private ParticleSystemPlayer _hitVFXPrefab;

        [SerializeField, FoldoutGroup("VFX")]
        private RigAttach _hitVFXAttach;

        private void OnEnable()
        {
            QuantumEvent.Subscribe<EventCharacterDashed>(this, OnCharacterDashed, onlyIfActiveAndEnabled: true);
            QuantumEvent.Subscribe<EventEntityHit>(this, OnEntityHit, onlyIfActiveAndEnabled: true);
        }

        private void OnCharacterDashed(EventCharacterDashed evt)
        {
            var view = _viewUpdater.GetView(evt.Character);
            var listener = view.GetComponentInChildren<ICharacterDashedListener>();
            listener?.OnCharacterDashed(evt);
        }

        private void OnEntityHit(EventEntityHit evt)
        {
            var entityView = _viewUpdater.GetView(evt.Victim);

            var characterView = entityView.GetComponentInChildren<CharacterView>();
            if (characterView != null && characterView.TryGetAttach(_hitVFXAttach, out var transform))
            {
                var vfx = Instantiate(_hitVFXPrefab);
                vfx.transform.parent = transform;
                vfx.transform.localPosition = Vector3.zero;
                vfx.Play();
            }

            var listener = entityView.GetComponentInChildren<IEntityHitListener>();
            listener?.OnEntityHit(evt);
        }
    }

    public interface ICharacterDashedListener
    {
        void OnCharacterDashed(EventCharacterDashed evt);
    }

    public interface IEntityHitListener
    {
        void OnEntityHit(EventEntityHit evt);
    }
}