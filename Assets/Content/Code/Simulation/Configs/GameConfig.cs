using Photon.Deterministic;
using Quantum;
using UnityEngine;

namespace ARPG.Config
{
    public class GameConfig : AssetObject
    {
        [field: SerializeField]
        public UnityEngine.LayerMask FloorMask { get; private set; }

        [field: SerializeField]
        public UnityEngine.LayerMask CharacterMask { get; private set; }

        [field: SerializeField]
        public FP ClosestCharacterMaxSearchDistance { get; private set; }

        [field: SerializeField]
        public AssetRef<PlayerConfig> PlayerConfig { get; private set; }
    }
}
