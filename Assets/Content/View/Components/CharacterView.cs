using Quantum;
using System;
using UnityEngine;

public class CharacterView : QuantumEntityViewComponent
{
    private static readonly int ANIM_IDLE = Animator.StringToHash("Main.Idle");
    private static readonly int ANIM_WALK = Animator.StringToHash("Main.Walk");
    private static readonly int ANIM_RUN = Animator.StringToHash("Main.Run");
    private static readonly int ANIM_SPRINT = Animator.StringToHash("Main.Sprint");
    private static readonly int ANIM_DASH = Animator.StringToHash("Main.Dash");

    [SerializeField]
    private float _rotationSmoothing;

    [Header("Dash")]
    [SerializeField]
    private ParticleSystem _dashVFXPrefab;

    [SerializeField]
    private float _dashVFXHeightOffset;

    [SerializeField]
    private Vector3 _dashVFXSpawnOffset;

    private Animator _animator;
    private CharacterState _lastCharacterState;
    private LocomotionKind _lastOngoingLocomotion;
    Quaternion _rotation;


    public override void OnInitialize()
    {
        _animator = GetComponentInChildren<Animator>();
        _rotation = transform.parent.rotation;

        QuantumEvent.Subscribe<EventCharacterDashed>(this, OnCharacterDashed, onlyIfActiveAndEnabled: true);
    }

    public override void OnUpdateView()
    {
        var character = PredictedFrame.Get<Character>(EntityRef);
        var changedStateThisTick = character.State != _lastCharacterState;

        switch (character.State)
        {
            case CharacterState.Locomotion:
                UpdateLocomotionAnimations(character, changedStateThisTick);
                break;

            case CharacterState.Dashing:
                UpdateDashAnimations(character, changedStateThisTick);
                break;
        }

        _lastCharacterState = character.State;
    }

    private void UpdateLocomotionAnimations(Character character, bool changedStateThisTick)
    {
        var ongoingLocomotion = character.OngoingLocomotion;
        if (changedStateThisTick || ongoingLocomotion != _lastOngoingLocomotion)
        {
            var locomotionAnim = default(int);
            switch (character.OngoingLocomotion)
            {
                case LocomotionKind.Idle:
                    locomotionAnim = ANIM_IDLE;
                    break;

                case LocomotionKind.Walk:
                    locomotionAnim = ANIM_WALK;
                    break;

                case LocomotionKind.Run:
                    locomotionAnim = ANIM_RUN;
                    break;

                case LocomotionKind.Sprint:
                    locomotionAnim = ANIM_SPRINT;
                    break;

                default: throw new Exception($"Unexpected '{nameof(LocomotionKind)}={character.OngoingLocomotion}'");
            }

            _animator.CrossFadeInFixedTime(locomotionAnim, 0.125f);
        }

        _lastOngoingLocomotion = ongoingLocomotion;
    }

    private void UpdateDashAnimations(Character character, bool changedStateThisTick)
    {
        if (!changedStateThisTick)
            return;

        _animator.PlayInFixedTime(ANIM_DASH);
    }

    private void OnCharacterDashed(EventCharacterDashed evt)
    {
        if (evt.Character != EntityRef)
            return;

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
