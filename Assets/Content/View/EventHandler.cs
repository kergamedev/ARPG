using Common;
using Quantum;
using Sirenix.OdinInspector;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    [SerializeField]
    private QuantumEntityViewUpdater _viewUpdater;

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
