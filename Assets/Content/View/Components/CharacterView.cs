using Common;
using Quantum;
using System;
using System.Linq;
using UnityEngine;

public class CharacterView : QuantumEntityViewComponent
{
    [Serializable]
    private struct RigAttachToTransform
    {
        [field: SerializeField]
        public RigAttach Kind { get; private set; }

        [field: SerializeField]
        public Transform Transform { get; private set; }
    }

    private static readonly int ANIM_IDLE = Animator.StringToHash("Main.Idle");
    private static readonly int ANIM_WALK = Animator.StringToHash("Main.Walk");
    private static readonly int ANIM_RUN = Animator.StringToHash("Main.Run");
    private static readonly int ANIM_SPRINT = Animator.StringToHash("Main.Sprint");
    private static readonly int ANIM_DASH = Animator.StringToHash("Main.Dash");

    [SerializeField]
    private float _rotationSmoothing;

    [Header("Dash")]
    [SerializeField]
    private ParticleSystemPlayer _dashVFXPrefab;

    [SerializeField]
    private float _dashVFXHeightOffset;

    [SerializeField]
    private Vector3 _dashVFXSpawnOffset;

    [Space]
    [SerializeField]
    private RigAttachToTransform[] _attaches;

    private Animator _animator;
    private GameObject _weaponView;
    private WeaponConfig _lastWeapon;
    private CharacterState _lastCharacterState;
    private LocomotionKind _lastOngoingLocomotion;
    private int _lastAbilityStartTick;
    private Quaternion _rotation;


    public override void OnInitialize()
    {
        _animator = GetComponentInChildren<Animator>();
        _rotation = transform.parent.rotation;

        QuantumEvent.Subscribe<EventCharacterDashed>(this, OnCharacterDashed, onlyIfActiveAndEnabled: true);
    }

    public override void OnUpdateView()
    {
        var character = PredictedFrame.Get<Character>(EntityRef);

        var currentWeapon = QuantumUnityDB.GetGlobalAsset(character.Weapon);
        if (currentWeapon != _lastWeapon)
        {       
            if (_weaponView != null)
            {
                Destroy(_weaponView);
                _weaponView = null;
            }

            if (character.Weapon.IsValid)
            {
                _weaponView = Instantiate(currentWeapon.ViewPrefab);
                AttachWeaponView(character, currentWeapon);
            }
        }

        _lastWeapon = currentWeapon;

        var changedStateThisTick = character.State != _lastCharacterState;
        if (changedStateThisTick)
            PrepareForNewCharacterState(character);

        switch (character.State)
        {
            case CharacterState.Locomotion:
                UpdateLocomotionAnimations(character, changedStateThisTick);
                break;

            case CharacterState.Dashing:
                UpdateDashAnimations(character, changedStateThisTick);
                break;

            case CharacterState.InAbility:
                UpdateAbilityAnimations(character, changedStateThisTick);
                break;
        }
     
        _lastCharacterState = character.State;
    }

    private void PrepareForNewCharacterState(Character character)
    {
        _lastOngoingLocomotion = default;
        _lastAbilityStartTick = -1;

        AttachWeaponView(character, _lastWeapon);
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

    private void UpdateAbilityAnimations(Character character, bool changedStateThisTick)
    {
        if (!PredictedFrame.TryGet(EntityRef, out AbilityAction action))
            return;

        var currentAbility = QuantumUnityDB.GetGlobalAsset(action.Ability);
        if (changedStateThisTick || action.StartTick != _lastAbilityStartTick)
        {
            _animator.Rebind();
            _animator.PlayInFixedTime(currentAbility.Animation);
        }

        _lastAbilityStartTick = action.StartTick;
    }

    private void AttachWeaponView(Character character, WeaponConfig weapon)
    {
        if (_weaponView == null)
            return;

        var weaponAttach = default(WeaponConfig.Attach?);
        foreach (var attach in weapon.ViewAttaches)
        {
            if (attach.ForState != character.State)
                continue;
            
            weaponAttach = attach;
            break;
        }

        if (weaponAttach == null)
            weaponAttach = weapon.ViewAttaches.FirstOrDefault(attach => attach.ForState == CharacterState.None);

        if (weaponAttach == null)
        {
            _weaponView.SetActive(false);
            return;
        }

        var matchIndex = Array.FindIndex(_attaches, attach => attach.Kind == weaponAttach.Value.RigAttach);
        if (matchIndex == -1)
        {
            _weaponView.SetActive(false);
            return;
        }

        var match = _attaches[matchIndex];

        _weaponView.SetActive(true);
        _weaponView.transform.parent = match.Transform;
        _weaponView.transform.localRotation = Quaternion.identity;
        _weaponView.transform.localPosition = weaponAttach.Value.LocalOffset;
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

    private void OnAnimationEvent(string value)
    {
        // NO-OP
    }
}
