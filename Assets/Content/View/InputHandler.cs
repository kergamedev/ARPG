using Photon.Deterministic;
using Quantum;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField]
    private InputActionReference _moveInput;

    [SerializeField]
    private InputActionReference _useWeaponInput;

    [SerializeField]
    private InputActionReference _dashInput;

    private void OnEnable()
    {
        QuantumCallback.Subscribe<CallbackPollInput>(this, PollInput);
    }

    private void PollInput(CallbackPollInput callback)
    {
        var i = new Quantum.Input();

        i.Move = ComputeMoveInput();
        i.Dash = _dashInput.action.ReadValue<float>() != 0;
        i.UseWeapon = _useWeaponInput.action.ReadValue<float>() != 0;

        callback.SetInput(i, DeterministicInputFlags.Repeatable);
    }

    private FPVector2 ComputeMoveInput()
    {
        var input = _moveInput.action.ReadValue<Vector2>();

        var camera = Camera.main;
        var cameraForward = Vector3.ProjectOnPlane(camera.transform.forward, Vector3.up);

        var rotatedInput = Quaternion.FromToRotation(Vector3.forward, cameraForward) * new Vector3(input.x, 0.0f, input.y);
        return new Vector2(rotatedInput.x, rotatedInput.z).ToFPVector2();
    }
}
