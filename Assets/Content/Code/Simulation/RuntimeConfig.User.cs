using ARPG.Config;
using UnityEngine;

namespace Quantum
{
    public partial class RuntimeConfig
    {
        [field: SerializeField]
        public AssetRef<GameConfig> GameConfig { get; private set; }

        public GameConfig GetGameConfig(Frame f)
        {
            return f.FindAsset(GameConfig);
        }

        public PlayerConfig GetPlayerConfig(Frame f)
        {
            return f.FindAsset(GetGameConfig(f).PlayerConfig);
        }
    }
}