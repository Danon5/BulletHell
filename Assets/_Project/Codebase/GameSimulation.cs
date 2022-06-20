using BulletHell.ECS.Components.Singletons;
using JetBrains.Annotations;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace BulletHell
{
    public sealed class GameSimulation : MonoBehaviour
    {
        private EntityManager _entityManager;

        [UsedImplicitly]
        private void Start()
        {
            Application.targetFrameRate = GameConstants.TARGET_FRAMERATE;
            QualitySettings.vSyncCount = 0;
            GraphicsSettings.transparencySortMode = TransparencySortMode.CustomAxis;
            GraphicsSettings.transparencySortAxis = Vector3.up;

            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            Entity gameDataEntity = _entityManager.CreateEntityQuery(
                typeof(EntityPrefabsSingletonComponent)).GetSingletonEntity();

            EntityPrefabsSingletonComponent entityPrefabs =
                _entityManager.GetComponentData<EntityPrefabsSingletonComponent>(gameDataEntity);

            _entityManager.Instantiate(entityPrefabs.playerPrefab);

            for (int i = 0; i < 400; i++)
            {
                Entity enemy = _entityManager.Instantiate(entityPrefabs.enemyPrefab);
                _entityManager.SetComponentData(enemy, new Translation
                {
                    Value = new float3
                    {
                        x = Random.Range(-50f, 50f),
                        y = Random.Range(-50f, 50f)
                    }
                });
            }
        }
    }
}