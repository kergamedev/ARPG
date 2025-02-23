using Quantum;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraHandler : MonoBehaviour
{
    [SerializeField]
    private QuantumEntityViewUpdater _viewUpdater;

    [SerializeField]
    private Vector3 _baseTargetOffset;

    [SerializeField]
    private float _distanceFromTarget;

    [SerializeField]
    private float _localFollowDamping;

    [SerializeField]
    private float _globalFollowDamping;

    [SerializeField, Range(0.0f, 1.0f)]
    private float _localToGlobalBlend;

    [SerializeField]
    private float _horizontalOrbitalAngle;

    [SerializeField]
    private float _verticalOrbitalAngle;

    [SerializeField]
    private float _lookAheadDistance;

    [SerializeField]
    private float _lookAheadDamping;

    private EntityRef _target;
    private Camera _camera;
    private Vector3 _lookAhead;
    private Vector3 _lookAheadVelocity;
    private Vector3 _localFollow;
    private Vector3 _localFollowVelocity;
    private Vector3 _globalFollow;
    private Vector3 _globalFollowVelocity;
    private Vector3 _lastTargetPosition;

    private void Awake()
    {
        _camera = GetComponent<Camera>();

        _localFollow = transform.position;
        _globalFollow = transform.position;
    }

    private void OnEnable()
    {
        QuantumEvent.Subscribe<EventPlayerCharacterSpawned>(this, OnPlayerCharacterSpawned, onlyIfActiveAndEnabled: true);
    }

    private void LateUpdate()
    {
        if (!_target.IsValid)
            return;

        var targetView = _viewUpdater.GetView(_target);
        var targetDelta = targetView.transform.position - _lastTargetPosition;

        var targetHasMoved = !Mathf.Approximately(targetDelta.magnitude, 0.0f);
        if (!Mathf.Approximately(targetDelta.magnitude, 0.0f))
        {
            var lookAheadGoal = targetDelta.normalized * _lookAheadDistance;
            _lookAhead = Vector3.SmoothDamp(_lookAhead, lookAheadGoal, ref _lookAheadVelocity, _lookAheadDamping, float.MaxValue, Time.deltaTime);
        }

        var spring = Quaternion.Euler(_horizontalOrbitalAngle, _verticalOrbitalAngle, 0.0f) * Vector3.forward;
        _camera.transform.rotation = Quaternion.LookRotation(-spring);

        var localFollowGoal = _baseTargetOffset + _lookAhead + (spring * _distanceFromTarget);
        _localFollow = Vector3.SmoothDamp(_localFollow, localFollowGoal, ref _localFollowVelocity, _localFollowDamping, float.MaxValue, Time.deltaTime);

        var globalFollowGoal = targetView.transform.position + localFollowGoal;
        _globalFollow = Vector3.SmoothDamp(_globalFollow, globalFollowGoal, ref _globalFollowVelocity, _globalFollowDamping, float.MaxValue, Time.deltaTime);

        _camera.transform.position = Vector3.Lerp(targetView.transform.position + _localFollow, _globalFollow, _localToGlobalBlend);
        _lastTargetPosition = targetView.transform.position;
    }

    private void OnPlayerCharacterSpawned(EventPlayerCharacterSpawned evt)
    {
        if (evt.Game.PlayerIsLocal(evt.Player))
            _target = evt.Character;
    }
}