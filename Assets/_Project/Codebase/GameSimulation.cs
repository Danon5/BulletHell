using JetBrains.Annotations;
using UnityEngine;

namespace BulletHell
{
    public sealed class GameSimulation : MonoBehaviour
    {
        private const float SPAWN_INTERVAL = 1f;
        
        private float _lastSpawnTime;
        
        [UsedImplicitly]
        private void Start()
        {
            EntitySpawner.SpawnPlayerEntity();
        }

        [UsedImplicitly]
        private void Update()
        {
            if (GameTime.Time > _lastSpawnTime + SPAWN_INTERVAL)
            {
                EntitySpawner.SpawnEnemyEntity();
                
                _lastSpawnTime += SPAWN_INTERVAL;
            }
        }
    }
}