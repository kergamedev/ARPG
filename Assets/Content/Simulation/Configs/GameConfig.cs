using UnityEngine;

namespace Quantum
{
    public class GameConfig : AssetObject
    {
        [field: SerializeField]
        public LayerMask FloorMask { get; private set; }

        [field: SerializeField]
        public AssetRef<PlayerConfig> PlayerConfig { get; private set; }
    }
}
