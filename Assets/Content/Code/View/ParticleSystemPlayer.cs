using System.Collections;
using UnityEngine;

namespace ARPG.Unity
{
    // TODO: Incorporate pooling logic rather than simple destroy to avoide garbage collection
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleSystemPlayer : MonoBehaviour
    {
        private ParticleSystem _vfx;

        private void OnEnable()
        {
            _vfx = GetComponent<ParticleSystem>();
        }

        public void Play()
        {
            StartCoroutine(PlayRoutine());
        }

        private IEnumerator PlayRoutine()
        {
            _vfx.Play();
            while (_vfx.isPlaying)
                yield return null;

            Destroy(gameObject);
        }
    }
}