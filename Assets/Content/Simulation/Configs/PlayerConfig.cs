using Photon.Deterministic;
using UnityEngine;

namespace Quantum
{
    public class PlayerConfig : AssetObject
    {
        [field: Header("Locomotion")]
        [field: SerializeField]
        public FP MoveSpeed { get; private set; }

        [field: SerializeField]
        public FP RunThreshold { get; private set; }

        [field: SerializeField]
        public FP WalkSpeedFactor { get; private set; }

        [field: SerializeField]
        public FP SprintSpeedFactor { get; private set; }

        [field: Header("Weapon")]
        [field: SerializeField]
        public AssetRef<WeaponConfig> StartingWeapon { get; private set; }

        [field: Header("Dash")]
        [field: SerializeField]
        public FP DashSpeed { get; private set; }

        [field: SerializeField]
        public FP DashCooldown { get; private set; }

        [field: SerializeField]
        public FP DashDistance { get; private set; }

        [field: SerializeField]
        public FP MaxDashLookAhead { get; private set; }

        [field: SerializeField]
        public FP DashLookAheadIncrement { get; private set; }
    }
}
