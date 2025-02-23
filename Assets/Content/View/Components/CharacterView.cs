using Quantum;
using UnityEngine;

public class CharacterView : QuantumEntityViewComponent
{
    private static readonly int IS_WALKING = Animator.StringToHash("IsWalking");
    private static readonly int IS_RUNNING = Animator.StringToHash("IsRunning");
    private static readonly int IS_SPRINTING = Animator.StringToHash("IsSprinting");
    private static readonly int ACTION_KIND = Animator.StringToHash("ActionKind");

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private float _rotationSmoothing;

    [SerializeField]
    private ParticleSystem _dashVFXPrefab;

    [SerializeField]
    private float _dashVFXHeightOffset;

    [SerializeField]
    private Vector3 _dashVFXSpawnOffset;

    Quaternion _rotation;

    public override void OnInitialize()
    {
        _rotation = transform.parent.rotation;

        QuantumEvent.Subscribe<EventCharacterMoved>(this, OnCharacterMoved, onlyIfActiveAndEnabled: true);
        QuantumEvent.Subscribe<EventCharacterDashed>(this, OnCharacterDashed, onlyIfActiveAndEnabled: true);
    }

    private void OnCharacterMoved(EventCharacterMoved evt)
    {
        if (evt.Character != EntityRef)
            return;

        if (PredictedFrame.Has<DashAction>(EntityRef))
        {
            _animator.SetBool(IS_WALKING, false);
            _animator.SetBool(IS_RUNNING, false);
            _animator.SetBool(IS_SPRINTING, false);
            _animator.SetInteger(ACTION_KIND, 2);
        }
        else SetAnimatorLocomotion(evt.LocomotionKind);
    }

    private void SetAnimatorLocomotion(LocomotionKind locomotionKind)
    {
        switch (locomotionKind)
        {
            case LocomotionKind.Idle:
                _animator.SetBool(IS_WALKING, false);
                _animator.SetBool(IS_RUNNING, false);
                _animator.SetBool(IS_SPRINTING, false);
                _animator.SetInteger(ACTION_KIND, 0);
                break;

            case LocomotionKind.Walk:
                _animator.SetBool(IS_WALKING, true);
                _animator.SetBool(IS_RUNNING, false);
                _animator.SetBool(IS_SPRINTING, false);
                _animator.SetInteger(ACTION_KIND, 1);
                break;

            case LocomotionKind.Run:
                _animator.SetBool(IS_WALKING, false);
                _animator.SetBool(IS_RUNNING, true);
                _animator.SetBool(IS_SPRINTING, false);
                _animator.SetInteger(ACTION_KIND, 1);
                break;

            case LocomotionKind.Sprint:
                _animator.SetBool(IS_WALKING, false);
                _animator.SetBool(IS_RUNNING, false);
                _animator.SetBool(IS_SPRINTING, true);
                _animator.SetInteger(ACTION_KIND, 1);
                break;
        }
    }

    private void OnCharacterDashed(EventCharacterDashed evt)
    {
        if (!PredictedFrame.TryGet(evt.Character, out DashAction dash))
            return;

        var dashStart = evt.Start.ToUnityVector3() + Vector3.up * _dashVFXHeightOffset;
        var dashEnd = dash.Destination.ToUnityVector3() + Vector3.up * _dashVFXHeightOffset;

        var vfx = Instantiate(_dashVFXPrefab);
        vfx.transform.rotation = Quaternion.FromToRotation(Vector3.forward, (dashEnd - dashStart).normalized);
        vfx.transform.position = dash.Destination.ToUnityVector3() + vfx.transform.TransformDirection(_dashVFXSpawnOffset) + (Vector3.up * _dashVFXHeightOffset);
        vfx.Play();
    }

    private void Update()
    {
        _rotation = Quaternion.Lerp(_rotation, transform.parent.rotation, _rotationSmoothing * Time.deltaTime);
        transform.rotation = _rotation;
    }
}
