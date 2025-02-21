using Quantum;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineCamera))]
public class CameraHandler : MonoBehaviour
{
    [SerializeField]
    private QuantumEntityViewUpdater _viewUpdater;

    [SerializeField]
    private float _lookAhead;

    [SerializeField]
    private float _lookAheadSmoothing;

    private EntityRef _playerRef;
    private CinemachineCamera _cinemachine;
    private Vector3 _worldOffset;
    private Vector3 _worldOffsetSmoothingVelocity;
    private CinemachineCameraOffset _cinemachineLookAhead;

    private void Awake()
    {
        _cinemachine = GetComponent<CinemachineCamera>();
        _cinemachineLookAhead = GetComponent<CinemachineCameraOffset>();
    }

    private void OnEnable()
    {
        QuantumEvent.Subscribe<EventPlayerSpawned>(this, OnPlayerSpawned, onlyIfActiveAndEnabled: true);
        QuantumCallback.Subscribe<CallbackUpdateView>(this, OnViewUpdate);
    }

    private void OnViewUpdate(CallbackUpdateView callback)
    {
        if (!_playerRef.IsValid)
            return;

        if (!callback.Game.Frames.PreviousUpdatePredicted.TryGet(_playerRef, out Transform3D previousTransform))
            return;

        if (!callback.Game.Frames.Predicted.TryGet(_playerRef, out Transform3D currentTransform))
            return;

        var worldDelta = (currentTransform.Position - previousTransform.Position).ToUnityVector3();
        if (Mathf.Approximately(worldDelta.magnitude, 0.0f))
            return;

        var targetWorldOffset = worldDelta.normalized * _lookAhead;
        _worldOffset = Vector3.SmoothDamp(_worldOffset, targetWorldOffset, ref _worldOffsetSmoothingVelocity, _lookAheadSmoothing);

        var forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
        var axisAlignedOffset = Quaternion.Inverse(Quaternion.FromToRotation(Vector3.forward, forward)) * _worldOffset;
        _cinemachineLookAhead.Offset = new Vector3(axisAlignedOffset.x, axisAlignedOffset.z, axisAlignedOffset.z);
    }

    private void OnPlayerSpawned(EventPlayerSpawned evt)
    {
        if (!evt.Game.PlayerIsLocal(evt.Player))
            return;

        _playerRef = evt.Entity;

        var view = _viewUpdater.GetView(evt.Entity);
        _cinemachine.Target = new CameraTarget() { TrackingTarget = view.transform };
        _cinemachine.enabled = true;
    }
}
