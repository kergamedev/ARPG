﻿using ARPG.Common;
using Photon.Deterministic;
using Quantum;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ARPG.Config
{
    public partial class WeaponConfig : AssetObject
    {
        [Serializable]
        public struct Attach
        {
            [field: SerializeField]
            public CharacterState ForState { get; private set; }

            [field: SerializeField]
            public RigAttach RigAttach { get; private set; }

            [field: SerializeField]
            public Vector3 LocalOffset { get; private set; }
        }

        [SerializeField]
        private AssetRef<AbilityConfig>[] _combo;

        [field: SerializeField]
        public FP ComboReset { get; private set; }

        public IReadOnlyList<AssetRef<AbilityConfig>> Combo => _combo;

        #if QUANTUM_UNITY

        [field: SerializeField]
        public GameObject ViewPrefab { get; private set; }

        [SerializeField]
        private Attach[] _viewAttaches;

        public IReadOnlyList<Attach> ViewAttaches => _viewAttaches;

        #endif
    }
}