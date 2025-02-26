using DG.Tweening;
using Quantum;
using Sirenix.OdinInspector;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using static UnityEngine.Rendering.ProbeAdjustmentVolume;

public class DummyView : QuantumEntityViewComponent, IEntityHitListener
{
    [SerializeField]
    private float _shakeDuration;

    [SerializeField]
    private int _shakeVibrato;

    [SerializeField]
    private float _shakeElasticity;

    [SerializeField]
    private float _shakeToScaleRatio;

    [SerializeField]
    private float _damageToShakeStrengthRatio;

    private Vector3 _shake;
    private Tween _shakeTween;

    void IEntityHitListener.OnEntityHit(EventEntityHit evt)
    {
        if (!PredictedFrame.TryGet(evt.Source, out Transform3D sourceTransform) ||
            !PredictedFrame.TryGet(evt.Victim, out Transform3D victimTransform))
            return;

        var direction = Vector3.ProjectOnPlane((victimTransform.Position - sourceTransform.Position).ToUnityVector3(), Vector3.up).normalized;
        Shake(direction, evt.Damage.AsFloat * _damageToShakeStrengthRatio);
    }

    [Button]
    private void Shake(Vector3 direction, float strength)
    {
        if (_shakeTween != null)
            _shakeTween.Kill();

        _shake = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        _shakeTween = DOTween.Punch(
            () => _shake,
            (value) =>
            {
                _shake = value;
                transform.localRotation = Quaternion.Euler(-_shake.z, 0.0f, _shake.x);
                transform.localScale = Vector3.one + (Vector3.one * Mathf.Max(0.0f, _shake.magnitude * _shakeToScaleRatio));
            },
            -direction * strength,
            _shakeDuration,
            _shakeVibrato,
            _shakeElasticity).OnComplete(() =>
            {
                _shakeTween = null;
                transform.localRotation = Quaternion.identity;
                transform.localScale = Vector3.one;
            });      
    }
}
