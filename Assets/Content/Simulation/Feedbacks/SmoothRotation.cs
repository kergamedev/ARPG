using UnityEngine;

namespace Quantum
{
    public class SmoothRotation : MonoBehaviour
    {
        [SerializeField]
        private float _smoothing;

        Quaternion _rotation;

        private void OnEnable()
        {
            _rotation = transform.parent.rotation;
        }

        private void Update()
        {
            _rotation = Quaternion.Lerp(_rotation, transform.parent.rotation, _smoothing * Time.deltaTime);
            transform.rotation = _rotation;
        }
    }
}
