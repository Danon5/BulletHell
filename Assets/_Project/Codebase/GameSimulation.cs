using JetBrains.Annotations;
using UnityEngine;

namespace BulletHell
{
    public sealed class GameSimulation : MonoBehaviour
    {
        [UsedImplicitly]
        private void Start()
        {
            Application.targetFrameRate = GameConstants.TARGET_FRAMERATE;
            QualitySettings.vSyncCount = 0;

            EntitySpawner.SpawnPlayerEntity();
        }
    }
}