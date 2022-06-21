using System;
using BulletHell.ECS.Components;
using BulletHell.ECS.SharedData;
using BulletHell.ECS.Tags;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace BulletHell
{
    public sealed class EntitySpawner : MonoBehaviour
    {
        private static EntitySpawner _singleton;
        private static EntityManager _entityManager;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void InitializeOnLoad()
        {
            _singleton = null;
        }
        
        private void Awake()
        {
            _singleton = this;
            _entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        public static Entity SpawnPlayerEntity()
        {
            Entity playerEntity = _entityManager.CreateEntity();
            
            AddTransformComponentsToEntity(playerEntity);

            _entityManager.AddComponentData(playerEntity, new PlayerComponent());
            _entityManager.AddComponentData(playerEntity, new CameraFollowTargetComponent());
            _entityManager.AddComponentData(playerEntity, new RigidbodyComponent());
            _entityManager.AddComponentData(playerEntity, new SpriteAutoFlipXTag());
            _entityManager.AddComponentData(playerEntity, new SpriteComponent());
            _entityManager.AddSharedComponentData(playerEntity, new SpriteSharedData
            {
                textureId = TextureId.Character_Havoc_Default,
                spriteSheetColumnsRows = new int2(1, 1),
                spriteOriginOffset = new int2(0, 12)
            });

            return playerEntity;
        }

        public static void AddTransformComponentsToEntity(in Entity entity)
        {
            _entityManager.AddComponentData(entity, new Translation());
            _entityManager.AddComponentData(entity, new Rotation());
            _entityManager.AddComponentData(entity, new NonUniformScale
            {
                Value = new float3(1f, 1f, 1f)
            });
            _entityManager.AddComponentData(entity, new LocalToWorld());
        }
    }
}