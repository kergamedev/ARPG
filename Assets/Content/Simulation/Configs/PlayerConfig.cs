using Photon.Deterministic;
using UnityEngine;

namespace Quantum
{
    public class PlayerConfig : AssetObject
    {
        [field: SerializeField]
        public FP MoveSpeed { get; private set; }
    }
}
