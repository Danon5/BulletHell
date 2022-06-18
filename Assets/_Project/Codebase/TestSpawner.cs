using BulletHell.ECS.Components.Singletons;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BulletHell
{
    public sealed class TestSpawner : MonoBehaviour
    {
        private void Start()
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            Entity gameDataEntity = entityManager.CreateEntityQuery(
                typeof(GameDataSingletonComponent)).GetSingletonEntity();

            GameDataSingletonComponent gameData = entityManager.GetComponentData<GameDataSingletonComponent>(gameDataEntity);
            
            for (int i = 0; i < 20000; i++)
            {
                Entity entity = entityManager.Instantiate(gameData.testPrefab);
                entityManager.SetComponentData(entity, new Translation
                {
                    Value = new float3
                    {
                        x = Random.Range(-20f, 20f),
                        y = Random.Range(-20f, 20f)
                    }
                });
            }
        }
    }
}