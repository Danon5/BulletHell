using BulletHell.ECS.Components;
using BulletHell.ECS.SharedData;
using JetBrains.Annotations;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BulletHell
{
    public sealed class TestSpawner : MonoBehaviour
    {
        
        [UsedImplicitly]
        private void Start()
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            for (int i = 0; i < 100; i++)
            {
                Entity entity = entityManager.CreateEntity();

                entityManager.AddComponent<Translation>(entity);
                entityManager.AddComponent<Rotation>(entity);
                entityManager.AddComponent<LocalToWorld>(entity);
                entityManager.AddComponent<SpriteComponent>(entity);
                entityManager.AddComponent<SpriteSharedData>(entity);
                
                entityManager.SetSharedComponentData(entity, new SpriteSharedData
                {
                    textureId = (TextureId) Random.Range(0, 5)
                });
                
                entityManager.SetComponentData(entity, new Translation
                {
                    Value = new float3
                    {
                        x = Random.Range(-40f, 40f),
                        y = Random.Range(-20f, 20f)
                    }
                });
            }
        }
    }
}